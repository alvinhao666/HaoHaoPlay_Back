using FreeSql.DataAnnotations;

namespace Hao.Core
{
    public abstract class SimpleEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// ���� Ψһ��ʶ id
        /// </summary>
        [Column(IsPrimary = true)]
        public TKey Id { get; set; }

    }
}