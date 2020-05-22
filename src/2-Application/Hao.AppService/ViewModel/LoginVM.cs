using System.Collections.Generic;

namespace Hao.AppService.ViewModel
{
    public class LoginVM
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 姓的拼音首字母
        /// </summary>
        public string FirstNameSpell { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Jwt { get; set; }

        /// <summary>
        /// 权限值集合
        /// </summary>
        public List<long> AuthNums { get; set; }

        /// <summary>
        /// 拥有菜单集合
        /// </summary>
        public List<MenuVM> Menus { get; set; }
    }

    /// <summary>
    /// 应用菜单
    /// </summary>
    public class MenuVM
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string RouterUrl { get; set; }

        /// <summary>
        /// 子应用菜单
        /// </summary>
        public List<MenuVM> ChildMenus { get; set; }
    }
}
