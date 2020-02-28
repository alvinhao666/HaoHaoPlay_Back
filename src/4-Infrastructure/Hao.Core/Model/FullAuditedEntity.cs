using Hao.Entity;
using SqlSugar;
using System;

namespace Hao.Core
{
    public abstract class FullAuditedEntity<TKey> : IEntity<TKey>, IsCreateAudited, IsModifyAudited, IsSoftDelete where TKey: struct
    {

        [SugarColumn(IsPrimaryKey = true)]
        public TKey Id { get; set; }

        public long? CreaterId { get; set; }
        public DateTime? CreateTime { get; set; }

        public long? LastModifyUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }

        public bool IsDeleted { get; set; }

    }
}
