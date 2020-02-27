using System;
using SqlSugar;

namespace Hao.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISqlSugarClient SqlSugarClient { get; set; }


        /// <summary>
        /// 保证每次访问，多个仓储类，都用一个 client 实例
        /// </summary>
        /// <returns></returns>
        public ISqlSugarClient GetDbClient()
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(SqlSugarClient.ContextID);
            Console.ForegroundColor = ConsoleColor.White;
#endif
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
            catch(Exception ex)
            {
                SqlSugarClient.Ado.RollbackTran();
                throw ex;
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