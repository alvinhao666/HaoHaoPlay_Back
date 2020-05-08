namespace Hao.Core
{
    public interface ICurrentUser
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long? Id { get; }

        /// <summary>
        /// 姓名
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
