using Hao.Dependency;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Hao.Entity;
using Hao.Snowflake;

namespace Hao.Core
{
    public abstract class RepositoryBase<T, TKey> : IRepositoryBase<T, TKey>  where T : EntityBase<TKey>, new() where TKey : struct
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
            var entity = await Task.Factory.StartNew(() => UnitOfWork.GetDbClient().Queryable<T>().InSingle(pkValue));
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

        //C#主要支持 5 种动态创建对象的方式： 
        //            1. Type.InvokeMember 
        //            2. ContructorInfo.Invoke 
        //            3. Activator.CreateInstance(Type) 
        //            4. Activator.CreateInstance(assemblyName, typeName) 
        //            5. Assembly.CreateInstance(typeName)
        //            最快的是方式 3 ，与 Direct Create 的差异在一个数量级之内，约慢 7 倍的水平。其他方式，至少在 40 倍以上，最慢的是方式 4 ，要慢三个数量级。 

        /// <summary>
        /// 查询所有数据（未删除）（单表）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc()
        {
            return await UnitOfWork.GetDbClient().Queryable<T>().ToListAsync();
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
        /// <param name="conditions"></param>
        /// <param name="expression"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null, OrderByType orderType = OrderByType.Asc)
        {
            return await UnitOfWork.GetDbClient().Queryable<T>().Where(conditions)
                .OrderByIF(expression != null, expression, orderType)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据（单表）
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="expression"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null,OrderByType orderType = OrderByType.Asc)
        {
            return await UnitOfWork.GetDbClient().Queryable<T>().Where(conditions)
                .OrderByIF(expression != null, expression, orderType)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据（未删除）（单表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc(Query<T> query)
        {
            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
                return await UnitOfWork.GetDbClient().Queryable<T>().Where(query.Conditions)
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
            var totalNumber = 0;
            var flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            var items = await Task.Factory.StartNew(() => UnitOfWork.GetDbClient().Queryable<T>().Where(query.Conditions)
                .OrderByIF(!flag, query.OrderFileds)
                                            .ToPageList(query.PageIndex, query.PageSize, ref totalNumber));

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
        public virtual async Task<TKey> InsertAysnc(T entity)
        {
            var type = typeof(T);
            var isGuid = typeof(TKey) == typeof(Guid);
            var id = type.GetProperty("Id");

            if (isGuid)
            {
                if (id != null) id.SetValue(entity, Guid.NewGuid());
            }
            else if (id != null) id.SetValue(entity, IdWorker.NextId());
            
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
            var isGuid = typeof(TKey) == typeof(Guid);
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
                
            });
            return await Task.Factory.StartNew(() => UnitOfWork.GetDbClient().GetSimpleClient<T>().InsertRange(entities));
        }
        

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            return await UnitOfWork.GetDbClient().Updateable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步更新数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(List<T> entities)
        {
            return await Task.Factory.StartNew(() => UnitOfWork.GetDbClient().GetSimpleClient<T>().UpdateRange(entities));
        }
    }
}
