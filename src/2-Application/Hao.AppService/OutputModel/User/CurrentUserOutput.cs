using Hao.Enum;

namespace Hao.AppService
{
    public class CurrentUserOutput
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadImgUrl { get; set; }

        // /// <summary>
        // /// 昵称
        // /// </summary>
        // public string NickName { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// 姓的拼音首字母
        /// </summary>
        public string FirstNameInitial { get; set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// 维系
        /// </summary>
        public string WeChat { get; set; }
    }
}