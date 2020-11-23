using Hao.Core;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public class SysLoginRecordRepository : BaseRepository<SysLoginRecord, long>, ISysLoginRecordRepository
    {
        /// <summary>
        /// 获取登录记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public async Task<List<SysLoginRecord>> GetLoginRecords(long? userId, DateTime? expireTime)
        {

            var result = await DbContext.Select<SysLoginRecord>()
                                 .WhereIf(userId.HasValue, a => a.UserId == userId)
                                 .WhereIf(expireTime.HasValue, a => a.JwtExpireTime >= expireTime)
                                 .ToListAsync();
            return result;
        }
    }
}
