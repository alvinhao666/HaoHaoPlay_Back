using Hao.Dependency;

namespace Hao.Runtime
{
    /// <summary>
    /// 当前用户接口
    /// </summary>
    public interface ICurrentUser : IScopeDependency
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long? Id { get; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int? RoleLevel { get; }

        /// <summary>
        /// json web token唯一标识
        /// </summary>
        public string Jti { get; }
    }
}