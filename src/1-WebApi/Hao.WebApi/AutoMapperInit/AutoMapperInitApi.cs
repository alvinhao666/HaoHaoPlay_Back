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
            cfg.CreateMap<UserQueryInput, UserQuery>();
        }
    }
}
