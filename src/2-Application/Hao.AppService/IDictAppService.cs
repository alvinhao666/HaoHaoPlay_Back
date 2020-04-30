using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IDictAppService
    {
        /// <summary>
        /// ����ֵ�
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddDict(DictAddRequest request);


        /// <summary>
        /// ����ֵ�������
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddDictItem(DictItemAddRequest request);
    }
}