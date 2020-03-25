using Hao.Core;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Hao.AppService
{
    public class ModuleQuery : Query<SysModule>
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        public override List<Expression<Func<SysModule, bool>>> QueryExpressions
        {
            get
            {
                List<Expression<Func<SysModule, bool>>> expressions = new List<Expression<Func<SysModule, bool>>>();
                if (!string.IsNullOrWhiteSpace(Name)) expressions.Add(x => x.Name == Name);
                return expressions;
            }
        }
    }
}
