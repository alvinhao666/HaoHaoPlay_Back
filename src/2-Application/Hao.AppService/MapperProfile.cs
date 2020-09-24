using AutoMapper;
using Hao.Core;
using Hao.Model;
using Hao.Utility;

namespace Hao.AppService
{
    /// <summary>
    /// 模型映射
    /// </summary>
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            MapUser();

            MapModule();

            MapRole();

            MapDict();
        }

        /// <summary>
        /// 用户
        /// </summary>
        private void MapUser()
        {
            CreateMap<SysUser, LoginVM>();

            CreateMap<PagedList<SysUser>, PagedList<UserVM>>();

            CreateMap<SysUser, UserVM>()
               .ForMember(x => x.GenderString, a => a.MapFrom(x => x.Gender.GetDescription()))
               .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.IsTrue() ? "启用" : "注销"));

            CreateMap<SysUser, UserDetailVM>()
               .ForMember(x => x.GenderString, a => a.MapFrom(x => x.Gender.GetDescription()))
               .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.IsTrue() ? "启用" : "注销"));

            CreateMap<UserAddRequest, SysUser>();


            CreateMap<SysUser, CurrentUserVM>();

            CreateMap<SysUser, UserSecurityVM>()
               .ForMember(x => x.PasswordLevel, a => a.MapFrom(x => x.PasswordLevel.GetDescription()))
               .ForMember(x => x.Phone, a => a.MapFrom(x => H_Util.HidePhoneNumber(x.Phone)))
               .ForMember(x => x.Email, a => a.MapFrom(x => H_Util.HideEmailNumber(x.Email)));

            CreateMap<UserQueryInput, UserQuery>()
               .ForMember(x => x.OrderFileds, a => a.MapFrom(x => x.OrderByType.CombineNameWithSpace(x.SortField)));

            CreateMap<UserUpdateRequest, SysUser>();
        }

        /// <summary>
        /// 模块
        /// </summary>
        private void MapModule()
        {
            #region 模块

            CreateMap<SysModule, ModuleDetailVM>()
               .ForMember(x => x.Code, a => a.MapFrom(x => string.Format("{0}_{1}", x.Layer, x.Number)));

            CreateMap<ModuleAddRequest, SysModule>();
            #endregion


            #region 资源
            CreateMap<ResourceAddRequest, SysModule>();

            CreateMap<SysModule, ResourceItemVM>()
               .ForMember(x => x.ResourceCode, a => a.MapFrom(x => string.Format("{0}_{1}", x.Layer, x.Number)));
            #endregion


        }

        /// <summary>
        /// 角色
        /// </summary>
        private void MapRole()
        {
            CreateMap<SysRole, RoleVM>();

            CreateMap<SysRole, RoleSelectVM>();
        }

        /// <summary>
        /// 字典
        /// </summary>
        private void MapDict()
        {
            CreateMap<DictAddRequest, SysDict>();

            CreateMap<DictItemAddRequest, SysDict>();

            CreateMap<PagedList<SysDict>, PagedList<DictVM>>();

            CreateMap<SysDict, DictVM>();

            CreateMap<PagedList<SysDict>, PagedList<DictItemVM>>();

            CreateMap<SysDict, DictItemVM>();

            CreateMap<SysDict, DictDataItemVM>();

            CreateMap<DictQueryInput, DictQuery>();

        }
    }
}
