using Hao.Core;
using Hao.Repository;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 数据字典应用服务
    /// </summary>
    public class DictAppService:ApplicationService,IDictAppService
    {
        private readonly ISysDictRepository _dictRep;
        
        public DictAppService(ISysDictRepository dictRep)
        {
            _dictRep = dictRep; 
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddDict(DictAddRequest request)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddDictItem(DictItemAddRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}