using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hao.Core
{
    public abstract class Query<T> : IPagedQuery where T : new()
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

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
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Query<T> OrderBy(string fieldName)
        {
            if (OrderByConditions == null) OrderByConditions = new List<OrderByInfo>();
            
            OrderByConditions.Add(new OrderByInfo {FieldName = fieldName, IsAsc = true});

            return this;
        }
        
        /// <summary>
        /// 降序
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Query<T> OrderByDescending(string fieldName)
        {
            if (OrderByConditions == null) OrderByConditions = new List<OrderByInfo>();
            
            OrderByConditions.Add(new OrderByInfo {FieldName = fieldName, IsAsc = false});

            return this;
        }
    }


    public class OrderByInfo
    {
        public string FieldName { get; internal set; }


        public bool IsAsc { get; internal set; }
    }
}
