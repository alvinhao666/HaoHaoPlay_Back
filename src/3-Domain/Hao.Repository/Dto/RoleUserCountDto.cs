namespace Hao.Model
{
    public class RoleUserCountDto
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long? RoleId { get; set; }
        
        /// <summary>
        /// 用户数量
        /// </summary>
        public int UserCount { get; set; }
    }
}