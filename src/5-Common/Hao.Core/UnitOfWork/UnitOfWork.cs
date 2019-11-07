using System;
using NLog;
using SqlSugar;

namespace Hao.Core
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public  ISqlSugarClient SqlSugarClient { get; set; }


        /// <summary>
        /// 保证每次访问，多个仓储类，都用一个 client 实例
        /// </summary>
        /// <returns></returns>
        public ISqlSugarClient GetDbClient()
        {
            return SqlSugarClient;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTran()
        {
            SqlSugarClient.Ado.BeginTran();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
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

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTran()
        {
            SqlSugarClient.Ado.RollbackTran();
        }
    }
}
