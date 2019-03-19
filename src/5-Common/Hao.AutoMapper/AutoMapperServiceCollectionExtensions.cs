using AutoMapper;
using Hao.AutoMapper;
using Hao.Core.Query;
using Hao.Core.QueryInput;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoMapperServiceCollectionExtensions
    {
        /// <summary>
        /// 添加AutoMapper至IOC中
        /// </summary>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configure)
        {
            var config = new MapperConfiguration(x =>
            {
                //默认类型通用转换
                x.CreateMap<long, DateTime>().ConvertUsing((s, d) =>
                {
                    return new DateTime(s);
                });
                x.CreateMap<long, DateTime?>().ConvertUsing((s, d) =>
                {
                    return s == long.MinValue ? (DateTime?)null : new DateTime(s);
                });
                x.CreateMap<DateTime, long>().ConvertUsing((s, d) =>
                {
                    return s.Ticks;
                });
                x.CreateMap<DateTime?, long>().ConvertUsing((s, d) =>
                {
                    return s == null ? long.MinValue : s.Value.Ticks;
                });
                x.CreateMap<string, string>().ConvertUsing((s, d) =>
                {
                    return s == null ? string.Empty : s;
                });
                //bool? 类型采用int32表示（1：true，0：false，-1：Null）
                x.CreateMap<bool?, int>().ConvertUsing((s, d) =>
                {
                    return s == null ? -1 : Convert.ToInt32(s.Value);
                });
                x.CreateMap<int, bool?>().ConvertUsing((s, d) =>
                {
                    return s == -1 ? (bool?)null : Convert.ToBoolean(s);
                });
                x.CreateMap<int?, int>().ConvertUsing((s, d) =>
                {
                    return s == null ? int.MinValue : s.Value;
                });
                x.CreateMap<int, int?>().ConvertUsing((s, d) =>
                {
                    return s == int.MinValue ? (int?)null : s;
                });
                x.CreateMap<long?, long>().ConvertUsing((s, d) =>
                {
                    return s == null ? long.MinValue : s.Value;
                });
                x.CreateMap<long, long?>().ConvertUsing((s, d) =>
                {
                    return s == long.MinValue ? (long?)null : s;
                });
                x.CreateMap<float?, float>().ConvertUsing((s, d) =>
                {
                    return s == null ? float.MinValue : s.Value;
                });
                x.CreateMap<float, float?>().ConvertUsing((s, d) =>
                {
                    return s == float.MinValue ? (float?)null : s;
                });
                x.CreateMap<long?, string>().ConvertUsing((s, d) =>
                { 
                    return s == null ? string.Empty : s.Value.ToString();
                });
                x.CreateMap<decimal, float>().ConvertUsing((s, d) =>
                {
                    return float.Parse(s.ToString());
                });
                x.CreateMap<decimal?, float>().ConvertUsing((s, d) =>
                {
                    return s == null ? float.MinValue : float.Parse(s.Value.ToString());
                });
                //x.CreateMap<Enum, int>().ConvertUsing((s, d) =>
                //{
                //    return s == null ? int.MinValue : (int)Enum.ToObject(s.GetType(), s);
                //});

                //x.CreateMap(typeof(IQueryInput), typeof(IQuery))
                //                .ForMember("Conditions", a => a.Ignore());

                //加载外部mapper配置
                configure(x);
            });
            services.AddSingleton<IAutoMapper>(new AutoMapperImp(config.CreateMapper()));
            return services;
        }
    }
}
