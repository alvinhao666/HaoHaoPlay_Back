using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Entity;
using Hao.Model;
using Hao.Snowflake;
using SqlSugar;

namespace Hao.Repository
{
    public class SysLoginRecordRepository : ISysLoginRecordRepository
    {

        private readonly IUnitOfWork _unitOfWork;

        public IdWorker IdWorker { get; set; }

        public SysLoginRecordRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddLoginRecord(SysLoginRecord record)
        {
            record.Id = IdWorker.NextId();
            await _unitOfWork.GetDbClient().Insertable(record).ExecuteReturnEntityAsync();
        }
    }
}
