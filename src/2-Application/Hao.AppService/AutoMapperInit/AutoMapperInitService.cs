using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core.Model;
using Hao.Enum;
using Hao.Model;
using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    public class AutoMapperInitService
    {
        public static void InitMap(IMapperConfigurationExpression cfg)
        {

            cfg.CreateMap<SysUser, LoginOut>();


            cfg.CreateMap<PagedList<SysUser>, PagedList<UserOut>>();

            cfg.CreateMap<SysUser, UserOut>()
              .ForMember(x => x.GenderString, a => a.MapFrom(x => HDescription.GetDescription(x.Gender)))
              .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.Value ? "启用" : "注销"));
            
            cfg.CreateMap<UserIn, SysUser>();
        }
    }
}
