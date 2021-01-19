using FreeSql.DataAnnotations;

namespace Hao.Core
{
    /// <summary>
    /// ��ʵ�壬������������Ϣ���޸���Ϣ��ɾ����Ϣ
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class SimpleEntity<TKey> : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// ���� Ψһ��ʶ id
        /// </summary>
        [Column(IsPrimary = true)]
        public TKey Id { get; set; }

    }
}