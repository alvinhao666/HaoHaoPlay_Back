namespace Hao.Runtime
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        public long? Id { get; set; }
        
        public string Name { get; set; }
        
        public int? RoleLevel { get; set; }
        
        public string Jti { get; set; }
    }
}
