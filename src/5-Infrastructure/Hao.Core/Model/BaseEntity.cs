using SqlSugar;

namespace Hao.Core
{
    public abstract class BaseEntity<TKey> : IEntity<TKey> where TKey : struct
    {

        [SugarColumn(IsPrimaryKey = true)]
        public TKey Id { get; set; }

    }
}