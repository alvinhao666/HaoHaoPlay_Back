using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.SqlSugarExtensions
{
    //ServiceStack.Redis是商业版，免费版有限制；

    //StackExchange.Redis是免费版，早期有Timeout Bug，当前版本使用需要全部使用异步方法方可解决；

    public class SqlSugarRedisCache : ICacheService
    {
        public void Add<V>(string key, V value)
        {
            RedisHelper.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            RedisHelper.Set(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<V>(string key)
        {
            return RedisHelper.Exists(key);
        }

        public V Get<V>(string key)
        {
            return RedisHelper.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return RedisHelper.Keys("SqlSugarDataCache.*");
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (this.ContainsKey<V>(cacheKey))
            {
                return this.Get<V>(cacheKey);
            }
            else
            {
                var result = create();
                this.Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<V>(string key)
        {
            RedisHelper.Expire(key,0);
        }
    }
}
