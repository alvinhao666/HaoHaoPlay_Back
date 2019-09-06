using Hao.Core.Model;
using Hao.Core.Dependency;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Core.Query;
using System.Linq;
using System.Linq.Expressions;
using Snowflake.Core;
using Microsoft.Extensions.Configuration;

namespace Hao.Core.Repository
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey>, ITransientDependency where T : Entity<TKey>, new()
    {
        private ISqlSugarClient _db;

        private static IdWorker _worker = null;

        // 定义一个标识确保线程同步
        private static readonly object _padlock = new object();

        private IConfiguration _config;

        private ICurrentUser _currentUser;
        public Repository(ISqlSugarClient db, ICurrentUser user, IConfiguration config)
        {
            _db = db;
            _currentUser = user;
            _config = config;
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            if (_worker == null)
            {
                lock (_padlock)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (_worker == null)
                    {
                        _worker = new IdWorker(_config["Snowflake:WorkerID"].ObjToInt(), _config["Snowflake:DataCenterID"].ObjToInt());
                    }
                }
            }
        }

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>s
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public async Task<T> GetAysnc(TKey pkValue)
        {
            var entity = await Task.Factory.StartNew(() => _db.Queryable<T>().Where(a => a.IsDeleted == false).InSingle(pkValue));
            return entity;
        }

        /// <summary>
        /// 根据主值查询多条数据
        /// </summary>s
        /// <param name="pkValues">主键值</param>
        /// <returns>泛型实体</returns>
        public async Task<List<T>> GetListAysnc(List<TKey> pkValues)
        {
            //Type type = typeof(T); 类型判断，主要包括 is 和 typeof 两个操作符及对象实例上的 GetType 调用。这是最轻型的消耗，可以无需考虑优化问题。注意 typeof 运算符比对象实例上的 GetType 方法要快，只要可能则优先使用 typeof 运算符。 
            return await _db.Queryable<T>().In(pkValues)
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
        public async Task<List<T>> GetListAysnc()
        {
            return await _db.Queryable<T>()
                        .Where(a => a.IsDeleted == false)
                        .OrderBy(a => a.CreateTime, OrderByType.Desc)
                        .ToListAsync();
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAysnc()
        {
            return await _db.Queryable<T>()
                        .OrderBy(a => a.CreateTime, OrderByType.Desc)
                        .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据（未删除）
        /// </summary>
        /// <param name="querys"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null, OrderByType orderType = OrderByType.Asc)
        {
            return await _db.Queryable<T>().Where(conditions)
                                    .Where(a => a.IsDeleted == false)
                                    .OrderByIF(expression == null, a => a.CreateTime, OrderByType.Desc)
                                    .OrderByIF(expression != null, expression, orderType)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="querys"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAllAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null,OrderByType orderType = OrderByType.Asc)
        {
            return await _db.Queryable<T>().Where(conditions)
                                    .OrderByIF(expression == null, a => a.CreateTime, OrderByType.Desc)
                                    .OrderByIF(expression != null, expression, orderType)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="querys"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAysnc(Query<T> query)
        {
            bool flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            return await _db.Queryable<T>().Where(query.Conditions)
                                    .Where(a => a.IsDeleted == false)
                                    .OrderByIF(flag, a => a.CreateTime, OrderByType.Desc)
                                    .OrderByIF(!flag, query.OrderFileds)
                                    .ToListAsync();
        }

        /// <summary>
        /// 根据条件查询所有分页数据
        /// </summary>
        /// <param name="querys"></param>
        /// <returns></returns>
        public async Task<PagedList<T>> GetPagedListAysnc(Query<T> query)
        {
            int totalNumber = 0;
            bool flag = string.IsNullOrWhiteSpace(query.OrderFileds);
            List<T> items = await Task.Factory.StartNew(() => _db.Queryable<T>().Where(query.Conditions)
                                            .Where(a => a.IsDeleted == false)
                                            .OrderByIF(flag, a => a.CreateTime, OrderByType.Desc)
                                            .OrderByIF(!flag, query.OrderFileds)
                                            .ToPageList(query.PageIndex.Value, query.PageSize.Value, ref totalNumber));

            PagedList<T> pageList = new PagedList<T>()
            {
                Items = items,
                TotalCount = totalNumber,
                PageIndex = query.PageIndex.Value,
                PageSize = query.PageSize.Value,
                TotalPagesCount = (totalNumber + query.PageSize.Value - 1) / query.PageSize.Value
            };
            return pageList;
        }

        /// <summary>
        /// 异步写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<TKey> InsertAysnc(T entity)
        {
            Type type = typeof(T);
            bool isGuid = typeof(TKey) == typeof(Guid);
            var id = type.GetProperty("ID");

            if (isGuid)
                id.SetValue(entity, Guid.NewGuid());
            else
                id.SetValue(entity, _worker.NextId());

            entity.CreaterID = _currentUser.UserID;
            entity.CreateTime = DateTime.Now;
            entity.IsDeleted = false;

            var obj = await _db.Insertable(entity).ExecuteReturnEntityAsync();
            return obj.ID;
        }

        /// <summary>
        /// 异步写入实体数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public async Task<bool> InsertAysnc(List<T> entities)
        {
            bool isGuid = typeof(TKey) == typeof(Guid);
            Type type = typeof(T);
            var id = type.GetProperty("ID");
            DateTime timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                if (isGuid)
                    id.SetValue(item, Guid.NewGuid());
                else
                    id.SetValue(item, _worker.NextId());
                item.CreaterID = _currentUser.UserID;
                item.CreateTime = timeNow;
                item.IsDeleted = false;
            });
            return await Task.Factory.StartNew(() => _db.GetSimpleClient<T>().InsertRange(entities.ToArray()));
        }

        /// <summary>
        /// 异步删除数据(逻辑删除)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public async Task<bool> DeleteAysnc(T entity)
        {
            entity.LastModifyUserID = _currentUser.UserID;
            entity.LastModifyTime = DateTime.Now;
            entity.IsDeleted = true;
            return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValue">实体类</param>
        /// <returns></returns>
        public async Task<bool> DeleteAysnc(TKey pkValue)
        {
            return await _db.Updateable<T>(new { LastModifyTime = DateTime.Now, LastModifyUserID = _currentUser.UserID, IsDeleted = true })
                        .Where($"ID='{pkValue}'").ExecuteCommandAsync() > 0;

        }

        /// <summary>
        /// 异步删除数据(多条)
        /// </summary>
        /// <param name="pkValues">实体类</param>
        /// <returns></returns>
        public async Task<bool> DeleteAysnc(List<TKey> pkValues)
        {
            return await _db.Updateable<T>(new { LastModifyTime = DateTime.Now, LastModifyUserID = _currentUser.UserID, IsDeleted = true })
                    .Where(it => pkValues.Contains(it.ID)).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步删除数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public async Task<bool> DeleteAysnc(List<T> entities)
        {
            DateTime timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                item.LastModifyUserID = _currentUser.UserID;
                item.LastModifyTime = timeNow;
                item.IsDeleted = true;
            });
            return await Task.Factory.StartNew(() => _db.GetSimpleClient<T>().UpdateRange(entities.ToArray()));
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            entity.LastModifyUserID = _currentUser.UserID;
            entity.LastModifyTime = DateTime.Now;
            return await _db.Updateable(entity).ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 异步更新数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<T> entities)
        {
            DateTime timeNow = DateTime.Now;
            entities.ForEach(item =>
            {
                item.LastModifyUserID = _currentUser.UserID;
                item.LastModifyTime = timeNow;
            });
            return await Task.Factory.StartNew(() => _db.GetSimpleClient<T>().UpdateRange(entities.ToArray()));
        }
    }
}
