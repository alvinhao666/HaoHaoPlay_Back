﻿using Hao.Dependency;

namespace Hao.Core
{
    public interface ICurrentUser : IScopeDependency
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int? RoleLevel { get; set; }

        /// <summary>
        /// json web token唯一标识
        /// </summary>
        public string Jti { get; set; }
    }
}