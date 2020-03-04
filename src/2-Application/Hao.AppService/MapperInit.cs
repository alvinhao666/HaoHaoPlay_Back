using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Entity;
using Hao.Enum;
using Hao.Model;
using Hao.Utility;


namespace Hao.AppService
{
    public class MapperInit
    {
        public static void Map(IMapperConfigurationExpression cfg)
        {

            cfg.CreateMap<SysUser, LoginVM>();


            cfg.CreateMap<PagedList<SysUser>, PagedList<UserListItemVM>>();

            cfg.CreateMap<SysUser, UserListItemVM>()
              .ForMember(x => x.GenderString, a => a.MapFrom(x => x.Gender.GetDescription()))
              .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.Value ? "启用" : "注销"));

            cfg.CreateMap<SysUser, UserDetailVM>();

            cfg.CreateMap<UserAddRequest, SysUser>()
            .ForMember(x => x.PasswordLevel, a => a.MapFrom(x =>(PasswordLevel)HUtil.CheckPasswordLevel(x.Password)));


            cfg.CreateMap<SysUser, CurrentUserVM>();

            cfg.CreateMap<SysUser, UserSecurityVM>()
                .ForMember(x => x.PasswordLevel, a => a.MapFrom(x => x.PasswordLevel.GetDescription()))
                .ForMember(x => x.Phone, a => a.MapFrom(x => HUtil.HidePhoneNumber(x.Phone)))
                .ForMember(x => x.Email, a => a.MapFrom(x => HUtil.HideEmailNumber(x.Email)));
        }
    }
}
