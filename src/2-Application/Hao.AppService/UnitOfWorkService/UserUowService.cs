using Hao.Core;
using Hao.Model;
using Hao.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    internal class UserUowService : UnitOfWorkService, IUserUowService
    {

        private readonly ISysUserRepository _userRep;

        private readonly ISysLoginRecordRepository _recordRep;

        public UserUowService( ISysUserRepository userRepository, ISysLoginRecordRepository recordRep)
        {
            _userRep = userRepository;
            _recordRep = recordRep;
        }


        /// <summary>
        /// 更新登录信息  
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [UseTransaction]
        public void UpdateLogin(SysUser user)
        {
            _userRep.Update(user, user => new { user.LastLoginTime, user.LastLoginIP });
            _recordRep.Insert(new SysLoginRecord() { UserId = user.Id, IP = user.LastLoginIP, Time = user.LastLoginTime });
        }
    }
}
