using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
using Hao.File;
using Hao.Library;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using Hao.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 用户应用服务
    /// </summary>
    public partial class UserAppService : ApplicationService, IUserAppService
    {

        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;

        private readonly ISysRoleRepository _roleRep;

        private readonly AppSettingsInfo _appsettings;

        private readonly ICurrentUser _currentUser;

        public UserAppService(ISysRoleRepository roleRep,
            IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, 
            ISysUserRepository userRepository,
            ISysLoginRecordRepository recordRep, 
            IMapper mapper,
            ICurrentUser currentUser)
        {
            _userRep = userRepository;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value;
            _currentUser = currentUser;
            _roleRep = roleRep;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task AddUser(UserAddRequest vm)
        {
            var users = await _userRep.GetListAysnc(new UserQuery()
            {
                LoginName = vm.LoginName
            });
            if (users.Count > 0) throw new HException("账号已存在，请重新输入");

            var role = await _roleRep.GetAysnc(vm.RoleId.Value);
            if (role == null) throw new HException("角色不存在，请重新添加");
            if (role.IsDeleted) throw new HException("角色已删除，请重新添加");
            if (role.Level <= _currentUser.RoleLevel) throw new HException("无法添加同级及高级角色用户");

            var user = _mapper.Map<SysUser>(vm);
            user.FirstNameSpell = H_Spell.GetFirstLetter(user.Name.ToCharArray()[0]);
            user.PasswordLevel = (PasswordLevel)H_Util.CheckPasswordLevel(user.Password);
            user.Password = EncryptProvider.HMACSHA256(user.Password, _appsettings.Key.Sha256Key);
            user.Enabled = true;
            user.RoleId = role.Id;
            user.RoleName = role.Name;
            user.AuthNumbers = role.AuthNumbers;
            user.RoleLevel = role.Level;
            await _userRep.InsertAysnc(user);
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vms"></param>
        /// <returns></returns>
        public async Task AddUsers(List<UserAddRequest> vms)
        {
            var users = _mapper.Map<List<SysUser>>(vms);
            foreach (var user in users)
            {
                user.FirstNameSpell = H_Spell.GetFirstLetter(user.Name.ToCharArray()[0]);
                user.Password = EncryptProvider.HMACSHA256(user.Password, _appsettings.Key.Sha256Key);
            }
            await _userRep.InsertAysnc(users);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<UserItemVM>> GetUserPageList(UserQuery query)
        {
            query.CurrentRoleLevel = _currentUser.RoleLevel; //只能获取角色等级低用户

            var users = await _userRep.GetPagedListAysnc(query);
            var result = _mapper.Map<PagedList<UserItemVM>>(users);

            return result;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDetailVM> GetUser(long id)
        {
            var user = await GetUserDetail(id);
            return _mapper.Map<UserDetailVM>(user);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUser(long userId)
        {
            CheckUser(userId);
            var user = await GetUserDetail(userId);
            await _userRep.DeleteAysnc(user.Id);
            await RedisHelper.DelAsync(_appsettings.RedisPrefix.LoginInfo + userId);
        }

        /// <summary>
        /// 注销/启用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task UpdateUserStatus(long userId, bool enabled)
        {
            CheckUser(userId);
            var user = await GetUserDetail(userId);
            user.Enabled = enabled;
            await _userRep.UpdateAsync(user, user => new { user.Enabled });
            // 注销用户，删除登录缓存
            if(!enabled) await RedisHelper.DelAsync(_appsettings.RedisPrefix.LoginInfo + user.Id);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task EditUser(long userId, UserUpdateRequest vm)
        {
            CheckUser(userId);
            var user = await GetUserDetail(userId);
            user.Name = vm.Name;
            user.Age = vm.Age;
            user.Gender = vm.Gender;
            user.Phone = vm.Phone;
            user.Email = vm.Email;
            user.WeChat = vm.WeChat;
            user.QQ = vm.QQ;
            await _userRep.UpdateAsync(user,
                user => new { user.Name, user.Age, user.Gender, user.Phone, user.Email, user.WeChat, user.QQ, user.RoleId, user.RoleName });
        }

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistUser(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query);
            return users.Count > 0;
        }


        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> ExportUsers(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query);

            var exportData = users.Select(a => new Dictionary<string, string>{
                {"姓名",a.Name},
                {"性别", a.Gender.GetDescription() },
                {"年龄",a.Age.ToString()},
                {"手机号",a.Phone},
                {"邮箱",a.Email},
                {"微信",a.WeChat},
                {"状态",a.Enabled.IsTrue()?"启用":"注销"},
                {"最后登录时间",a.LastLoginTime.ToDateString()},
                {"最后登录地点",a.LastLoginIP}
            });

            string fileName = $"{Guid.NewGuid()}.xlsx";
            string rootPath = _appsettings.FilePath.ExportExcelPath;

            HFile.CreateDirectory(rootPath);
            string filePath = Path.Combine(rootPath, $"{fileName}");

            await HFile.ExportToExcelEPPlus(filePath, exportData);

            return fileName;
        }

        #region private
        /// <summary>
        /// 检测用户
        /// </summary>
        /// <param name="userId"></param>
        private void CheckUser(long userId)
        {
            if (userId == _currentUser.Id) throw new HException("无法操作当前登录用户");
            if (userId == -1) throw new HException("无法操作系统管理员账户");
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<SysUser> GetUserDetail(long userId)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user == null) throw new HException("用户不存在");
            if (user.IsDeleted) throw new HException("用户已删除");
            if (user.RoleLevel <= _currentUser.RoleLevel) throw new HException("无法操作同级及高级角色用户");
            return user;
        }
        #endregion
    }
}
