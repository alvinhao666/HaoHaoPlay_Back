using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hao.Core
{
    /// <summary>
    /// 实体查询类，适用于单表查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Query<T> : PagedQuery where T : new()
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public abstract List<Expression<Func<T, bool>>> QueryExpressions { get; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public List<OrderByInfo> OrderByConditions { get; set; } = new List<OrderByInfo>();

        /// <summary>
        /// 升序
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public Query<T> OrderBy<TMember>(Expression<Func<T, TMember>> column)
        {
            var body = column.Body as MemberExpression;

            ThenBy(body.Member.Name);

            return this;
        }

        /// <summary>
        /// 降序
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public Query<T> OrderByDescending<TMember>(Expression<Func<T, TMember>> column)
        {
            var body = column.Body as MemberExpression;

            ThenBy(body.Member.Name, false);

            return this;
        }


        private void ThenBy(string fieldName, bool isAsc = true)
        {
            if (OrderByConditions == null) OrderByConditions = new List<OrderByInfo>();

            OrderByConditions.Add(new OrderByInfo { FieldName = fieldName, IsAsc = isAsc });
        }
    }

    /// <summary>
    /// 排序信息
    /// </summary>
    public class OrderByInfo
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; internal set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAsc { get; internal set; }
    }
}
