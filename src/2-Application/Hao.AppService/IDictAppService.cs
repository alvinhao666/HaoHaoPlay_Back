using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IDictAppService
    {
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddDict(DictAddRequest request);


        /// <summary>
        /// 添加字典数据项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddDictItem(DictItemAddRequest request);
    }
}