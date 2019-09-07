using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core.Application;
using Hao.Core.Model;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IUserAppService : IApplicationService
    {
        Task<LoginVMOut> Login(UserQuery query);

        Task<long> AddUser(UserVMIn vm);
        
        Task AddUsers(List<UserVMIn> vms);
        
        Task EditUser(long userID, UserVMIn vm);

        Task<PagedList<UserVMOut>> GetUsers(UserQuery query);

        Task<UserVMOut> GetUser(long id);

        Task<UserVMOut> GetCurrentUser();

        Task UpdateLoginTimeAndIP(long userId, DateTime lastLoginTime, string ip);

        Task DeleteUser(long userId);

        Task UpdateUserEnabled(long userId,bool enabled);

        Task<string> ExportUsers(UserQuery query);
    }
}
