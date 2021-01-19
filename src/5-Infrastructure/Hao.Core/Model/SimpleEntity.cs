using FreeSql.DataAnnotations;

namespace Hao.Core
{
    /// <summary>
    /// 简单实体类，不包括创建信息，修改信息，逻辑删除
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class SimpleEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Column(IsPrimary = true)]
        public TKey Id { get; set; }

    }
}