using Hao.Core.Model;
using Hao.Core.Dependency;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.Core.Query;
using System.Linq.Expressions;
using Snowflake.Core;

namespace Hao.Core.Repository
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey>, ITransientDependency where T : Entity<TKey>, new()
    {
        public ISqlSugarClient Db { get; set; }
        
        public ICurrentUser CurrentUser { get; set; }
        
        public IdWorker IdWorker { get; set; }

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>s
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<T> GetAysnc(TKey pkValue)
        {
            var entity = await Task.Factory.StartNew(() => Db.Queryable<T>().Where(a => a.IsDeleted == false).InSingle(pkValue));
            return entity;
        }

        /// <summary>
        /// 根据主值查询多条数据
        /// </summary>s
        /// <param name="pkValues">主键值</param>
        /// <returns>泛型实体</returns>
        public virtual async Task<List<T>> GetListAysnc(List<TKey> pkValues)
        {
            //Type type = typeof(T); 类型判断，主要包括 is 和 typeof 两个操作符及对象实例上的 GetType 调用。这是最轻型的消耗，可以无需考虑优化问题。注意 typeof 运算符比对象实例上的 GetType 方法要快，只要可能则优先使用 typeof 运算符。 
            return await Db.Queryable<T>().In(pkValues)
                            //.OrderByIF(typeof(IsCreateAudited).IsAssignableFrom(type), "CreateTime Desc")
                            .OrderBy(a => a.CreateTime, OrderByType.Desc)
                            .ToListAsync();
        }

        //C#主要支持 5 种动态创建对象的方式： 
        //            1. Type.InvokeMember 
        //            2. ContructorInfo.Invoke 
        //            3. Activator.CreateInstance(Type) 
        //            4. Activator.CreateInstance(assemblyName, typeName) 
        //            5. Assembly.CreateInstance(typeName)
        //            最快的是方式 3 ，与 Direct Create 的差异在一个数量级之内，约慢 7 倍的水平。其他方式，至少在 40 倍以上，最慢的是方式 4 ，要慢三个数量级。 

        /// <summary>
        /// 查询所有数据（未删除）
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc()
        {
            return await Db.Queryable<T>()
                        .Where(a => a.IsDeleted == false)
                        .OrderBy(a => a.CreateTime, OrderByType.Desc)
                        .ToListAsync();
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc()
        {
            return await Db.Queryable<T>()
                        .OrderBy(a => a.CreateTime, OrderByType.Desc)
                        .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据（未删除）
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="expression"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null, OrderByType orderType = OrderByType.Asc)
        {
            return await Db.Queryable<T>().Where(conditions)
                                    .Where(a => a.IsDeleted == false)
                                    .OrderByIF(expression == null, a => a.CreateTime, OrderByType.Desc)
                                    .OrderByIF(expression != null, expression, orderType)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="expression"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null,OrderByType orderType = OrderByType.Asc)
        {
            return await Db.Queryable<T>().Where(conditions)
                                    .OrderByIF(expression == null, a => a.CreateTime, OrderByType.Desc)
                                    .OrderByIF(expression != null, expression, orderType)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetListAysnc(Query<T> query)
        {
            bool flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            return await Db.Queryable<T>().Where(query.Conditions)
                                    .Where(a => a.IsDeleted == false)
                                    .OrderByIF(flag, a => a.CreateTime, OrderByType.Desc)
                                    .OrderByIF(!flag, query.OrderFileds)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual async Task<PagedList<T>> GetPagedListAysnc(Query<T> query)
        {
            int totalNumber = 0;
            bool flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            List<T> items = await Task.Factory.StartNew(() => Db.Queryable<T>().Where(query.Conditions)
                                            .Where(a => a.IsDeleted == false)
                                            .OrderByIF(flag, a => a.CreateTime, OrderByType.Desc)
                                            .OrderByIF(!flag, query.OrderFileds)
                                            .ToPageList(query.PageIndex, query.PageSize, ref totalNumber));

            PagedList<T> pageList = new PagedList<T>()
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
            Type type = typeof(T);
            bool isGuid = typeof(TKey) == typeof(Guid);
            var id = type.GetProperty("Id");

            if (isGuid)
            {
                if (id != null) id.SetValue(entity, Guid.NewGuid());
            }
            else if (id != null) id.SetValue(entity, IdWorker.NextId());

            entity.CreaterId = CurrentUser.UserId;
            entity.CreateTime = DateTime.Now;
            entity.IsDeleted = false;

            var obj = await Db.Insertable(entity).ExecuteReturnEntityAsync();
            return obj.Id;
        }

        /// <summary>
        /// 异步写入实体数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAysnc(List<T> entities)
        {
            bool isGuid = typeof(TKey) == typeof(Guid);
            Type type = typeof(T);
            var id = type.GetProperty("Id");
            DateTime timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                if (isGuid)
                {
                    if (id != null) id.SetValue(item, Guid.NewGuid());
                }
                else if (id != null) id.SetValue(item, IdWorker.NextId());

                item.CreaterId = CurrentUser.UserId;
                item.CreateTime = timeNow;
                item.IsDeleted = false;
            });
            return await Task.Factory.StartNew(() => Db.GetSimpleClient<T>().InsertRange(entities.ToArray()));
        }

        /// <summary>
        /// 异步删除数据(逻辑删除)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(T entity)
        {
            entity.LastModifyUserId = CurrentUser.UserId;
            entity.LastModifyTime = DateTime.Now;
            entity.IsDeleted = true;
            return await Db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValue">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(TKey pkValue)
        {
            return await Db.Updateable<T>(new { LastModifyTime = DateTime.Now, LastModifyUserId = CurrentUser.UserId, IsDeleted = true })
                        .Where($"Id='{pkValue}'").ExecuteCommandAsync() > 0;

        }

        /// <summary>
        /// 异步删除数据(多条)
        /// </summary>
        /// <param name="pkValues">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(List<TKey> pkValues)
        {
            return await Db.Updateable<T>(new { LastModifyTime = DateTime.Now, LastModifyUserId = CurrentUser.UserId, IsDeleted = true })
                    .Where(it => pkValues.Contains(it.Id)).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步删除数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAysnc(List<T> entities)
        {
            DateTime timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                item.LastModifyUserId = CurrentUser.UserId;
                item.LastModifyTime = timeNow;
                item.IsDeleted = true;
            });
            return await Task.Factory.StartNew(() => Db.GetSimpleClient<T>().UpdateRange(entities.ToArray()));
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            entity.LastModifyUserId = CurrentUser.UserId;
            entity.LastModifyTime = DateTime.Now;
            return await Db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步更新数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(List<T> entities)
        {
            DateTime timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                item.LastModifyUserId = CurrentUser.UserId;
                item.LastModifyTime = timeNow;
            });
            return await Task.Factory.StartNew(() => Db.GetSimpleClient<T>().UpdateRange(entities.ToArray()));
        }
    }
}
