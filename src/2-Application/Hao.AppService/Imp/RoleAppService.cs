using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.CAP;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Enum;
using Hao.EventData;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using Newtonsoft.Json;
using Npgsql;

namespace Hao.AppService
{
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly ISysRoleRepository _roleRep;

        private readonly ISysModuleRepository _moduleRep;

        private readonly ISysUserRepository _userRep;

        private readonly IMapper _mapper;

        private readonly ICapPublisher _publisher;

        public RoleAppService(ICapPublisher publisher,ISysRoleRepository roleRep, ISysModuleRepository moduleRep, ISysUserRepository userRep, IMapper mapper)
        {
            _roleRep = roleRep;
            _mapper = mapper;
            _moduleRep = moduleRep;
            _userRep = userRep;
            _publisher = publisher;
        }


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task AddRole(RoleAddRequest vm)
        {
            var role = new SysRole() { Name = vm.Name };
            try
            {
                await _roleRep.InsertAysnc(role);
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23505") throw new HException("角色名称已存在，请重新输入");//违反唯一键
            }
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleVM>> GetRoleList()
        {
            var roles = await _roleRep.GetListAysnc();
            roles = roles.OrderBy(a => a.Sort).ToList();
            var result = _mapper.Map<List<RoleVM>>(roles);
            var roleUsers = await _roleRep.GetRoleUserCount();
            foreach (var item in result)
            {
                item.UserCount = roleUsers.FirstOrDefault(a => a.RoleId == item.Id)?.UserCount ?? 0;
            }
            return result;
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task UpdateRoleAuth(long id, RoleUpdateRequest vm)
        {
            var role = await GetRoleDetail(id);
            var modules = await _moduleRep.GetListAysnc(vm.ModuleIds.Select(a=>long.Parse(a)).ToList());
            var maxLayer = modules.Max(a => a.Layer);

            var authNumbers = new List<long>();
            for (int i = 1; i <= maxLayer; i++)
            {
                var authNumber = 0L;
                var items = modules.Where(a => a.Layer == i);
                foreach (var x in items)
                {
                    authNumber = authNumber | x.Number;
                }
                authNumbers.Add(authNumber);
            }

            role.AuthNumbers = JsonConvert.SerializeObject(authNumbers);
            var users = await _userRep.GetListAysnc(new UserQuery() { RoleId = role.Id });
            var ids = users.Where(a => a.AuthNumbers != role.AuthNumbers).Select(a => a.Id).ToList();

            await _roleRep.UpdateAsync(role, a => new { a.AuthNumbers });
            await _userRep.UpdateAuth(role.Id, role.AuthNumbers);

            //注销该角色下用户的登录信息

            if (ids.Count < 1) return;
            await _publisher.PublishAsync(nameof(LogoutEventData), new LogoutEventData
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
            var modules = await _moduleRep.GetListAysnc();
            var result = new RoleModuleVM();
            result.Nodes = new List<RoleModuleItemVM>();
            result.CheckedKeys = new List<string>();
            InitModuleTree(result.Nodes, null, modules, authNumbers, result.CheckedKeys);
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteRole(long id)
        {
            var users = await _userRep.GetListAysnc(new UserQuery() { RoleId = id });
            if (users.Count > 0) throw new HException("该角色下存在用户，暂时无法删除");
            await _roleRep.DeleteAysnc(id);
        }


        #region private
        private async Task<SysRole> GetRoleDetail(long userId)
        {
            var item = await _roleRep.GetAysnc(userId);
            if (item == null) throw new HException("角色不存在");
            if (item.IsDeleted) throw new HException("角色已删除");
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
        private void InitModuleTree(List<RoleModuleItemVM> result, long? parentID, List<SysModule> sources, List<long> authNumbers, List<string> checkedKeys)
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
                if (node.isLeaf && authNumbers?.Count > 0 && item.Layer.Value <= authNumbers.Count ) 
                {
                    if((authNumbers[item.Layer.Value - 1] & item.Number) == item.Number)
                    {
                        checkedKeys.Add(node.key);
                    }    
                }
                result.Add(node);
                InitModuleTree(node.children, item.Id, sources, authNumbers,checkedKeys);
            }
        }
        #endregion
    }
}

