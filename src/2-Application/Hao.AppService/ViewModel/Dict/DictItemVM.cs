namespace Hao.AppService.ViewModel.Dict
{
    public class DictItemVM
    {
        public long? Id { get; set; }
        
        /// <summary>
        /// 数据项名称
        /// </summary>
        public string ItemName { get; set; }
        
        /// <summary>
        /// 数据项值
        /// </summary>
        public string ItemValue { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}