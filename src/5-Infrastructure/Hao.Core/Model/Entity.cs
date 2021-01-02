using FreeSql.DataAnnotations;
using System;

namespace Hao.Core
{
    public abstract class Entity<TKey> : IEntity<TKey>, IsCreateAudited, IsModifyAudited, IsSoftDelete where TKey : struct
    {
        /// <summary>
        /// 主键 唯一标识 id
        /// </summary>
        [Column(IsPrimary = true)]
        public TKey Id { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public long? CreatorId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改人id
        /// </summary>
        public long? ModifierId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDeleted { get; set; }

    }
}
