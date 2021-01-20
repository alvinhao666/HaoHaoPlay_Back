namespace Hao.Runtime
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long? Id { get; set; }
        
        /// <summary>
        /// 用户姓名
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
