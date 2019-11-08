namespace Hao.Entity
{
    /// <summary>
    /// 逻辑删除
    /// </summary>
    public interface IsSoftDelete
    {
        /// <summary>
        /// 是否已被删除
        /// </summary>
        bool? IsDeleted { get; set; }
    }
}
