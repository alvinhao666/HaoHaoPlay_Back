using FreeRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hao.Redis
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public static class RedisHelper
    {
        private static RedisClient _redisClient;

        public static RedisClient Cli => _redisClient;

        public static void AddClient(RedisClient redisClient)
        {
            _redisClient = redisClient;

            _redisClient.Serialize = obj => JsonConvert.SerializeObject(obj);
            _redisClient.Deserialize = (json, type) => JsonConvert.DeserializeObject(json, type);
        }
    }
}