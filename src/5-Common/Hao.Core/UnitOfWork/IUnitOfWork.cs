using Hao.Core.Dependency;
using SqlSugar;

namespace Hao.Core
{
    public interface IUnitOfWork: ITransientDependency
    {
        // 创建 sqlsugar client 实例
        ISqlSugarClient GetDbClient();
        // 开始事务
        void BeginTran();
        // 提交事务
        void CommitTran();
        // 回滚事务
        void RollbackTran();
    }
}
