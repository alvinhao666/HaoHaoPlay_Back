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

        /// <summary>
        /// 检查是否存在相同名字or相同编码
        /// </summary>
        /// <returns></returns>
        public async Task CheckNameOrCode(string name, string code)
        {
            var sameItems = await _dictRep.GetListAsync(new DictQuery { DictName = name });

            H_AssertEx.That(sameItems.Count > 0, "字典名称已存在，请重新输入");

            sameItems = await _dictRep.GetListAsync(new DictQuery { DictCode = code });

            H_AssertEx.That(sameItems.Count > 0, "字典名称已存在，请重新输入");
        }

        /// <summary>
        /// 检查字典数据项是否存在相同名字or相同值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task CheckItemNameOrValue(string name, int value, long parentId)
        {
            var sameItems = await _dictRep.GetListAsync(new DictQuery { ParentId = parentId, ItemName = name });

            H_AssertEx.That(sameItems.Count > 0, "数据项名称已存在，请重新输入");


            sameItems = await _dictRep.GetListAsync(new DictQuery { ParentId = parentId, ItemValue = value });

            H_AssertEx.That(sameItems.Count > 0, "数据项值已存在，请重新输入");
        }
    }
}
