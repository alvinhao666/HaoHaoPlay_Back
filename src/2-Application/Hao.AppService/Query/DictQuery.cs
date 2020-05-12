using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hao.Core;
using Hao.Model;
using Hao.Utility;

namespace Hao.AppService
{
    /// <summary>
    /// 字典查询
    /// </summary>
    public class DictQuery : Query<SysDict>
    {
        /// <summary>
        /// 字典编码
        /// </summary>
        public string DictCode { get; set; }
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictName { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }

        public override List<Expression<Func<SysDict, bool>>> QueryExpressions
        {

            get
            {
                var result = new List<Expression<Func<SysDict, bool>>>();

                if (DictCode.HasValue()) result.Add(x => x.DictCode.Contains(DictCode));

                if (DictName.HasValue()) result.Add(x => x.DictName.Contains(DictName) );

                if (ParentId.HasValue) result.Add(x => x.ParentId == ParentId);

                return result;
            }
        }


    }
}