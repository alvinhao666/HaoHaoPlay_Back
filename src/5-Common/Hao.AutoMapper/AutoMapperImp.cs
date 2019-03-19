using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AutoMapper
{
    /// <summary>
    /// IAutoMapper接口的实现
    /// </summary>
    public class AutoMapperImp: IAutoMapper
    {
        private IMapper _mapper;

        public AutoMapperImp(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}
