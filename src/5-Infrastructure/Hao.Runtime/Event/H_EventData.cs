using System;

namespace Hao.Runtime
{
    /// <summary>
    /// �¼�����ʵ�����
    /// </summary>
    public abstract class H_EventData
    {
        /// <summary>
        /// �¼�������
        /// </summary>
        public CurrentUser PublishUser { get; set; } //ֻ�����࣬�ӿںͳ����࣬�����л�����

        /// <summary>
        /// �¼�����ʱ��
        /// </summary>
        public DateTime PublishTime { get; set; } = DateTime.Now;
    }
}