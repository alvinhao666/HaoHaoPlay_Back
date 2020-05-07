using Hao.Core;
using Hao.Enum;
using Hao.Model;
using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Hao.AppService
{
    public class ModuleQuery : Query<SysModule>
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 是否包含资源
        /// </summary>
        public bool? IncludeResource { get; set; }


        public override List<Expression<Func<SysModule, bool>>> QueryExpressions
        {
            get
            {
                List<Expression<Func<SysModule, bool>>> expressions = new List<Expression<Func<SysModule, bool>>>();
                if (Name.HasValue()) expressions.Add(x => x.Name == Name);
                if (ParentId.HasValue) expressions.Add(x => x.ParentId == ParentId);
                if (IncludeResource.IsFalse()) expressions.Add(x => x.Type != ModuleType.Resource);
                return expressions;
            }
        }
    }
}
