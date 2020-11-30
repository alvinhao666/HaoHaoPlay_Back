using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Hao.Snowflake;
using Hao.Utility;
using System.Linq;
using AspectCore.DependencyInjection;

namespace Hao.Core
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey>
        where T : FullAuditedEntity<TKey>, new() where TKey : struct
    {
        [FromServiceContext]
        public IFreeSqlContext DbContext { get; set; }
        
        [FromServiceContext]
        public ICurrentUser CurrentUser { get; set; }

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

            return await DbContext.Select<T>().Where(x => pkValues.Contains(x.Id)).ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（未删除）（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc()
        {
            return await DbContext.Select<T>().Where(a => a.IsDeleted == false).ToListAsync();
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

            return await q.Where(a => a.IsDeleted == false)
                .OrderByDescending(flag, a => a.CreateTime)
                .OrderBy(!flag, query.OrderByFileds)
                .ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc()
        {
            return await DbContext.Select<T>().OrderByDescending(a => a.CreateTime).ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            var flag = string.IsNullOrWhiteSpace(query.OrderByFileds);
            var q = DbContext.Select<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            return await q.OrderByDescending(flag, a => a.CreateTime)
                .OrderBy(!flag, query.OrderByFileds)
                .ToListAsync();
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

            return (int)await q.Where(a => a.IsDeleted == false).CountAsync();
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

            var items = await q.Where(a => a.IsDeleted == false)
                .OrderByDescending(flag, a => a.CreateTime)
                .OrderBy(!flag, query.OrderByFileds)
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
            var isGuid = typeof(TKey) == H_Util.GuidType;
            var id = type.GetProperty(nameof(FullAuditedEntity<TKey>.Id));

            if (isGuid)
            {
                id.SetValue(entity, Guid.NewGuid());
            }
            else
            {
                id.SetValue(entity, IdWorker.NextId());
            }

            entity.CreatorId = CurrentUser.Id;
            entity.CreateTime = DateTime.Now;

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

            var isGuid = typeof(TKey) == H_Util.GuidType;
            var type = typeof(T);
            var id = type.GetProperty(nameof(FullAuditedEntity<TKey>.Id));
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

                item.CreatorId = CurrentUser.Id;
                item.CreateTime = timeNow;
            });
            return await DbContext.Insert(entities).ExecuteAffrowsAsync();
        }


        /// <summary>
        /// 异步删除数据（逻辑删除）
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(T entity)
        {
            H_Check.Argument.NotNull(entity, nameof(entity));

            return await DeleteAysnc(entity.Id);
        }

        /// <summary>
        /// 异步删除数据（批量）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(List<T> entities)
        {
            H_Check.Argument.NotEmpty(entities, nameof(entities));

            return await DeleteAysnc(entities.Select(a => a.Id).ToList());
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="pkValue">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(TKey pkValue)
        {

            return await DbContext.Update<T>()
                .Set(a => a.IsDeleted, true)
                .SetIf(CurrentUser.Id.HasValue, a => a.ModifierId, CurrentUser.Id)
                .SetIf(CurrentUser.Id.HasValue, a => a.ModifyTime, DateTime.Now)
                .Where(a => a.Id.Equals(pkValue))
                .Where(a => a.IsDeleted == false)
                .ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步删除数据（批量）
        /// </summary>
        /// <param name="pkValues">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(List<TKey> pkValues)
        {
            H_Check.Argument.NotEmpty(pkValues, nameof(pkValues));

            return await DbContext.Update<T>()
                .Set(a => a.IsDeleted, true)
                .SetIf(CurrentUser.Id.HasValue, a => a.ModifierId, CurrentUser.Id)
                .SetIf(CurrentUser.Id.HasValue, a => a.ModifyTime, DateTime.Now)
                .Where(it => pkValues.Contains(it.Id)).Where(a => a.IsDeleted == false).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(T entity)
        {
            H_Check.Argument.NotNull(entity, nameof(entity));

            if (CurrentUser.Id.HasValue)
            {
                entity.ModifierId = CurrentUser.Id;
                entity.ModifyTime = DateTime.Now;
            }

            return await DbContext.Update<T>().SetSource(entity)
                .Where(a => a.IsDeleted == false)
                .ExecuteAffrowsAsync();
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

            var body = columns.Body as NewExpression;
            H_Check.Argument.NotNull(body, nameof(columns));

            var updateColumns = body.Members.Select(a => a.Name).ToList();

            if (CurrentUser.Id.HasValue)
            {
                entity.ModifierId = CurrentUser.Id;
                entity.ModifyTime = DateTime.Now;
                updateColumns.Add(nameof(entity.ModifierId));
                updateColumns.Add(nameof(entity.ModifyTime));
            }

            return await DbContext.Update<T>().SetSource(entity).UpdateColumns(updateColumns.ToArray())
                .Where(a => a.IsDeleted == false)
                .ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 异步更新数据（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(List<T> entities)
        {
            H_Check.Argument.NotEmpty(entities, nameof(entities));

            if (CurrentUser.Id.HasValue)
            {
                var timeNow = DateTime.Now;
                entities.ForEach(item =>
                {
                    item.ModifierId = CurrentUser.Id;
                    item.ModifyTime = timeNow;
                });
            }

            return await DbContext.Update<T>().SetSource(entities)
                .Where(a => a.IsDeleted == false)
                .ExecuteAffrowsAsync();
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

            var body = columns.Body as NewExpression;
            H_Check.Argument.NotNull(body, nameof(columns));

            var updateColumns = body.Members.Select(a => a.Name).ToList();

            if (CurrentUser.Id.HasValue)
            {
                var timeNow = DateTime.Now;
                entities.ForEach(item =>
                {
                    item.ModifierId = CurrentUser.Id;
                    item.ModifyTime = timeNow;
                });
                updateColumns.Add(nameof(FullAuditedEntity<TKey>.ModifierId));
                updateColumns.Add(nameof(FullAuditedEntity<TKey>.ModifyTime));
            }


            return await DbContext.Update<T>().SetSource(entities).UpdateColumns(updateColumns.ToArray())
                .Where(a => a.IsDeleted == false)
                .ExecuteAffrowsAsync();
        }
    }
}