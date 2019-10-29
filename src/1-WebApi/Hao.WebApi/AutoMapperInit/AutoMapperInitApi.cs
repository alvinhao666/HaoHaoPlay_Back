using AutoMapper;
using Hao.AppService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class AutoMapperInitApi
    {
        public static void InitMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserQueryInput, UserQuery>()
                .ForMember(x => x.OrderFileds, a => a.MapFrom(x => x.OrderByType.HasValue && x.SortField.HasValue ? (x.SortField + " " + x.OrderByType) : null));
        }
    }
}
