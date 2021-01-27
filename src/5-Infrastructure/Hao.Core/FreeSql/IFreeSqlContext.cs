using FreeSql;
using FreeSql.Internal;
using FreeSql.Internal.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using AspectCore.DependencyInjection;
using DotNetCore.CAP;
using Hao.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Hao.Core
{
    /// <summary>
    /// FreeSql上下文
    /// </summary>
    public interface IFreeSqlContext : IFreeSql
    {
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();

        /// <summary>
        /// 发起事务
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="capPublisher"></param>
        void Transaction(Action handler, ICapPublisher capPublisher);
    }

    /// <summary>
    /// FreeSql上下文实现类，默认访问修饰符internal
    /// </summary>
    internal class FreeSqlContext : IFreeSqlContext
    {
        [FromServiceContext] public ICurrentUser CurrentUser { get; set; }

        private ICapPublisher _capPublisher;

        private readonly IFreeSql _originalFreeSql;

        private int _transactionCount;

        private DbTransaction _transaction;

        private Object<DbConnection> _connection;

        public FreeSqlContext(IFreeSql freeSql)
        {
            _originalFreeSql = freeSql;
        }

        public IAdo Ado => _originalFreeSql.Ado;

        public IAop Aop => _originalFreeSql.Aop;

        public ICodeFirst CodeFirst => _originalFreeSql.CodeFirst;

        public IDbFirst DbFirst => _originalFreeSql.DbFirst;

        public GlobalFilter GlobalFilter => _originalFreeSql.GlobalFilter;

        public ISelect<T1> Select<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFreeSql.Select<T1>().WithTransaction(_transaction);
        }

        public ISelect<T1> Select<T1>(object dyWhere) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Select<T1>().WhereDynamic(dyWhere);
        }

        public IDelete<T1> Delete<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFreeSql.Delete<T1>().WithTransaction(_transaction);
        }

        public IDelete<T1> Delete<T1>(object dyWhere) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Delete<T1>().WhereDynamic(dyWhere);
        }

        public IUpdate<T1> Update<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFreeSql.Update<T1>().WithTransaction(_transaction);
        }

        public IUpdate<T1> Update<T1>(object dyWhere) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Update<T1>().WhereDynamic(dyWhere);
        }

        public IInsert<T1> Insert<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFreeSql.Insert<T1>().WithTransaction(_transaction);
        }

        public IInsert<T1> Insert<T1>(T1 source) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Insert<T1>().AppendData(source);
        }

        public IInsert<T1> Insert<T1>(T1[] source) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Insert<T1>().AppendData(source);
        }

        public IInsert<T1> Insert<T1>(List<T1> source) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Insert<T1>().AppendData(source);
        }

        public IInsert<T1> Insert<T1>(IEnumerable<T1> source) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Insert<T1>().AppendData(source);
        }

        [Obsolete]
        public IInsertOrUpdate<T1> InsertOrUpdate<T1>() where T1 : class
        {
            //InitAsyncLocalCurrentUser();
            //return _originalFreeSql.InsertOrUpdate<T1>().WithTransaction(_transaction);
            throw new H_Exception("InsertOrUpdate方法暂且不可使用");
        }

        public void Dispose() => HandleTransaction(true);

        public void Transaction(Action handler, ICapPublisher capPublisher) => HandleTransaction(null, handler, capPublisher);

        public void Transaction(Action handler) => HandleTransaction(null, handler);

        public void Transaction(IsolationLevel isolationLevel, Action handler) => HandleTransaction(isolationLevel, handler);

        private void HandleTransaction(IsolationLevel? isolationLevel, Action handler, ICapPublisher capPublisher = null)
        {
            if (_transaction != null)
            {
                _transactionCount++;
                return; //事务已开启
            }
            try
            {
                if (_connection == null) _connection = _originalFreeSql.Ado.MasterPool.Get();

                if (capPublisher == null)
                {
                    _transaction = isolationLevel == null ? _connection.Value.BeginTransaction() : _connection.Value.BeginTransaction(isolationLevel.Value);
                }
                else
                {
                    _transaction = (DbTransaction)_connection.Value.BeginTransaction(capPublisher).DbTransaction;
                    _capPublisher = capPublisher;
                }

                _transactionCount = 0;
            }
            catch
            {
                HandleTransaction(false);
                throw;
            }
        }
        public void Commit() => HandleTransaction(true);

        public void Rollback() => HandleTransaction(false);

        private void HandleTransaction(bool isCommit)
        {
            if (_transaction == null) return;
            _transactionCount--;
            try
            {
                if (isCommit == false) _transaction.Rollback();
                else if (_transactionCount <= 0)
                {
                    _transaction.Commit();
                    if (_capPublisher != null) _capPublisher.Transaction.Value.Flush();
                }
            }
            finally
            {
                if (isCommit == false || _transactionCount <= 0)
                {
                    _originalFreeSql.Ado.MasterPool.Return(_connection);
                    _connection = null;
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// 设置当前用户，不能为异步方法
        /// </summary>
        private void InitAsyncLocalCurrentUser()
        {
            FreeSqlCollectionExtensions.CurrentUser.Value = CurrentUser;
        }
    }

    internal static class CapUnitOfWorkExtensions
    {
        public static void Flush(this ICapTransaction capTransaction)
        {
            capTransaction?.GetType().GetMethod("Flush", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(capTransaction, null);
        }
    }
}
