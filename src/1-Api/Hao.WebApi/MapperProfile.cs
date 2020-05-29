using AutoMapper;
using Hao.AppService;
using Hao.Core;

namespace Hao.WebApi
{
    /// <summary>
    /// 模型映射
    /// </summary>
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<UserQueryInput, UserQuery>()
               .ForMember(x => x.OrderFileds, a => a.MapFrom(x => x.OrderByType.CombineNameWithSpace(x.SortField)));


            CreateMap<DictQueryInput, DictQuery>();
        }
    }
}
