using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core.Model;
using Hao.Model;
using Hao.Model.Enum;
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
            cfg.CreateMap<SYSUser, LoginVMOut>();


            cfg.CreateMap<PagedList<SYSUser>, PagedList<UserVMOut>>();

            cfg.CreateMap<SYSUser, UserVMOut>()
              .ForMember(x => x.GenderString, a => a.MapFrom(x => HDescription.GetDescription(typeof(Gender), (int)x.Gender.Value)))
              .ForMember(x => x.EnabledString, a => a.MapFrom(x => x.Enabled.Value ? "启用" : "注销"));


            cfg.CreateMap<UserVMIn, SYSUser>();
        }
    }
}
