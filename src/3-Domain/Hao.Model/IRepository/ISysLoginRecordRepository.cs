using Hao.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Model
{
    public interface ISysLoginRecordRepository: IRepository<SysLoginRecord,long>
    {
        /// <summary>
        /// 获取登录记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        Task<List<SysLoginRecord>> GetLoginRecords(long? userId, DateTime? expireTime);
    }
}
