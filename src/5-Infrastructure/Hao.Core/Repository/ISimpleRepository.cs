using Hao.Dependency;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hao.Core
{
    /// <summary>
    /// 仓储通用接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ISimpleRepository<T, TKey> : ITransientDependency where T : IEntity<TKey>, new() where TKey : struct
    {
        /// <summary>
        /// 根据主键值查询单条数据
        /// </summary>
        /// <param name="pkValue"></param>
        /// <returns></returns>
        Task<T> GetAysnc(TKey pkValue);

        /// <summary>
        /// 根据主键集合查询多条数据
        /// </summary>
        /// <param name="pkValues"></param>
        /// <returns></returns>
        Task<List<T>> GetListAysnc(List<TKey> pkValues);

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAysnc();

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAysnc(Query<T> query);

        /// <summary>
        /// 根据条件查询所有数据数量
        /// </summary>
        /// <returns></returns>
        Task<int> GetCountAysnc(Query<T> query);

        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <returns></returns>
        Task<Paged<T>> GetPagedAysnc(Query<T> query);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<T> InsertAysnc(T entity);

        /// <summary>
        /// 插入数据（批量）
        /// </summary>
        /// <param name="entities">实体类</param>
        /// <returns></returns>
        Task<List<T>> InsertAysnc(List<T> entities);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity, params Expression<Func<T, bool>>[] whereColumns);

        /// <summary>
        /// 更新数据（指定列）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity, Expression<Func<T, object>> updateColumns, params Expression<Func<T, bool>>[] whereColumns);

        /// <summary>
        /// 更新数据（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<T> entities, params Expression<Func<T, bool>>[] whereColumns);

        /// <summary>
        /// 更新数据（批量）（指定列）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<T> entities, Expression<Func<T, object>> updateColumns, params Expression<Func<T, bool>>[] whereColumns);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAysnc(T entity, params Expression<Func<T, bool>>[] whereColumns);

        /// <summary>
        /// 删除数据（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> DeleteAysnc(List<T> entities, params Expression<Func<T, bool>>[] whereColumns);

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name = "pkValue" ></ param >
        ///// < param name="whereColumns"></param>
        ///// <returns></returns>
        ////Task<int> DeleteAysnc(TKey pkValue, params Expression<Func<T, bool>>[] whereColumns);

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name = "pkValues" ></ param >
        ///// < param name="whereColumns"></param>
        ///// <returns></returns>
        ////Task<int> DeleteAysnc(List<TKey> pkValues, params Expression<Func<T, bool>>[] whereColumns);
    }
}