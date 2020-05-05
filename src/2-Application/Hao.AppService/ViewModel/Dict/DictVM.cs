namespace Hao.AppService.ViewModel.Dict
{
    public class DictVM
    {
        public long? Id { get; set; }
        
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictName { get; set; }
        
        /// <summary>
        /// 字典编码
        /// </summary>
        public string DictCode { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}