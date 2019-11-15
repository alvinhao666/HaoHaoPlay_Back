namespace Hao.Entity
{
    /// <summary>
    /// 表示继承于该接口的类型是领域实体类。
    /// </summary>
    public interface IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 获取当前领域实体类的全局唯一标识。
        /// </summary>
        TKey Id { get; set; }
    }
}
