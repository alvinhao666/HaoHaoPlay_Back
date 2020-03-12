using Hao.Dependency;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Hao.Entity;
using Hao.Snowflake;
using Hao.Utility;
using System.Linq;

namespace Hao.Core
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey>  where T : FullAuditedEntity<TKey>, new() where TKey : struct
    {
        public ICurrentUser CurrentUser { get; set; }

        public IUnitOfWork UnitOfWork { get; set; }
        
        public IdWorker IdWorker { get; set; }

        /// <summary>
        /// 根据主值查询单条数据（单表）
        /// </summary>s
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<T> GetAysnc(TKey pkValue)
        {
            var entity = await UnitOfWork.GetDbClient().Queryable<T>().InSingleAsync(pkValue);
            return entity;
        }

        /// <summary>
        /// 根据主值查询多条数据（单表）
        /// </summary>s
        /// <param name="pkValues">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<List<T>> GetListAysnc(List<TKey> pkValues)
        {
            //Type type = typeof(T); 类型判断，主要包括 is 和 typeof 两个操作符及对象实例上的 GetType 调用。这是最轻型的消耗，可以无需考虑优化问题。注意 typeof 运算符比对象实例上的 GetType 方法要快，只要可能则优先使用 typeof 运算符。 
            return await UnitOfWork.GetDbClient().Queryable<T>().In(pkValues).ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（未删除）（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc()
        {
            return await UnitOfWork.GetDbClient().Queryable<T>().Where(a => a.IsDeleted == false).ToListAsync();
        }

        /// <summary>
        /// 查询所有数据（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc()
        {
            return await UnitOfWork.GetDbClient().Queryable<T>().ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc(Query<T> query)
        {
            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            var q = UnitOfWork.GetDbClient().Queryable<T>();
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
        /// 根据条件查询所有分页数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<T>> GetPagedListAysnc(Query<T> query)
        {
            RefAsync<int> totalNumber = 10;
            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            var q = UnitOfWork.GetDbClient().Queryable<T>();
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
        public virtual async Task<TKey> InsertAysnc(T entity)
        {
            var type = typeof(T);
            var isGuid = typeof(TKey) == HUtil.GuidType;
            var id = type.GetProperty("Id");

            if (isGuid)
            {
                if (id != null) id.SetValue(entity, Guid.NewGuid());
            }
            else if (id != null) id.SetValue(entity, IdWorker.NextId());

            entity.CreatorId = CurrentUser.Id;
            entity.CreateTime = DateTime.Now;
            entity.IsDeleted = false;

            var obj = await UnitOfWork.GetDbClient().Insertable(entity).ExecuteReturnEntityAsync();
            return obj.Id;
        }

        /// <summary>
        /// 异步写入实体数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAysnc(List<T> entities)
        {
            var isGuid = typeof(TKey) == HUtil.GuidType;
            var type = typeof(T);
            var id = type.GetProperty("Id");
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
            return await UnitOfWork.GetDbClient().Insertable(entities).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步删除数据（逻辑删除）
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(T entity)
        {
            entity.ModifierId = CurrentUser.Id;
            entity.ModifyTime = DateTime.Now;
            entity.IsDeleted = true;
            var columns = new string[] { "LastModifyUserId", "LastModifyTime", "IsDeleted" };

            return await UnitOfWork.GetDbClient().Updateable(entity).UpdateColumns(columns.ToArray()).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValue">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(TKey pkValue)
        {
            return await UnitOfWork.GetDbClient().Updateable<T>(new { LastModifyTime = DateTime.Now, LastModifyUserId = CurrentUser.Id, IsDeleted = true })
                        .Where($"Id='{pkValue}'").ExecuteCommandAsync() > 0;

        }

        /// <summary>
        /// 异步删除数据(多条)
        /// </summary>
        /// <param name="pkValues">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(List<TKey> pkValues)
        {
            return await UnitOfWork.GetDbClient().Updateable<T>(new { LastModifyTime = DateTime.Now, LastModifyUserId = CurrentUser.Id, IsDeleted = true })
                    .Where(it => pkValues.Contains(it.Id)).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步删除数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(List<T> entities)
        {
            var timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                item.ModifierId = CurrentUser.Id;
                item.ModifyTime = timeNow;
                item.IsDeleted = true;
            });
            var columns = new string[] { "LastModifyUserId", "LastModifyTime", "IsDeleted" };
            return await UnitOfWork.GetDbClient().Updateable(entities).UpdateColumns(columns).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            entity.ModifierId = CurrentUser.Id;
            entity.ModifyTime = DateTime.Now;
            return await UnitOfWork.GetDbClient().Updateable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步更新数据（指定列名）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity, Expression<Func<T, object>> columns)
        {
            entity.ModifierId = CurrentUser.Id;
            entity.ModifyTime = DateTime.Now;

            var properties = columns.Body.Type.GetProperties();
            var updateColumns = properties.Select(a => a.Name).ToList();
            updateColumns.Add(nameof(entity.ModifierId));
            updateColumns.Add(nameof(entity.ModifyTime));

            return await UnitOfWork.GetDbClient().Updateable(entity).UpdateColumns(updateColumns.ToArray()).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步更新数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(List<T> entities)
        {
            var timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                item.ModifierId = CurrentUser.Id;
                item.ModifyTime = timeNow;
            });
            return await UnitOfWork.GetDbClient().Updateable(entities).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步更新数据(多条)（指定列名）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(List<T> entities, Expression<Func<T, object>> columns)
        {
            var timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                item.ModifierId = CurrentUser.Id;
                item.ModifyTime = timeNow;
            });
            var properties = columns.Body.Type.GetProperties();
            var updateColumns = properties.Select(a => a.Name).ToList();
            updateColumns.Add("LastModifyUserId");
            updateColumns.Add("LastModifyTime");
            return await  UnitOfWork.GetDbClient().Updateable(entities).UpdateColumns(updateColumns.ToArray()).ExecuteCommandAsync() > 0;
        }
    }
}
