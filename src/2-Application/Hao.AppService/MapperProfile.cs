using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Model;
using Hao.Utility;

namespace Hao.AppService
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            #region 用户
            CreateMap<SysUser, LoginVM>();

            CreateMap<PagedList<SysUser>, PagedList<UserItemVM>>();

            CreateMap<SysUser, UserItemVM>()
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
            #endregion


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


            #region 角色
            CreateMap<SysRole, RoleVM>();

            CreateMap<SysRole, RoleSelectVM>();
            #endregion


            #region 数据字典
            CreateMap<DictAddRequest, SysDict>();

            CreateMap<DictItemAddRequest, SysDict>();

            CreateMap<PagedList<SysDict>, PagedList<DictVM>>();
            
            CreateMap<SysDict, DictVM>();
            
            CreateMap<PagedList<SysDict>, PagedList<DictItemVM>>();
            
            CreateMap<SysDict, DictItemVM>();
            #endregion

        }
    }
}
