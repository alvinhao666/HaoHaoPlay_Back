using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core.Model
{
    public abstract class Entity<TKey> : IEntity<TKey>, IsCreateAudited, IsModifyAudited, IsSoftDelete
    {

        [SugarColumn(IsPrimaryKey = true)]
        public TKey ID { get; set; }

        public long? CreaterID { get; set; }
        public DateTime? CreateTime { get; set; }

        public long? LastModifyUserID { get; set; }
        public DateTime? LastModifyTime { get; set; }

        public bool? IsDeleted { get; set; }

    }
}
