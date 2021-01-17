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
    public interface IFreeSqlContext : IFreeSql
    {
        void Commit();
        void Rollback();
        void Transaction(Action handler, ICapPublisher capPublisher);
    }

    class FreeSqlContext : IFreeSqlContext
    {
        [FromServiceContext] public ICurrentUser CurrentUser { get; set; }

        IFreeSql _originalFsql;
        int _transactionCount;
        DbTransaction _transaction;
        Object<DbConnection> _connection;

        ICapPublisher _capPublisher;
        public FreeSqlContext(IFreeSql fsql)
        {
            _originalFsql = fsql;
        }

        public IAdo Ado => _originalFsql.Ado;
        public IAop Aop => _originalFsql.Aop;
        public ICodeFirst CodeFirst => _originalFsql.CodeFirst;
        public IDbFirst DbFirst => _originalFsql.DbFirst;
        public GlobalFilter GlobalFilter => _originalFsql.GlobalFilter;

        public ISelect<T1> Select<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFsql.Select<T1>().WithTransaction(_transaction);
        }

        public ISelect<T1> Select<T1>(object dywhere) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Select<T1>().WhereDynamic(dywhere);
        }

        public IDelete<T1> Delete<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFsql.Delete<T1>().WithTransaction(_transaction);
        }

        public IDelete<T1> Delete<T1>(object dywhere) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Delete<T1>().WhereDynamic(dywhere);
        }

        public IUpdate<T1> Update<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFsql.Update<T1>().WithTransaction(_transaction);
        }

        public IUpdate<T1> Update<T1>(object dywhere) where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return Update<T1>().WhereDynamic(dywhere);
        }

        public IInsert<T1> Insert<T1>() where T1 : class
        {
            InitAsyncLocalCurrentUser();
            return _originalFsql.Insert<T1>().WithTransaction(_transaction);
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
            // InitAsyncLocalCurrentUser();
            // return _originalFsql.InsertOrUpdate<T1>().WithTransaction(_transaction);
            throw new H_Exception("InsertOrUpdate方法暂且不可使用");
        }
          

        public void Dispose() => TransactionCommitPriv(true);
        
        public void Transaction(Action handler,ICapPublisher capPublisher) => TransactionPriv(null, handler, capPublisher);
        public void Transaction(Action handler) => TransactionPriv(null, handler);
        public void Transaction(IsolationLevel isolationLevel, Action handler) => TransactionPriv(isolationLevel, handler);
        void TransactionPriv(IsolationLevel? isolationLevel, Action handler, ICapPublisher capPublisher = null)
        {
            if (_transaction != null)
            {
                _transactionCount++;
                return; //事务已开启
            }
            try
            {
                if (_connection == null) _connection = _originalFsql.Ado.MasterPool.Get();
                
                if (capPublisher == null)
                {
                    _transaction = isolationLevel == null ? _connection.Value.BeginTransaction() : _connection.Value.BeginTransaction(isolationLevel.Value);
                }
                else
                {
                    _transaction = (DbTransaction) _connection.Value.BeginTransaction(capPublisher).DbTransaction;
                    _capPublisher = capPublisher;
                }
                
                _transactionCount = 0;
            }
            catch
            {
                TransactionCommitPriv(false);
                throw;
            }
        }
        public void Commit() => TransactionCommitPriv(true);
        public void Rollback() => TransactionCommitPriv(false);
        void TransactionCommitPriv(bool iscommit)
        {
            if (_transaction == null) return;
            _transactionCount--;
            try
            {
                if (iscommit == false) _transaction.Rollback();
                else if (_transactionCount <= 0)
                {
                    _transaction.Commit();
                    if (_capPublisher != null) _capPublisher.Transaction.Value.Flush();
                }
            }
            finally
            {
                if (iscommit == false || _transactionCount <= 0)
                {
                    _originalFsql.Ado.MasterPool.Return(_connection);
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


    public static class CapUnitOfWorkExtensions
    {
        public static void Flush(this ICapTransaction capTransaction)
        {
            capTransaction?.GetType().GetMethod("Flush", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(capTransaction, null);
        }
    }
}
