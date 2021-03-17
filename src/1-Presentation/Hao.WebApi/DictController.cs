using Hao.AppService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Runtime;
using System;

namespace Hao.WebApi
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictController : H_Controller
    {
        private readonly IDictAppService _dictAppService;

        public DictController(IDictAppService dictAppService, ICurrentUser currentUser)
        {
            //Console.WriteLine(currentUser.GetHashCode());

            //var user = ServiceLocator.Resolve<ICurrentUser>();

            //Console.WriteLine(user.GetHashCode());

            //Console.WriteLine(currentUser.Equals(user));

            _dictAppService = dictAppService;
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("Dict_Add_1_2097152")]
        public async Task AddDict([FromBody] DictAddInput input) => await _dictAppService.Add(input);

        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("Dict_Search_1_1048576")]
        public async Task<Paged<DictOutput>> GetDictPagedList([FromQuery] DictQueryInput queryInput) => await _dictAppService.GetPaged(queryInput);

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("Dict_Edit_1_4194304")]
        public async Task UpdateDict(long? id, [FromBody] DictUpdateInput input) => await _dictAppService.Update(id.Value, input);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("Dict_Delete_1_8388608")]
        public async Task DeleteDict(long? id) => await _dictAppService.Delete(id.Value);

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("Dict_Add_1_2097152")]
        public async Task AddDictItem([FromBody] DictItemAddInput input) => await _dictAppService.AddDictItem(input);

        /// <summary>
        /// 查询字典数据项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("Dict_Search_1_1048576")]
        public async Task<Paged<DictItemOutput>> GetDictItemPagedList([FromQuery] DictQueryInput queryInput) => await _dictAppService.GetDictItemPaged(queryInput);

        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("Dict_Edit_1_4194304")]
        public async Task UpdateDictItem(long? id, [FromBody] DictItemUpdateInput input) => await _dictAppService.UpdateDictItem(id.Value, input);

        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("Dict_Delete_1_8388608")]
        public async Task DeleteDictItem(long? id) => await _dictAppService.DeleteDictItem(id.Value);
    }
}