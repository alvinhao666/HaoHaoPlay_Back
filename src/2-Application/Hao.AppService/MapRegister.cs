using Hao.Core;
using Hao.Model;
using Hao.Utility;
using Mapster;

namespace Hao.AppService
{
    /// <summary>
    /// 模型映射
    /// </summary>
    public class MapRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            MapUser(config);
            MapModule(config);
            MapRole(config);
            MapDict(config);
        }

        /// <summary>
        /// 用户
        /// </summary>
        private void MapUser(TypeAdapterConfig config)
        {
            config.ForType<SysUser, LoginVM>();

            config.ForType<PagedList<SysUser>, PagedList<UserVM>>();

            config.ForType<SysUser, UserVM>()
               .Map(x => x.GenderString, a => a.Gender.GetDescription())
               .Map(x => x.EnabledString, a => a.Enabled.IsTrue() ? "启用" : "注销");

            config.ForType<SysUser, UserDetailVM>()
               .Map(x => x.GenderString, a => a.Gender.GetDescription())
               .Map(x => x.EnabledString, a => a.Enabled.IsTrue() ? "启用" : "注销");

            config.ForType<UserAddRequest, SysUser>();


            config.ForType<SysUser, CurrentUserVM>();

            config.ForType<SysUser, UserSecurityVM>()
               .Map(x => x.PasswordLevel, a => a.PasswordLevel.GetDescription())
               .Map(x => x.Phone, a => H_Util.HidePhoneNumber(a.Phone))
               .Map(x => x.Email, a => H_Util.HideEmailNumber(a.Email));

            config.ForType<UserQueryInput, UserQuery>()
               .Map(x => x.OrderFileds, a => a.OrderByType.CombineNameWithSpace(a.SortField));

            config.ForType<UserUpdateRequest, SysUser>();
        }

        /// <summary>
        /// 模块
        /// </summary>
        private void MapModule(TypeAdapterConfig config)
        {
            #region 模块

            config.ForType<SysModule, ModuleDetailVM>()
               .Map(x => x.Code, a => string.Format("{0}_{1}", a.Layer, a.Number));

            config.ForType<ModuleAddRequest, SysModule>();
            #endregion


            #region 资源
            config.ForType<ResourceAddRequest, SysModule>();

            config.ForType<SysModule, ResourceItemVM>()
               .Map(x => x.ResourceCode, a => string.Format("{0}_{1}", a.Layer, a.Number));
            #endregion
        }

        /// <summary>
        /// 角色
        /// </summary>
        private void MapRole(TypeAdapterConfig config)
        {
            config.ForType<SysRole, RoleVM>();

            config.ForType<SysRole, RoleSelectVM>();
        }


        /// <summary>
        /// 字典
        /// </summary>
        private void MapDict(TypeAdapterConfig config)
        {
            config.ForType<DictAddRequest, SysDict>();

            config.ForType<DictItemAddRequest, SysDict>();

            config.ForType<PagedList<SysDict>, PagedList<DictVM>>();

            config.ForType<SysDict, DictVM>();

            config.ForType<PagedList<SysDict>, PagedList<DictItemVM>>();

            config.ForType<SysDict, DictItemVM>();

            config.ForType<SysDict, DictDataItemVM>();

            config.ForType<DictQueryInput, DictQuery>();
        }
    }
}
