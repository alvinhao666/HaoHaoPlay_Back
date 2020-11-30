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
        [FromServiceContext]
        public IFreeSqlContext DbContext { get; set; }

        [FromServiceContext]
        public IdWorker IdWorker { get; set; }

        /// <summary>
        /// 根据主值查询单条数据（单表）
        /// </summary>s
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<T> GetAysnc(TKey pkValue)
        {
            var entity = await DbContext.Select<T>().Where(a => a.Id.Equals(pkValue)).ToOneAsync();
            return entity;
        }

        /// <summary>
        /// 根据主值查询多条数据（单表）
        /// </summary>s
        /// <param name="pkValues">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<List<T>> GetListAysnc(List<TKey> pkValues)
        {
            H_Check.Argument.NotEmpty(pkValues, nameof(pkValues));

            return await DbContext.Select<T>().Where(a => pkValues.Contains(a.Id)).ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc()
        {
            return await DbContext.Select<T>().ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            var flag = string.IsNullOrWhiteSpace(query.OrderByFileds);
            var q = DbContext.Select<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            return await q.OrderBy(!flag, query.OrderByFileds).ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据数量（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<int> GetCountAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            var q = DbContext.Select<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            return (int)await q.CountAsync();
        }

        /// <summary>
        /// 根据条件查询所有分页数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<T>> GetPagedListAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));


            var flag = string.IsNullOrWhiteSpace(query.OrderByFileds);
            var q = DbContext.Select<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            var items = await q.OrderBy(!flag, query.OrderByFileds)
                .Count(out var total) 
                .Page(query.PageIndex, query.PageSize).ToListAsync();

            var pageList = new PagedList<T>()
            {
                Items = items,
                TotalCount = (int)total,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalPagesCount = ((int)total + query.PageSize - 1) / query.PageSize
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
                id.SetValue(entity, Guid.NewGuid());
            }
            else
            {
                id.SetValue(entity, IdWorker.NextId());
            }

            var obj = await DbContext.Insert(entity).ExecuteInsertedAsync();
            return obj.First();
        }

        /// <summary>
        /// 异步写入实体数据（批量）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> InsertAysnc(List<T> entities)
        {
            H_Check.Argument.NotEmpty(entities, nameof(entities));

            var isGuid = typeof(TKey) == typeof(Guid);
            var type = typeof(T);
            var id = type.GetProperty(nameof(BaseEntity<TKey>.Id));
            var timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                if (isGuid)
                {
                    id.SetValue(item, Guid.NewGuid());
                }
                else
                {
                    id.SetValue(item, IdWorker.NextId());
                }
            });
            return await DbContext.Insert(entities).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T entity)
        {
            H_Check.Argument.NotNull(entity, nameof(entity));

            return await DbContext.Update<T>().SetSource(entity).ExecuteAffrowsAsync();
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
            H_Check.Argument.NotEmpty(properties, nameof(columns));

            var updateColumns = properties.Select(a => a.Name);


            return await DbContext.Update<T>().SetSource(entity).UpdateColumns(updateColumns.ToArray()).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步更新数据（批量）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(List<T> entities)
        {
            H_Check.Argument.NotEmpty(entities, nameof(entities));

            return await DbContext.Update<T>().SetSource(entities).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步更新数据（批量）（指定列名）
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(List<T> entities, Expression<Func<T, object>> columns)
        {
            H_Check.Argument.NotEmpty(entities, nameof(entities));

            H_Check.Argument.NotNull(columns, nameof(columns));

            var properties = columns.Body.Type.GetProperties();
            H_Check.Argument.NotEmpty(properties, nameof(columns));

            var updateColumns = properties.Select(a => a.Name);

            return await DbContext.Update<T>().SetSource(entities).UpdateColumns(updateColumns.ToArray()).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(TKey pkValue)
        {
            return await DbContext.Delete<T>().Where(a => a.Id.Equals(pkValue)).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(List<TKey> pkValues)
        {
            H_Check.Argument.NotEmpty(pkValues, nameof(pkValues));

            return await DbContext.Delete<T>().Where(a => pkValues.Contains(a.Id)).ExecuteAffrowsAsync();
        }
    }
}