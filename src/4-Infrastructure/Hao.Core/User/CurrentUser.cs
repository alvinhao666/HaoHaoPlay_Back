namespace Hao.Core
{
    public class CurrentUser : ICurrentUser
    {
        public long? Id { get; set; }
        
        public string Name { get; set; }
        
        public int? RoleLevel { get; set; }
        
        public string Jti { get; set; }
    }
}
