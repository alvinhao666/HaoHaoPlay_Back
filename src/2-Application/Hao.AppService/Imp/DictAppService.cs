using Hao.Core;
using Hao.Repository;

namespace Hao.AppService
{
    public class DictAppService:ApplicationService,IDictAppService
    {
        private readonly ISysDictRepository _dictRep;
        
        public DictAppService(ISysDictRepository dictRep)
        {
            _dictRep = dictRep; 
        }
    }
}