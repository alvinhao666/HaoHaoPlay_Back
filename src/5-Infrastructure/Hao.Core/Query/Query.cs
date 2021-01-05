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
        /// 适用于单表
        /// </summary>
        public abstract List<Expression<Func<T, bool>>> QueryExpressions { get; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public List<OrderByInfo> OrderByConditions { get; set; } = new List<OrderByInfo>();
        

        ///// <summary>
        ///// 适用于多表
        ///// </summary>
        //public virtual string QuerySql { get; }

        ///// <summary>
        ///// 排序字段
        ///// </summary>
        //public virtual string OrderByFields { get; set; }


        public Query<T> OrderBy(string fieldName)
        {
            if (OrderByConditions == null) OrderByConditions = new List<OrderByInfo>();
            
            OrderByConditions.Add(new OrderByInfo {FieldName = fieldName, IsAsc = true});

            return this;
        }
        
        public Query<T> OrderByDescending(string fieldName)
        {
            if (OrderByConditions == null) OrderByConditions = new List<OrderByInfo>();
            
            OrderByConditions.Add(new OrderByInfo {FieldName = fieldName, IsAsc = false});

            return this;
        }
    }


    public class OrderByInfo
    {
        public string FieldName { get; set; }


        public bool IsAsc { get; set; } = true;
    }
}
