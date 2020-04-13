using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
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

        private readonly IRoleUowService _roleUow;

        private readonly IMapper _mapper;

        public RoleAppService(ISysRoleRepository roleRep, ISysModuleRepository moduleRep, IRoleUowService roleUow, IMapper mapper)
        {
            _roleRep = roleRep;
            _mapper = mapper;
            _moduleRep = moduleRep;
            _roleUow = roleUow;
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
            var result = _mapper.Map<List<RoleVM>>(roles);
            var roleUsers = await _roleRep.GetRoleUserCount();
            foreach (var item in result)
            {
                item.UserCount = roleUsers.FirstOrDefault(a => a.RoleId == item.Id)?.UserCount;
            }
            return result;
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task UpdateRoleAuth(long id, RoleUpdateRequest vm)
        {
            var role = await _roleRep.GetAysnc(id);
            var modules = await _moduleRep.GetListAysnc(vm.ModuleIds);
            var maxLayer = modules.Max(a => a.Layer);

            List<long> authNumbers = new List<long>();
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
            _roleUow.UpdateAuth(role);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteRole(long id)
        {
            await _roleRep.DeleteAysnc(id);
        }


    }
}

