namespace Hao.Core
{
    /// <summary>
    /// ��ҳ��ѯ������
    /// </summary>
    public abstract class PagedQuery : IPagedQuery
    {
        /// <summary>
        /// ҳ�룬Ĭ�ϵ�1ҳ
        /// </summary>
        public virtual int PageIndex { get; set; } = 1;

        /// <summary>
        /// ÿҳ������Ĭ��ÿҳ10������
        /// </summary>
        public virtual int PageSize { get; set; } = 10;
    }
}