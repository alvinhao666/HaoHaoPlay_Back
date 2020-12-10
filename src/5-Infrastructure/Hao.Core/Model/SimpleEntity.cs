using FreeSql.DataAnnotations;

namespace Hao.Core
{
    public abstract class SimpleEntity<TKey> : IEntity<TKey> where TKey : struct
    {

        [Column(IsPrimary = true)]
        public TKey Id { get; set; }

    }
}