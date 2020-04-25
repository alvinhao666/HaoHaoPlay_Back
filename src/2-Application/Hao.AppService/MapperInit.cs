using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Model;
using Hao.Utility;


namespace Hao.AppService
{
    public class MapperInit
    {
        public static void Map(IMapperConfigurationExpression cfg)
        {

            cfg.CreateMap<SysUser, LoginVM>();


            cfg.CreateMap<PagedList<SysUser>, PagedList<UserItemVM>>();

            cfg.CreateMap<SysUser, UserItemVM>()
              .ForMember(x => x.GenderString, a => a.MapFrom(x => x.Gender.GetDescription()))
              .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.Value ? "启用" : "注销"));

            cfg.CreateMap<SysUser, UserDetailVM>()
              .ForMember(x => x.GenderString, a => a.MapFrom(x => x.Gender.GetDescription()))
              .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.Value ? "启用" : "注销"));

            cfg.CreateMap<UserAddRequest, SysUser>();


            cfg.CreateMap<SysUser, CurrentUserVM>();

            cfg.CreateMap<SysUser, UserSecurityVM>()
                .ForMember(x => x.PasswordLevel, a => a.MapFrom(x => x.PasswordLevel.GetDescription()))
                .ForMember(x => x.Phone, a => a.MapFrom(x => HUtil.HidePhoneNumber(x.Phone)))
                .ForMember(x => x.Email, a => a.MapFrom(x => HUtil.HideEmailNumber(x.Email)));


            #region 模块

            cfg.CreateMap<SysModule, ModuleDetailVM>()
                .ForMember(x => x.Code, a => a.MapFrom(x => string.Format("{0}_{1}", x.Layer, x.Number)));

            cfg.CreateMap<ModuleAddRequest, SysModule>();
            #endregion

            #region 资源
            cfg.CreateMap<ResourceAddRequest, SysModule>();

            cfg.CreateMap<SysModule, ResourceItemVM>()
                .ForMember(x => x.ResourceCode, a => a.MapFrom(x => string.Format("{0}_{1}", x.Layer, x.Number)));
            #endregion


            #region 角色
            cfg.CreateMap<SysRole, RoleVM>();

            cfg.CreateMap<SysRole, RoleSelectVM>();
            #endregion

        }
    }
}
