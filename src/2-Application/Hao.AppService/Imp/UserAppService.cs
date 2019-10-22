using AutoMapper;
using DotNetCore.CAP;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Core.Application;
using Hao.Core.Model;
using Hao.Encrypt;
using Hao.EventData;
using Hao.FileHelper;
using Hao.Library;
using Hao.Model;
using Hao.Model.Enum;
using Hao.Repository;
using Hao.Utility;
using Microsoft.AspNetCore.Hosting;
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

        private IMapper _mapper;


        private ISysUserRepository _userRepository;

        private readonly ICapPublisher _publisher;

        private IHostingEnvironment _hostingEnvironment;

        public UserAppService(ISysUserRepository userRepository, IMapper mapper, ICapPublisher publisher, IHostingEnvironment hostingEnvironment)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _publisher = publisher;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<UserVMOut> GetByID(long? id)
        {
            var user = await _userRepository.GetAysnc(id.Value);

            return _mapper.Map<UserVMOut>(user);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<LoginVMOut> Login(UserQuery query)
        {
            var user = new SysUser();
            var users = await _userRepository.GetListAysnc(query.Conditions);
            if (users.Count == 0)
                throw new HException(ErrorInfo.E005005, nameof(ErrorInfo.E005005).GetErrorCode());
            user = users.FirstOrDefault();
            return _mapper.Map<LoginVMOut>(user);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<long> AddUser(UserVMIn vm)
        {
            var user = _mapper.Map<SysUser>(vm);
            user.FirstNameSpell = HUtil.GetInitialSpell(user.UserName.ToCharArray()[0].ToString());
            user.Password = EncryptProvider.HMACSHA256(user.Password, "haohaoplay");
            return await _userRepository.InsertAysnc(user);
        }
        
        
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task AddUsers(List<UserVMIn> vms)
        {
            var users = _mapper.Map<List<SysUser>>(vms);
            foreach (var user in users)
            {
                user.FirstNameSpell = HUtil.GetInitialSpell(user.UserName.ToCharArray()[0].ToString());
                user.Password = EncryptProvider.HMACSHA256(user.Password, "haohaoplay");
            }
            await _userRepository.InsertAysnc(users);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<UserVMOut>> GetUsers(UserQuery query)
        {
            var users = await _userRepository.GetPagedListAysnc(query);
            var result = _mapper.Map<PagedList<UserVMOut>>(users);

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
        public async Task<UserVMOut> GetUser(long id)
        {
            var user = await _userRepository.GetAysnc(id);
            return _mapper.Map<UserVMOut>(user);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserVMOut> GetCurrentUser()
        {
            return new UserVMOut();
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
            var user = await _userRepository.GetAysnc(userId);
            if (user != null)
            {
                user.LastLoginTime = lastLoginTime;
                user.LastLoginIP = ip;
                await _userRepository.UpdateAsync(user);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUser(long userId)
        {
            await _userRepository.DeleteAysnc(userId);
        }

        /// <summary>
        /// 注销/启用，用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateUserEnabled(long userId, bool enabled)
        {
            var user = await _userRepository.GetAysnc(userId);
            if (user != null)
            {
                user.Enabled = enabled;
                await _userRepository.UpdateAsync(user);
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task EditUser(long userId, UserVMIn vm)
        {
            var user = await _userRepository.GetAysnc(userId);
            if (user != null)
            {
                user.UserName = vm.UserName;
                user.Age = vm.Age;
                user.Gender = vm.Gender;
                user.Phone = vm.Phone;
                user.Email = vm.Email;
                user.WeChat = vm.WeChat;
                await _userRepository.UpdateAsync(user);
            }
        }

        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> ExportUsers(UserQuery query)
        {
            var users = await _userRepository.GetListAysnc(query);

            var exportData = users.Select(a => new Dictionary<string, string>{
                {"姓名",a.UserName},
                {"性别", HDescription.GetDescription(typeof(Gender),(int)a.Gender) },
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
            //string rootPath = _hostingEnvironment.ContentRootPath + "/excel/";
            if (!HFile.IsExistDirectory(rootPath))
                HFile.CreateDirectory(rootPath);
            string filePath = Path.Combine(rootPath, $"{fileName}");

            await HFile.ExportToExcel("用户数据", filePath, "", exportData);

            return fileName; 
        }
    }
}
