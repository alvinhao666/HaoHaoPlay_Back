﻿using Hao.Core;
using Hao.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Hao.AppService
{
    public class RoleQuery : Query<SysRole>
    {
        /// <summary>
        /// 当前角色等级，只能查询后一级的角色
        /// </summary>
        public int? CurrentRoleLevel { get; set; }

        public override List<Expression<Func<SysRole, bool>>> QueryExpressions
        {
            get
            {
                List<Expression<Func<SysRole, bool>>> expressions = new List<Expression<Func<SysRole, bool>>>();
                if (CurrentRoleLevel.HasValue) expressions.Add(x => x.Level > CurrentRoleLevel);
                return expressions;
            }
        }
    }
}