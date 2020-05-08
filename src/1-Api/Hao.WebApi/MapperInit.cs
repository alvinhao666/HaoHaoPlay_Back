using AutoMapper;
using Hao.AppService;
using Hao.Core;

namespace Hao.WebApi
{
    public class MapperInit
    {
        public static void Map(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserQueryInput, UserQuery>()
               .ForMember(x => x.OrderFileds, a => a.MapFrom(x => x.OrderByType.CombineNameWithSpace(x.SortField))); 
        }
    }
}
