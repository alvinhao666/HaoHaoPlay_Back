using Hao.Core.Dependency;
using SqlSugar;

namespace Hao.Core
{
    public interface IUnitOfWork: ITransientDependency
    {
        /// <summary>
        /// 获取 sqlsugar client 实例
        /// </summary>
        /// <returns></returns>
        ISqlSugarClient GetDbClient();
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran();
        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();
    }
}
