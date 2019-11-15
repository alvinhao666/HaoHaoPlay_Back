using Hao.Core;
using Hao.Dependency;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public interface ISysLoginRecordRepository: ITransientDependency
    {
        /// <summary>
        /// 添加登录记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task AddLoginRecord(SysLoginRecord record);
    }
}
