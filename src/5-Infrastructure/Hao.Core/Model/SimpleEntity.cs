using FreeSql.DataAnnotations;

namespace Hao.Core
{
    public abstract class SimpleEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 主键 唯一标识 id
        /// </summary>
        [Column(IsPrimary = true)]
        public TKey Id { get; set; }

    }
}