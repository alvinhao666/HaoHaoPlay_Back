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
        /// 字典名称
        /// </summary>
        public string EqualDictName { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
        
        /// <summary>
        /// 数据项名称，模糊查询
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 数据项名称
        /// </summary>
        public string EqualItemName { get; set; }

        /// <summary>
        /// 数据项值
        /// </summary>
        public int? ItemValue { get; set; }

        public override List<Expression<Func<SysDict, bool>>> QueryExpressions
        {

            get
            {
                var result = new List<Expression<Func<SysDict, bool>>>();

                if (DictCode.HasValue()) result.Add(x => x.DictCode.Contains(DictCode));

                if (DictName.HasValue()) result.Add(x => x.DictName.Contains(DictName));

                if (EqualDictName.HasValue()) result.Add(x => x.DictName == EqualDictName);

                if (ItemName.HasValue()) result.Add(x => x.ItemName.Contains(ItemName));

                if (EqualItemName.HasValue()) result.Add(x => x.ItemName == EqualItemName);

                if (ParentId.HasValue)
                {
                    result.Add(x => x.ParentId == ParentId);
                }
                else
                {
                    result.Add(x => x.ParentId == null);
                }

                if (ItemValue.HasValue) result.Add(x => x.ItemValue == ItemValue);


                return result;
            }
        }


    }
}