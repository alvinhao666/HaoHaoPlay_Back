namespace Hao.AppService
{
    public class RoleVM
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long? Id { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 用户数量
        /// </summary>
        public int? UserCount { get; set; }
    }
}