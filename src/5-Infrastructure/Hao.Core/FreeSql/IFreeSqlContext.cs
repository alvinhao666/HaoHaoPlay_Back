using FreeSql;
using FreeSql.Internal;
using FreeSql.Internal.ObjectPool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hao.Core
{
    public interface IFreeSqlContext : IFreeSql
    {
        void Commit();
        void RollBack();
    }

    class FreeSqlContext : IFreeSqlContext
    {
        IFreeSql _originalFsql;
        int _transactionCount;
        DbTransaction _transaction;
        Object<DbConnection> _connection;
        public FreeSqlContext(IFreeSql fsql)
        {
            _originalFsql = fsql;
        }

        public IAdo Ado => _originalFsql.Ado;
        public IAop Aop => _originalFsql.Aop;
        public ICodeFirst CodeFirst => _originalFsql.CodeFirst;
        public IDbFirst DbFirst => _originalFsql.DbFirst;
        public GlobalFilter GlobalFilter => _originalFsql.GlobalFilter;

        public ISelect<T1> Select<T1>() where T1 : class =>
          _originalFsql.Select<T1>().WithTransaction(_transaction);
        public ISelect<T1> Select<T1>(object dywhere) where T1 : class => Select<T1>().WhereDynamic(dywhere);

        public IDelete<T1> Delete<T1>() where T1 : class =>
          _originalFsql.Delete<T1>().WithTransaction(_transaction);
        public IDelete<T1> Delete<T1>(object dywhere) where T1 : class => Delete<T1>().WhereDynamic(dywhere);

        public IUpdate<T1> Update<T1>() where T1 : class =>
          _originalFsql.Update<T1>().WithTransaction(_transaction);
        public IUpdate<T1> Update<T1>(object dywhere) where T1 : class => Update<T1>().WhereDynamic(dywhere);

        public IInsert<T1> Insert<T1>() where T1 : class =>
          _originalFsql.Insert<T1>().WithTransaction(_transaction);
        public IInsert<T1> Insert<T1>(T1 source) where T1 : class => Insert<T1>().AppendData(source);
        public IInsert<T1> Insert<T1>(T1[] source) where T1 : class => Insert<T1>().AppendData(source);
        public IInsert<T1> Insert<T1>(List<T1> source) where T1 : class => Insert<T1>().AppendData(source);
        public IInsert<T1> Insert<T1>(IEnumerable<T1> source) where T1 : class => Insert<T1>().AppendData(source);

        public IInsertOrUpdate<T1> InsertOrUpdate<T1>() where T1 : class =>
          _originalFsql.InsertOrUpdate<T1>().WithTransaction(_transaction);

        public void Dispose() => TransactionCommitPriv(true);
        public void Transaction(Action handler) => TransactionPriv(null, handler);
        public void Transaction(IsolationLevel isolationLevel, Action handler) => TransactionPriv(isolationLevel, handler);
        void TransactionPriv(IsolationLevel? isolationLevel, Action handler)
        {
            if (_transaction != null)
            {
                _transactionCount++;
                return; //事务已开启
            }
            try
            {
                if (_connection == null) _connection = _originalFsql.Ado.MasterPool.Get();
                _transaction = isolationLevel == null ? _connection.Value.BeginTransaction() : _connection.Value.BeginTransaction(isolationLevel.Value);
                _transactionCount = 0; //
            }
            catch
            {
                TransactionCommitPriv(false);
                throw;
            }
        }
        public void Commit() => TransactionCommitPriv(true);
        public void RollBack() => TransactionCommitPriv(false);
        void TransactionCommitPriv(bool iscommit)
        {
            if (_transaction == null) return;
            _transactionCount--;
            try
            {
                if (iscommit == false) _transaction.Rollback();
                else if (_transactionCount <= 0) _transaction.Commit();
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
    }
}
