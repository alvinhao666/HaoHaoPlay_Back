using Hao.Core;
using Hao.Repository;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// �����ֵ�Ӧ�÷���
    /// </summary>
    public class DictAppService:ApplicationService,IDictAppService
    {
        private readonly ISysDictRepository _dictRep;
        
        public DictAppService(ISysDictRepository dictRep)
        {
            _dictRep = dictRep; 
        }


        /// <summary>
        /// ����ֵ�
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddDict(DictAddRequest request)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// ����ֵ���
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddDictItem(DictItemAddRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}