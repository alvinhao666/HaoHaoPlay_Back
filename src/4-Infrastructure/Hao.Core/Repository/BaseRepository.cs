using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Hao.Snowflake;
using System.Linq;
using AspectCore.DependencyInjection;
using Hao.Utility;

namespace Hao.Core
{
    public abstract class BaseRepository<T, TKey> : IBaseRepository<T, TKey>
        where T : BaseEntity<TKey>, new() where TKey : struct
    {
        public ISqlSugarClient Db { get; set; }

        public IdWorker IdWorker { get; set; }

        /// <summary>
        /// 根据主值查询单条数据（单表）
        /// </summary>s
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<T> GetAysnc(TKey pkValue)
        {
            var entity = await Db.Queryable<T>().Where($"{nameof(BaseEntity<TKey>.Id)}='{pkValue}'").SingleAsync();
            return entity;
        }

        /// <summary>
        /// 根据主值查询多条数据（单表）
        /// </summary>s
        /// <param name="pkValues">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<List<T>> GetListAysnc(List<TKey> pkValues)
        {
            H_Check.Argument.NotNull(pkValues, nameof(pkValues));

            if (pkValues.Count == 0) return new List<T>();

            //Type type = typeof(T); 类型判断，主要包括 is 和 typeof 两个操作符及对象实例上的 GetType 调用。这是最轻型的消耗，可以无需考虑优化问题。注意 typeof 运算符比对象实例上的 GetType 方法要快，只要可能则优先使用 typeof 运算符。 
            return await Db.Queryable<T>().In(pkValues).ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc()
        {
            return await Db.Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            var q = Db.Queryable<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            return await q.OrderByIF(!flag, query.OrderFileds).ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据数量（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<int> GetCountAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            var q = Db.Queryable<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            return await q.CountAsync();
        }

        /// <summary>
        /// 根据条件查询所有分页数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<T>> GetPagedListAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            RefAsync<int> totalNumber = 0;
            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            var q = Db.Queryable<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            var items = await q.OrderByIF(!flag, query.OrderFileds)
                .ToPageListAsync(query.PageIndex, query.PageSize, totalNumber);

            var pageList = new PagedList<T>()
            {
                Items = items,
                TotalCount = totalNumber,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalPagesCount = (totalNumber + query.PageSize - 1) / query.PageSize
            };
            return pageList;
        }

        /// <summary>
        /// 异步写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public virtual async Task<T> InsertAysnc(T entity)
        {
            H_Check.Argument.NotNull(entity, nameof(entity));

            var type = typeof(T);
            var isGuid = typeof(TKey) == typeof(Guid);
            var id = type.GetProperty(nameof(BaseEntity<TKey>.Id));

            if (isGuid)
            {
                if (id != null) id.SetValue(entity, Guid.NewGuid());
            }
            else if (id != null) id.SetValue(entity, IdWorker.NextId());

            var obj = await Db.Insertable(entity).ExecuteReturnEntityAsync();
            return obj;
        }

        /// <summary>
        /// 异步写入实体数据（批量）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> InsertAysnc(List<T> entities)
        {
            H_Check.Argument.IsNotEmpty(entities, nameof(entities));

            var isGuid = typeof(TKey) == typeof(Guid);
            var type = typeof(T);
            var id = type.GetProperty(nameof(BaseEntity<TKey>.Id));
            var timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                if (isGuid)
                {
                    if (id != null) id.SetValue(item, Guid.NewGuid());
                }
                else if (id != null) id.SetValue(item, IdWorker.NextId());
            });
            return await Db.Insertable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T entity)
        {
            H_Check.Argument.NotNull(entity, nameof(entity));

            return await Db.Updateable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步更新数据（指定列名）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T entity, Expression<Func<T, object>> columns)
        {
            H_Check.Argument.NotNull(entity, nameof(entity));

            H_Check.Argument.NotNull(columns, nameof(columns));

            var properties = columns.Body.Type.GetProperties();
            H_Check.Argument.IsNotEmpty(properties, nameof(columns));

            var updateColumns = properties.Select(a => a.Name);

            return await Db.Updateable(entity).UpdateColumns(updateColumns.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步更新数据（批量）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(List<T> entities)
        {
            H_Check.Argument.IsNotEmpty(entities, nameof(entities));

            return await Db.Updateable(entities).ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步更新数据（批量）（指定列名）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(List<T> entities, Expression<Func<T, object>> columns)
        {
            H_Check.Argument.IsNotEmpty(entities, nameof(entities));

            H_Check.Argument.NotNull(columns, nameof(columns));

            var properties = columns.Body.Type.GetProperties();
            H_Check.Argument.IsNotEmpty(properties, nameof(columns));

            var updateColumns = properties.Select(a => a.Name);

            return await Db.Updateable(entities).UpdateColumns(updateColumns.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(TKey pkValue)
        {
            return await Db.Deleteable<T>().Where($"{nameof(BaseEntity<TKey>.Id)}='{pkValue}'").ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(List<TKey> pkValues)
        {
            H_Check.Argument.IsNotEmpty(pkValues, nameof(pkValues));

            return await Db.Deleteable<T>().In(pkValues).ExecuteCommandAsync();
        }
    }
}