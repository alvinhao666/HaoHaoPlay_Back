using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.Core;
using Hao.Enum;
using Hao.EventData;
using Hao.Library;
using Hao.Model;
using Hao.Runtime;
using Hao.Utility;
using Mapster;
using Newtonsoft.Json;
using Npgsql;

namespace Hao.AppService
{
    /// <summary>
    /// 角色应用服务
    /// </summary>
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly ISysRoleRepository _roleRep;

        private readonly ISysModuleRepository _moduleRep;

        private readonly ISysUserRepository _userRep;

        private readonly ICapPublisher _publisher;

        private readonly ICurrentUser _currentUser;

        public RoleAppService(ICurrentUser currentUser,ICapPublisher publisher,ISysRoleRepository roleRep, ISysModuleRepository moduleRep, ISysUserRepository userRep)
        {
            _roleRep = roleRep;
            _moduleRep = moduleRep;
            _userRep = userRep;
            _publisher = publisher;
            _currentUser = currentUser;
        }


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task Add(RoleAddRequest vm)
        {
            var role = new SysRole() { Name = vm.Name };
            try
            {
                await _roleRep.InsertAsync(role);
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == H_PostgresSqlState.E23505) throw new H_Exception("角色名称已存在，请重新输入");//违反唯一键
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleVM>> GetList()
        {
            var query = new RoleQuery();
            
            if (_currentUser.RoleLevel != (int)RoleLevelType.SuperAdministrator) //超级管理员能看见所有，其他用户只能看见比自己等级低的用户列表
            {
                query.CurrentRoleLevel = _currentUser.RoleLevel;
            }

            var roles = await _roleRep.GetRoleList(query);

            var result = roles.Adapt<List<RoleVM>>();

            return result;
        }

        /// <summary>
        /// 根据当前用户角色，获取可以操作得角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleSelectVM>> GetRoleListByCurrentRole()
        {
            var query = new RoleQuery()
            {
                CurrentRoleLevel = _currentUser.RoleLevel
            };
            
            query.OrderBy(a=>a.Level);
            
            var roles = await _roleRep.GetListAsync(query);

            var result = roles.Adapt<List<RoleSelectVM>>();
            return result;
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [CapUnitOfWork]
        public async Task UpdateRoleAuth(long id, RoleUpdateRequest vm)
        {
            var role = await GetRoleDetail(id);

            if (role.Level != (int)RoleLevelType.SuperAdministrator && _currentUser.RoleLevel >= role.Level) throw new H_Exception("无法操作该角色的权限");

            var modules = await _moduleRep.GetListAsync(vm.ModuleIds);
            var maxLayer = modules.Max(a => a.Layer);

            var authNumbers = new List<long>();
            for (int i = 1; i <= maxLayer; i++)
            {
                var authNumber = 0L;
                var items = modules.Where(a => a.Layer == i);
                foreach (var x in items)
                {
                    authNumber = authNumber | x.Number.Value;
                }
                authNumbers.Add(authNumber);
            }

            role.AuthNumbers = JsonConvert.SerializeObject(authNumbers);
            var users = await _userRep.GetListAsync(new UserQuery() { RoleLevel = role.Level });
            var ids = users.Where(a => a.AuthNumbers != role.AuthNumbers).Select(a => a.Id).ToList();

            await _roleRep.UpdateAsync(role, a => new { a.AuthNumbers });
            await _userRep.UpdateAuth(role.Id, role.AuthNumbers);

            //注销该角色下用户的登录信息

            if (ids.Count < 1) return;
            await _publisher.PublishAsync(nameof(LogoutForUpdateAuthEventData), new LogoutForUpdateAuthEventData
            {
                UserIds = ids
            });
        }

        /// <summary>
        /// 获取角色拥有的模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleModuleVM> GetRoleModule(long id)
        {
            var role = await GetRoleDetail(id);
            var authNumbers = string.IsNullOrWhiteSpace(role.AuthNumbers) ? null : JsonConvert.DeserializeObject<List<long>>(role.AuthNumbers);
            var modules = await _moduleRep.GetListAsync();
            var result = new RoleModuleVM();
            result.Nodes = new List<RoleModuleItemVM>();
            result.CheckedKeys = new List<string>();
            InitModuleTree(result.Nodes, -1, modules, authNumbers, result.CheckedKeys);
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            var role = await GetRoleDetail(id);
            var users = await _userRep.GetListAsync(new UserQuery() { RoleLevel = role.Level });
            if (users.Count > 0) throw new H_Exception("该角色下存在用户，暂时无法删除");
            await _roleRep.DeleteAsync(role);
        }


        #region private
        /// <summary>
        /// 角色详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<SysRole> GetRoleDetail(long userId)
        {
            var item = await _roleRep.GetAsync(userId);
            if (item == null) throw new H_Exception("角色不存在");
            if (item.IsDeleted) throw new H_Exception("角色已删除");
            return item;
        }

        /// <summary>
        /// 递归初始化模块树
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parentID"></param>
        /// <param name="sources"></param>
        /// <param name="authNumbers"></param>
        /// <param name="checkedKeys"></param>
        private void InitModuleTree(List<RoleModuleItemVM> result, long parentID, List<SysModule> sources, List<long> authNumbers, List<string> checkedKeys)
        {
            //递归寻找子节点  
            var tempTree = sources.Where(item => item.ParentId == parentID).OrderBy(a => a.Sort).ToList();
            foreach (var item in tempTree)
            {
                var node = new RoleModuleItemVM()
                {
                    key = item.Id.ToString(),
                    title = item.Name,
                    isLeaf = item.Type == ModuleType.Resource,
                    expanded = (int)item.Type.Value < (int)ModuleType.Sub,
                    children = new List<RoleModuleItemVM>()
                };
 
                result.Add(node);

                InitModuleTree(node.children, item.Id, sources, authNumbers,checkedKeys);

                if (item.Type == ModuleType.Sub) node.isLeaf = node.children.Count == 0;

                if (node.isLeaf && authNumbers?.Count > 0 && item.Layer.Value <= authNumbers.Count)
                {
                    if ((authNumbers[item.Layer.Value - 1] & item.Number) == item.Number)
                    {
                        checkedKeys.Add(node.key);
                    }
                }
                
                if (item.Type == ModuleType.Main && node.children.Count < 1) result.Remove(node);
            }
        }
        #endregion
    }
}

