using Hao.Core.Model;
using Hao.Core.Query;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hao.Core.Repository
{
    /// <summary>
    /// 仓储通用接口类
    /// </summary>
    /// <typeparam name="T">泛型实体类</typeparam>
    public interface IRepository<T,Key> where T : Entity<Key>, new()
    {
        /// <summary>
        /// 根据主键值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        Task<T> GetAysnc(Key pkValue);

        /// <summary>
        /// 根据主键值查询多条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        Task<List<T>> GetListAysnc(List<Key> pkValues);

        /// <summary>
        /// 查询所有数据（未删除）
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAysnc();

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAysnc();

        /// <summary>
        /// 根据条件查询所有数据（未删除）
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null, OrderByType orderType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAysnc(List<IConditionalModel> conditions, Expression<Func<T, object>> expression = null, OrderByType orderType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAysnc(Query<T> query);

        /// <summary>
        /// 根据条件查询所有分页数据
        /// </summary>
        /// <returns></returns>
        Task<PagedList<T>> GetPagedListAysnc(Query<T> query);

        /// <summary>
        /// 异步写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<Key> InsertAysnc(T entity);

        /// <summary>
        /// 异步写入实体数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        Task<bool> InsertAysnc(List<T> entities);

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<bool> DeleteAysnc(T entity);

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValue">实体类</param>
        /// <returns></returns>
        Task<bool> DeleteAysnc(Key pkValue);

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValues">实体类</param>
        /// <returns></returns>
        Task<bool> DeleteAysnc(List<Key> pkValues);

        /// <summary>
        /// 异步删除数据(多条)
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        Task<bool> DeleteAysnc(List<T> entities);

        /// <summary>
        /// 异步更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// 异步更新实体数据(多条)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> entities);
    }
}
