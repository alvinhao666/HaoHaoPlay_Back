using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Entity;
using Hao.Model;
using Hao.Utility;


namespace Hao.AppService
{
    public class MapperInit
    {
        public static void Map(IMapperConfigurationExpression cfg)
        {

            cfg.CreateMap<SysUser, LoginOut>();


            cfg.CreateMap<PagedList<SysUser>, PagedList<UserOut>>();

            cfg.CreateMap<SysUser, UserOut>()
              .ForMember(x => x.GenderString, a => a.MapFrom(x => x.Gender.GetDescription()))
              .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.Value ? "启用" : "注销"));
            
            cfg.CreateMap<UserIn, SysUser>();
            
            cfg.CreateMap<SysUser, CurrentUserOut>();
        }
    }
}
