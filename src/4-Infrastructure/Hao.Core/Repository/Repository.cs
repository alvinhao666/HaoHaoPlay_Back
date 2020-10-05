using SqlSugar;
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
        public ICurrentUser CurrentUser { get; set; }
        
        public ISqlSugarClient Db { get; set; }
        
        public IdWorker IdWorker { get; set; }

        /// <summary>
        /// 根据主值查询单条数据（单表）
        /// </summary>s
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<T> GetAysnc(TKey pkValue)
        {
            var entity = await Db.Queryable<T>().Where($"{nameof(FullAuditedEntity<TKey>.Id)}='{pkValue}'").SingleAsync();
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
        /// 查询所有数据（未删除）（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc()
        {
            return await Db.Queryable<T>().Where(a => a.IsDeleted == false).ToListAsync();
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

            return await q.Where(a => a.IsDeleted == false)
                .OrderByIF(flag, a => a.CreateTime, OrderByType.Desc)
                .OrderByIF(!flag, query.OrderFileds)
                .ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc()
        {
            return await Db.Queryable<T>().OrderBy(a => a.CreateTime, OrderByType.Desc).ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            var q = Db.Queryable<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            return await q.OrderByIF(flag, a => a.CreateTime, OrderByType.Desc)
                .OrderByIF(!flag, query.OrderFileds)
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

            var q = Db.Queryable<T>();
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            return await q.Where(a => a.IsDeleted == false).CountAsync();
        }

        /// <summary>
        /// 根据条件查询所有分页数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<T>> GetPagedListAysnc(Query<T> query)
        {
            H_Check.Argument.NotNull(query, nameof(query));

            RefAsync<int> totalNumber = 10;
            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            var q = Db.Queryable<T>(); //.WithCache(int cacheDurationInSeconds = int.MaxValue) 使用缓存取数据 
            foreach (var item in query.QueryExpressions)
            {
                q.Where(item);
            }

            var items = await q.Where(a => a.IsDeleted == false)
                .OrderByIF(flag, a => a.CreateTime, OrderByType.Desc)
                .OrderByIF(!flag, query.OrderFileds)
                .ToPageListAsync(query.PageIndex, query.PageSize, totalNumber);

            var pageList = new PagedList<T>()
            {
                Items = items,
                TotalCount = totalNumber,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                TotalPagesCount = (totalNumber.Value + query.PageSize - 1) / query.PageSize
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
                if (id != null) id.SetValue(entity, Guid.NewGuid());
            }
            else if (id != null) id.SetValue(entity, IdWorker.NextId());

            entity.CreatorId = CurrentUser.Id;
            entity.CreateTime = DateTime.Now;
            entity.IsDeleted = false;

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

            var isGuid = typeof(TKey) == H_Util.GuidType;
            var type = typeof(T);
            var id = type.GetProperty(nameof(FullAuditedEntity<TKey>.Id));
            var timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                if (isGuid)
                {
                    if (id != null) id.SetValue(item, Guid.NewGuid());
                }
                else if (id != null) id.SetValue(item, IdWorker.NextId());

                item.CreatorId = CurrentUser.Id;
                item.CreateTime = timeNow;
                item.IsDeleted = false;
            });
            return await Db.Insertable(entities).ExecuteCommandAsync();
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
            H_Check.Argument.IsNotEmpty(entities, nameof(entities));

            return await DeleteAysnc(entities.Select(a => a.Id).ToList());
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="pkValue">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(TKey pkValue)
        {
            var dic = new Dictionary<string, object>();
            dic.Add($"{nameof(FullAuditedEntity<TKey>.IsDeleted)}", true);

            if (CurrentUser.Id.HasValue)
            {
                dic.Add($"{nameof(FullAuditedEntity<TKey>.ModifyTime)}", DateTime.Now);
                dic.Add($"{nameof(FullAuditedEntity<TKey>.ModifierId)}", CurrentUser.Id);
            }

            return await Db.Updateable<T>(dic)
                .Where($"{nameof(FullAuditedEntity<TKey>.Id)}='{pkValue}'")
                .Where(a => a.IsDeleted == false)
                .ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步删除数据（批量）
        /// </summary>
        /// <param name="pkValues">实体类</param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAysnc(List<TKey> pkValues)
        {
            H_Check.Argument.IsNotEmpty(pkValues, nameof(pkValues));


            var dic = new Dictionary<string, object>();
            dic.Add($"{nameof(FullAuditedEntity<TKey>.IsDeleted)}", true);

            if (CurrentUser.Id.HasValue)
            {
                dic.Add($"{nameof(FullAuditedEntity<TKey>.ModifyTime)}", DateTime.Now);
                dic.Add($"{nameof(FullAuditedEntity<TKey>.ModifierId)}", CurrentUser.Id);
            }

            return await Db.Updateable<T>(dic)
                .Where(it => pkValues.Contains(it.Id)).Where(a => a.IsDeleted == false).ExecuteCommandAsync();
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

            return await Db.Updateable(entity)
                .Where($"{nameof(FullAuditedEntity<TKey>.Id)}='{entity.Id}'")
                .Where(a => a.IsDeleted == false)
                .ExecuteCommandAsync();
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

            var updateColumns = properties.Select(a => a.Name).ToList();


            if (CurrentUser.Id.HasValue)
            {
                entity.ModifierId = CurrentUser.Id;
                entity.ModifyTime = DateTime.Now;
                updateColumns.Add(nameof(entity.ModifierId));
                updateColumns.Add(nameof(entity.ModifyTime));
            }

            return await Db.Updateable(entity).UpdateColumns(updateColumns.ToArray())
                .Where($"{nameof(FullAuditedEntity<TKey>.Id)}='{entity.Id}'")
                .Where(a => a.IsDeleted == false)
                .ExecuteCommandAsync();
        }

        /// <summary>
        /// 异步更新数据（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(List<T> entities)
        {
            H_Check.Argument.IsNotEmpty(entities, nameof(entities));

            if (CurrentUser.Id.HasValue)
            {
                var timeNow = DateTime.Now;
                entities.ForEach(item =>
                {
                    item.ModifierId = CurrentUser.Id;
                    item.ModifyTime = timeNow;
                });
            }

            return await Db.Updateable(entities)
                .WhereColumns(a => new {a.Id, a.IsDeleted}) //以id和isdeleted为条件更新，如果数据isdeleted发生变化则不更新
                .ExecuteCommandAsync();
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

            var updateColumns = properties.Select(a => a.Name).ToList();
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

            updateColumns.Add(nameof(FullAuditedEntity<TKey>.IsDeleted)); //需要注意 当WhereColumns和UpdateColumns一起用时，需要把wherecolumns中的列加到UpdateColumns中

            return await Db.Updateable(entities).UpdateColumns(updateColumns.ToArray())
                .WhereColumns(a => new {a.Id, a.IsDeleted}) //以id和isdeleted为条件更新，如果数据isdeleted发生变化则不更新
                .ExecuteCommandAsync();
        }
    }
}