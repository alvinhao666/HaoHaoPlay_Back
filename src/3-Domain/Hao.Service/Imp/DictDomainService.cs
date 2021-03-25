using Hao.Core;
using Hao.Model;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 字典领域服务
    /// </summary>
    public class DictDomainService : DomainService, IDictDomainService
    {
        private readonly IDictRepository _dictRep;

        public DictDomainService(IDictRepository dictRep)
        {
            _dictRep = dictRep;
        }

        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysDict> Get(long id)
        {
            var dict = await _dictRep.GetAsync(id);

            H_AssertEx.That(dict == null, "字典数据不存在");
            H_AssertEx.That(dict.IsDeleted, "字典数据已删除");

            return dict;
        }
    }
}
