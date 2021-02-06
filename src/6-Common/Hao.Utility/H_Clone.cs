using Mapster;

namespace Hao.Utility
{
    /// <summary>
    /// 克隆
    /// </summary>
    public static class H_Clone
    {
        /// <summary>
        /// 深克隆
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeepClone<T>(this T source)
        {
            return source.Adapt<T>();
        }
    }
}