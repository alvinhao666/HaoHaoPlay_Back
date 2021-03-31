namespace Hao.Core
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 主键id
        /// </summary>
        TKey Id { get; set; }
    }
}
