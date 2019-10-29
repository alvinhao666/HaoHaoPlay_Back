using AutoMapper;
using DotNetCore.CAP;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Core.Application;
using Hao.Core.Model;
using Hao.Encrypt;
using Hao.Enum;
using Hao.EventData;
using Hao.File;
using Hao.Library;
using Hao.Model;
using Hao.Repository;
using Hao.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class UserAppService : ApplicationService, IUserAppService
    {

        private readonly IMapper _mapper;


        private readonly ISysUserRepository _userRep;

        private readonly ICapPublisher _publisher;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IConfiguration _config;

        public UserAppService(IConfiguration config, ISysUserRepository userRepository, IMapper mapper, ICapPublisher publisher, IHostingEnvironment hostingEnvironment)
        {
            _userRep = userRepository;
            _mapper = mapper;
            _publisher = publisher;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }

        public async Task<UserOut> GetByID(long? id)
        {
            var user = await _userRep.GetAysnc(id.Value);

            return _mapper.Map<UserOut>(user);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //[UseTransaction]
        public async Task<LoginOut> Login(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query.Conditions);
            if (users.Count == 0)
                throw new HException(ErrorInfo.E005005, nameof(ErrorInfo.E005005).GetErrorCode());
            var user = users.FirstOrDefault();
            return _mapper.Map<LoginOut>(user);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<long> AddUser(UserIn vm)
        {
            var user = _mapper.Map<SysUser>(vm);
            user.FirstNameSpell = HUtil.GetInitialSpell(user.UserName.ToCharArray()[0].ToString());
            user.Password = EncryptProvider.HMACSHA256(user.Password, _config["KeyInfo:Sha256Key"]);
            return await _userRep.InsertAysnc(user);
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task AddUsers(List<UserIn> vms)
        {
            var users = _mapper.Map<List<SysUser>>(vms);
            foreach (var user in users)
            {
                user.FirstNameSpell = HUtil.GetInitialSpell(user.UserName.ToCharArray()[0].ToString());
                user.Password = EncryptProvider.HMACSHA256(user.Password, _config["KeyInfo:Sha256Key"]);
            }
            await _userRep.InsertAysnc(users);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<UserOut>> GetUsers(UserQuery query)
        {
            var users = await _userRep.GetPagedListAysnc(query);
            var result = _mapper.Map<PagedList<UserOut>>(users);

            ////指定发送的消息标题（供订阅）和内容
            //await _publisher.PublishAsync("xxx.services.account.check",
            //    new PersonEventData { Name = "Foo", Age = 11 }).ConfigureAwait(false);
            // 你的业务代码。

            return result;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserOut> GetUser(long id)
        {
            var user = await _userRep.GetAysnc(id);
            return _mapper.Map<UserOut>(user);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserOut> GetCurrentUser()
        {
            return new UserOut();
        }

        /// <summary>
        /// 更新用户最后登录时间Ip
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastLoginTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public async Task UpdateLoginTimeAndIP(long userId, DateTime lastLoginTime, string ip)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user != null)
            {
                user.LastLoginTime = lastLoginTime;
                user.LastLoginIP = ip;
                await _userRep.UpdateAsync(user);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUser(long userId)
        {
            await _userRep.DeleteAysnc(userId);
        }

        /// <summary>
        /// 注销/启用，用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateUserEnabled(long userId, bool enabled)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user != null)
            {
                user.Enabled = enabled;
                await _userRep.UpdateAsync(user);
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task EditUser(long userId, UserIn vm)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user != null)
            {
                user.UserName = vm.UserName;
                user.Age = vm.Age;
                user.Gender = vm.Gender;
                user.Phone = vm.Phone;
                user.Email = vm.Email;
                user.WeChat = vm.WeChat;
                await _userRep.UpdateAsync(user);
            }
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
                {"姓名",a.UserName},
                {"性别", HDescription.GetDescription(a.Gender) },
                {"年龄",a.Age.ToString()},
                {"手机号",a.Phone},
                {"邮箱",a.Email},
                {"微信",a.WeChat},
                {"状态",a.Enabled.HasValue&&a.Enabled.Value?"启用":"注销"},
                {"最后登录时间",a.LastLoginTime.HasValue?a.LastLoginTime.Value.ToDateString():""},
                {"最后登录地点",a.LastLoginIP}
            }).ToList();

            string fileName = $"{Guid.NewGuid()}.xlsx";
            string rootPath = new DirectoryInfo(_hostingEnvironment.ContentRootPath).Parent.FullName + "/ExportFile/Excel/";

            if (!HFile.IsExistDirectory(rootPath))
                HFile.CreateDirectory(rootPath);
            string filePath = Path.Combine(rootPath, $"{fileName}");

            await HFile.ExportToExcel(filePath, exportData);

            return fileName;
        }
    }
}
