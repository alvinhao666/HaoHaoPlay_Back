using AutoMapper;
using Hao.AppService;
using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class AutoMapperInitApi
    {
        public static void InitMap(IMapperConfigurationExpression cfg)
        {
            //string.Format方法在内部使用StringBuilder进行字符串的格式化
            cfg.CreateMap<UserQueryInput, UserQuery>()
                .ForMember(x => x.OrderFileds, a => a.MapFrom(x => x.OrderByType.CombineNameWithSpace(x.SortField))); 
        }
    }
}
