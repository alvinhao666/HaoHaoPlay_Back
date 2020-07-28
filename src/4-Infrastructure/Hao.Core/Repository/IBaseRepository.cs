using Hao.Dependency;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hao.Core
{
    /// <summary>
    /// 仓储通用接口类
    /// </summary>
    /// <typeparam name="T">泛型实体类</typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IBaseRepository<T, TKey> : ITransientDependency where T :IEntity<TKey>, new() where TKey : struct
    {
        /// <summary>
        /// 根据主键值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        Task<T> GetAysnc(TKey pkValue);

        /// <summary>
        /// 根据主键值查询多条数据
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns>泛型实体</returns>
        Task<List<T>> GetListAysnc(List<TKey> pkValues);

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAysnc();

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAysnc(Query<T> query);
        
        /// <summary>
        /// 根据条件查询所有数据数量（未删除）
        /// </summary>
        /// <returns></returns>
        Task<int> GetCountAysnc(Query<T> query);

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
        Task<T> InsertAysnc(T entity);

        ///// <summary>
        ///// 异步写入实体数据
        ///// </summary>
        ///// <param name="entity">实体类</param>
        ///// <returns></returns>
        //T Insert(T entity);

        /// <summary>
        /// 异步写入实体数据（批量）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        Task<bool> InsertAysnc(List<T> entities);

        ///// <summary>
        ///// 写入实体数据（批量）
        ///// </summary>
        ///// <param name="entities">实体类</param>
        ///// <returns></returns>
        //bool Insert(List<T> entities);

        /// <summary>
        /// 异步更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        ///// <summary>
        ///// 更新实体数据
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //bool Update(T entity);

        /// <summary>
        /// 异步更新实体数据（指定列）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity, Expression<Func<T, object>> columns);

        ///// <summary>
        ///// 更新实体数据（指定列）
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //bool Update(T entity, Expression<Func<T, object>> columns);

        /// <summary>
        /// 异步更新实体数据（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> entities);

        ///// <summary>
        ///// 更新实体数据（批量）
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <returns></returns>
        //bool Update(List<T> entities);


        /// <summary>
        /// 异步更新实体数据（批量）（指定列）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> entities, Expression<Func<T, object>> columns);

        ///// <summary>
        ///// 更新实体数据（批量）（指定列）
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <returns></returns>
        //bool Update(List<T> entities, Expression<Func<T, object>> columns);

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        Task<bool> DeleteAysnc(TKey pkValue);

        ///// <summary>
        ///// 异步删除数据
        ///// </summary>
        ///// <param name="pkValue"></param>
        ///// <returns></returns>
        //bool Delete(TKey pkValue);

        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        Task<bool> DeleteAysnc(List<TKey> pkValues);

        ///// <summary>
        ///// 异步删除数据
        ///// </summary>
        ///// <param name="pkValues"></param>
        ///// <returns></returns>
        //bool Delete(List<TKey> pkValues);
    }
}
