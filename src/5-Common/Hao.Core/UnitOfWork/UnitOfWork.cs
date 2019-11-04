using System;
using NLog;
using SqlSugar;

namespace Hao.Core
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public  ISqlSugarClient SqlSugarClient { get; set; }


        // 保证每次访问，多个仓储类，都用一个 client 实例
        public ISqlSugarClient GetDbClient()
        {
            return SqlSugarClient;
        }

        public void BeginTran()
        {
            SqlSugarClient.Ado.BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                SqlSugarClient.Ado.CommitTran(); 
            }
            catch (Exception ex)
            {
                SqlSugarClient.Ado.RollbackTran();
                _logger.Error(ex, ex.Message);
            }
        }


        public void RollbackTran()
        {
            SqlSugarClient.Ado.RollbackTran();
        }
    }
}
