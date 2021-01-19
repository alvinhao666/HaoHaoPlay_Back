using FreeSql.DataAnnotations;

namespace Hao.Core
{
    /// <summary>
    /// 简单实体，不包含创建信息，修改信息，删除信息
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class SimpleEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 主键 唯一标识 id
        /// </summary>
        [Column(IsPrimary = true)]
        public TKey Id { get; set; }

    }
}