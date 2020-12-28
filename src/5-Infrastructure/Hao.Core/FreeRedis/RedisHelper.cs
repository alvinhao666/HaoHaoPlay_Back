using FreeRedis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Hao.Core
{
    public static class RedisHelper
    {
        private static RedisClient _redisClient;

        public static void AddClient(RedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public static void CreateInstance(ConnectionStringBuilder sentinelConnectionString, string[] sentinels, bool rw_splitting)
        {
            _redisClient = new RedisClient(sentinelConnectionString, sentinels, rw_splitting);
        }

        public static void CreateInstance(ConnectionStringBuilder[] connectionStrings, Func<string, string> redirectRule)
        {
            _redisClient = new RedisClient(connectionStrings, redirectRule);
        }

        public static void CreateInstance(ConnectionStringBuilder[] clusterConnectionStrings)
        {
            _redisClient = new RedisClient(clusterConnectionStrings);
        }

        public static void CreateInstance(ConnectionStringBuilder connectionString, params ConnectionStringBuilder[] slaveConnectionStrings)
        {
            _redisClient = new RedisClient(connectionString, slaveConnectionStrings);
        }


        public static async Task SetExAsync<T>(String key, Int32 seconds, T value)
        {
            await _redisClient.SetExAsync<T>(key, seconds, value);
        }

        public static async Task<Boolean> SetNxAsync<T>(String key, T value)
        {
            var result = await _redisClient.SetNxAsync<T>(key, value);
            return result;
        }

        public static async Task<Int64> SetRangeAsync<T>(String key, Int64 offset, T value)
        {
            var result = await _redisClient.SetRangeAsync<T>(key, offset, value);
            return result;
        }

        public static async Task<Int64> StrLenAsync(String key)
        {
            var result = await _redisClient.StrLenAsync(key);
            return result;
        }

        public static Int64 Append<T>(String key, T value)
        {
            var result = _redisClient.Append<T>(key, value);
            return result;
        }

        public static Int64 BitCount(String key, Int64 start, Int64 end)
        {
            var result = _redisClient.BitCount(key, start, end);
            return result;
        }

        public static Int64 BitOp(BitOpOperation operation, String destkey, String[] keys)
        {
            var result = _redisClient.BitOp(operation, destkey, keys);
            return result;
        }

        public static Int64 BitPos(String key, Boolean bit, Nullable<Int64> start, Nullable<Int64> end)
        {
            var result = _redisClient.BitPos(key, bit, start, end);
            return result;
        }

        public static Int64 Decr(String key)
        {
            var result = _redisClient.Decr(key);
            return result;
        }

        public static Int64 DecrBy(String key, Int64 decrement)
        {
            var result = _redisClient.DecrBy(key, decrement);
            return result;
        }

        public static String Get(String key)
        {
            var result = _redisClient.Get(key);
            return result;
        }

        public static T Get<T>(String key)
        {
            var result = _redisClient.Get<T>(key);
            return result;
        }

        public static void Get(String key, Stream destination, Int32 bufferSize)
        {
            _redisClient.Get(key, destination, bufferSize);
        }

        public static Boolean GetBit(String key, Int64 offset)
        {
            var result = _redisClient.GetBit(key, offset);
            return result;
        }

        public static String GetRange(String key, Int64 start, Int64 end)
        {
            var result = _redisClient.GetRange(key, start, end);
            return result;
        }

        public static T GetRange<T>(String key, Int64 start, Int64 end)
        {
            var result = _redisClient.GetRange<T>(key, start, end);
            return result;
        }

        public static String GetSet<T>(String key, T value)
        {
            var result = _redisClient.GetSet<T>(key, value);
            return result;
        }

        public static Int64 Incr(String key)
        {
            var result = _redisClient.Incr(key);
            return result;
        }

        public static Int64 IncrBy(String key, Int64 increment)
        {
            var result = _redisClient.IncrBy(key, increment);
            return result;
        }

        public static Decimal IncrByFloat(String key, Decimal increment)
        {
            var result = _redisClient.IncrByFloat(key, increment);
            return result;
        }

        public static String[] MGet(params string[] keys)
        {
            var result = _redisClient.MGet(keys);
            return result;
        }

        public static T[] MGet<T>(params string[] keys)
        {
            var result = _redisClient.MGet<T>(keys);
            return result;
        }

        public static void MSet(String key, Object value, Object[] keyValues)
        {
            _redisClient.MSet(key, value, keyValues);
        }

        public static void MSet<T>(Dictionary<String, T> keyValues)
        {
            _redisClient.MSet<T>(keyValues);
        }

        public static Boolean MSetNx(String key, Object value, Object[] keyValues)
        {
            var result = _redisClient.MSetNx(key, value, keyValues);
            return result;
        }

        public static Boolean MSetNx<T>(Dictionary<String, T> keyValues)
        {
            var result = _redisClient.MSetNx<T>(keyValues);
            return result;
        }

        public static void PSetEx<T>(String key, Int64 milliseconds, T value)
        {
            _redisClient.PSetEx<T>(key, milliseconds, value);
        }

        public static void Set<T>(String key, T value, Int32 timeoutSeconds)
        {
            _redisClient.Set<T>(key, value, timeoutSeconds);
        }

        public static void Set<T>(String key, T value, Boolean keepTtl)
        {
            _redisClient.Set<T>(key, value, keepTtl);
        }

        public static Boolean SetNx<T>(String key, T value, Int32 timeoutSeconds)
        {
            var result = _redisClient.SetNx<T>(key, value, timeoutSeconds);
            return result;
        }

        public static Boolean SetXx<T>(String key, T value, Int32 timeoutSeconds)
        {
            var result = _redisClient.SetXx<T>(key, value, timeoutSeconds);
            return result;
        }

        public static Boolean SetXx<T>(String key, T value, Boolean keepTtl)
        {
            var result = _redisClient.SetXx<T>(key, value, keepTtl);
            return result;
        }

        public static String Set<T>(String key, T value, TimeSpan timeout, Boolean keepTtl, Boolean nx, Boolean xx, Boolean get)
        {
            var result = _redisClient.Set<T>(key, value, timeout, keepTtl, nx, xx, get);
            return result;
        }

        public static Int64 SetBit(String key, Int64 offset, Boolean value)
        {
            var result = _redisClient.SetBit(key, offset, value);
            return result;
        }

        public static void SetEx<T>(String key, Int32 seconds, T value)
        {
            _redisClient.SetEx<T>(key, seconds, value);
        }

        public static Boolean SetNx<T>(String key, T value)
        {
            var result = _redisClient.SetNx<T>(key, value);
            return result;
        }

        public static Int64 SetRange<T>(String key, Int64 offset, T value)
        {
            var result = _redisClient.SetRange<T>(key, offset, value);
            return result;
        }

        public static Int64 StrLen(String key)
        {
            var result = _redisClient.StrLen(key);
            return result;
        }

        public static async Task<StreamsXPendingResult> XPendingAsync(String key, String group)
        {
            var result = await _redisClient.XPendingAsync(key, group);
            return result;
        }

        public static async Task<StreamsXPendingConsumerResult[]> XPendingAsync(String key, String group, String start, String end, Int64 count, String consumer)
        {
            var result = await _redisClient.XPendingAsync(key, group, start, end, count, consumer);
            return result;
        }

        public static async Task<StreamsEntry[]> XRangeAsync(String key, String start, String end, Int64 count)
        {
            var result = await _redisClient.XRangeAsync(key, start, end, count);
            return result;
        }

        public static async Task<StreamsEntry[]> XRevRangeAsync(String key, String end, String start, Int64 count)
        {
            var result = await _redisClient.XRevRangeAsync(key, end, start, count);
            return result;
        }

        public static async Task<StreamsEntry> XReadAsync(Int64 block, String key, String id)
        {
            var result = await _redisClient.XReadAsync(block, key, id);
            return result;
        }

        public static async Task<StreamsEntryResult[]> XReadAsync(Int64 count, Int64 block, String key, String id, String[] keyIds)
        {
            var result = await _redisClient.XReadAsync(count, block, key, id, keyIds);
            return result;
        }

        public static async Task<StreamsEntryResult[]> XReadAsync(Int64 count, Int64 block, Dictionary<String, String> keyIds)
        {
            var result = await _redisClient.XReadAsync(count, block, keyIds);
            return result;
        }

        public static async Task<StreamsEntry> XReadGroupAsync(String group, String consumer, Int64 block, String key, String id)
        {
            var result = await _redisClient.XReadGroupAsync(group, consumer, block, key, id);
            return result;
        }

        public static async Task<StreamsEntryResult[]> XReadGroupAsync(String group, String consumer, Int64 count, Int64 block, Boolean noack, String key, String id, String[] keyIds)
        {
            var result = await _redisClient.XReadGroupAsync(group, consumer, count, block, noack, key, id, keyIds);
            return result;
        }

        public static async Task<StreamsEntryResult[]> XReadGroupAsync(String group, String consumer, Int64 count, Int64 block, Boolean noack, Dictionary<String, String> keyIds)
        {
            var result = await _redisClient.XReadGroupAsync(group, consumer, count, block, noack, keyIds);
            return result;
        }

        public static async Task<Int64> XTrimAsync(String key, Int64 count)
        {
            var result = await _redisClient.XTrimAsync(key, count);
            return result;
        }

        public static Int64 XAck(String key, String group, String[] id)
        {
            var result = _redisClient.XAck(key, group, id);
            return result;
        }

        public static String XAdd<T>(String key, String field, T value, Object[] fieldValues)
        {
            var result = _redisClient.XAdd<T>(key, field, value, fieldValues);
            return result;
        }

        public static String XAdd<T>(String key, Int64 maxlen, String id, String field, T value, Object[] fieldValues)
        {
            var result = _redisClient.XAdd<T>(key, maxlen, id, field, value, fieldValues);
            return result;
        }

        public static String XAdd<T>(String key, Dictionary<String, T> fieldValues)
        {
            var result = _redisClient.XAdd<T>(key, fieldValues);
            return result;
        }

        public static String XAdd<T>(String key, Int64 maxlen, String id, Dictionary<String, T> fieldValues)
        {
            var result = _redisClient.XAdd<T>(key, maxlen, id, fieldValues);
            return result;
        }

        public static StreamsEntry[] XClaim(String key, String group, String consumer, Int64 minIdleTime, String[] id)
        {
            var result = _redisClient.XClaim(key, group, consumer, minIdleTime, id);
            return result;
        }

        public static StreamsEntry[] XClaim(String key, String group, String consumer, Int64 minIdleTime, String[] id, Int64 idle, Int64 retryCount, Boolean force)
        {
            var result = _redisClient.XClaim(key, group, consumer, minIdleTime, id, idle, retryCount, force);
            return result;
        }

        public static String[] XClaimJustId(String key, String group, String consumer, Int64 minIdleTime, String[] id)
        {
            var result = _redisClient.XClaimJustId(key, group, consumer, minIdleTime, id);
            return result;
        }

        public static String[] XClaimJustId(String key, String group, String consumer, Int64 minIdleTime, String[] id, Int64 idle, Int64 retryCount, Boolean force)
        {
            var result = _redisClient.XClaimJustId(key, group, consumer, minIdleTime, id, idle, retryCount, force);
            return result;
        }

        public static Int64 XDel(String key, String[] id)
        {
            var result = _redisClient.XDel(key, id);
            return result;
        }

        public static void XGroupCreate(String key, String group, String id, Boolean MkStream)
        {
            _redisClient.XGroupCreate(key, group, id, MkStream);
        }

        public static void XGroupSetId(String key, String group, String id)
        {
            _redisClient.XGroupSetId(key, group, id);
        }

        public static Boolean XGroupDestroy(String key, String group)
        {
            var result = _redisClient.XGroupDestroy(key, group);
            return result;
        }

        public static void XGroupCreateConsumer(String key, String group, String consumer)
        {
            _redisClient.XGroupCreateConsumer(key, group, consumer);
        }

        public static Int64 XGroupDelConsumer(String key, String group, String consumer)
        {
            var result = _redisClient.XGroupDelConsumer(key, group, consumer);
            return result;
        }

        public static StreamsXInfoStreamResult XInfoStream(String key)
        {
            var result = _redisClient.XInfoStream(key);
            return result;
        }

        public static StreamsXInfoStreamFullResult XInfoStreamFull(String key, Int64 count)
        {
            var result = _redisClient.XInfoStreamFull(key, count);
            return result;
        }

        public static StreamsXInfoGroupsResult[] XInfoGroups(String key)
        {
            var result = _redisClient.XInfoGroups(key);
            return result;
        }

        public static StreamsXInfoConsumersResult[] XInfoConsumers(String key, String group)
        {
            var result = _redisClient.XInfoConsumers(key, group);
            return result;
        }

        public static Int64 XLen(String key)
        {
            var result = _redisClient.XLen(key);
            return result;
        }

        public static StreamsXPendingResult XPending(String key, String group)
        {
            var result = _redisClient.XPending(key, group);
            return result;
        }

        public static StreamsXPendingConsumerResult[] XPending(String key, String group, String start, String end, Int64 count, String consumer)
        {
            var result = _redisClient.XPending(key, group, start, end, count, consumer);
            return result;
        }

        public static StreamsEntry[] XRange(String key, String start, String end, Int64 count)
        {
            var result = _redisClient.XRange(key, start, end, count);
            return result;
        }

        public static StreamsEntry[] XRevRange(String key, String end, String start, Int64 count)
        {
            var result = _redisClient.XRevRange(key, end, start, count);
            return result;
        }

        public static StreamsEntry XRead(Int64 block, String key, String id)
        {
            var result = _redisClient.XRead(block, key, id);
            return result;
        }

        public static StreamsEntryResult[] XRead(Int64 count, Int64 block, String key, String id, String[] keyIds)
        {
            var result = _redisClient.XRead(count, block, key, id, keyIds);
            return result;
        }

        public static StreamsEntryResult[] XRead(Int64 count, Int64 block, Dictionary<String, String> keyIds)
        {
            var result = _redisClient.XRead(count, block, keyIds);
            return result;
        }

        public static StreamsEntry XReadGroup(String group, String consumer, Int64 block, String key, String id)
        {
            var result = _redisClient.XReadGroup(group, consumer, block, key, id);
            return result;
        }

        public static StreamsEntryResult[] XReadGroup(String group, String consumer, Int64 count, Int64 block, Boolean noack, String key, String id, String[] keyIds)
        {
            var result = _redisClient.XReadGroup(group, consumer, count, block, noack, key, id, keyIds);
            return result;
        }

        public static StreamsEntryResult[] XReadGroup(String group, String consumer, Int64 count, Int64 block, Boolean noack, Dictionary<String, String> keyIds)
        {
            var result = _redisClient.XReadGroup(group, consumer, count, block, noack, keyIds);
            return result;
        }

        public static Int64 XTrim(String key, Int64 count)
        {
            var result = _redisClient.XTrim(key, count);
            return result;
        }

        public static async Task<Int64> AppendAsync<T>(String key, T value)
        {
            var result = await _redisClient.AppendAsync<T>(key, value);
            return result;
        }

        public static async Task<Int64> BitCountAsync(String key, Int64 start, Int64 end)
        {
            var result = await _redisClient.BitCountAsync(key, start, end);
            return result;
        }

        public static async Task<Int64> BitOpAsync(BitOpOperation operation, String destkey, String[] keys)
        {
            var result = await _redisClient.BitOpAsync(operation, destkey, keys);
            return result;
        }

        public static async Task<Int64> BitPosAsync(String key, Boolean bit, Nullable<Int64> start, Nullable<Int64> end)
        {
            var result = await _redisClient.BitPosAsync(key, bit, start, end);
            return result;
        }

        public static async Task<Int64> DecrAsync(String key)
        {
            var result = await _redisClient.DecrAsync(key);
            return result;
        }

        public static async Task<Int64> DecrByAsync(String key, Int64 decrement)
        {
            var result = await _redisClient.DecrByAsync(key, decrement);
            return result;
        }

        public static async Task<String> GetAsync(String key)
        {
            var result = await _redisClient.GetAsync(key);
            return result;
        }

        public static async Task<T> GetAsync<T>(String key)
        {
            var result = await _redisClient.GetAsync<T>(key);
            return result;
        }

        public static async Task<Boolean> GetBitAsync(String key, Int64 offset)
        {
            var result = await _redisClient.GetBitAsync(key, offset);
            return result;
        }

        public static async Task<String> GetRangeAsync(String key, Int64 start, Int64 end)
        {
            var result = await _redisClient.GetRangeAsync(key, start, end);
            return result;
        }

        public static async Task<T> GetRangeAsync<T>(String key, Int64 start, Int64 end)
        {
            var result = await _redisClient.GetRangeAsync<T>(key, start, end);
            return result;
        }

        public static async Task<String> GetSetAsync<T>(String key, T value)
        {
            var result = await _redisClient.GetSetAsync<T>(key, value);
            return result;
        }

        public static async Task<Int64> IncrAsync(String key)
        {
            var result = await _redisClient.IncrAsync(key);
            return result;
        }

        public static async Task<Int64> IncrByAsync(String key, Int64 increment)
        {
            var result = await _redisClient.IncrByAsync(key, increment);
            return result;
        }

        public static async Task<Decimal> IncrByFloatAsync(String key, Decimal increment)
        {
            var result = await _redisClient.IncrByFloatAsync(key, increment);
            return result;
        }

        public static async Task<String[]> MGetAsync(params string[] keys)
        {
            var result = await _redisClient.MGetAsync(keys);
            return result;
        }

        public static async Task<T[]> MGetAsync<T>(params string[] keys)
        {
            var result = await _redisClient.MGetAsync<T>(keys);
            return result;
        }

        public static async Task MSetAsync(String key, Object value, Object[] keyValues)
        {
            await _redisClient.MSetAsync(key, value, keyValues);
        }

        public static async Task MSetAsync<T>(Dictionary<String, T> keyValues)
        {
            await _redisClient.MSetAsync<T>(keyValues);
        }

        public static async Task<Boolean> MSetNxAsync(String key, Object value, Object[] keyValues)
        {
            var result = await _redisClient.MSetNxAsync(key, value, keyValues);
            return result;
        }

        public static async Task<Boolean> MSetNxAsync<T>(Dictionary<String, T> keyValues)
        {
            var result = await _redisClient.MSetNxAsync<T>(keyValues);
            return result;
        }

        public static async Task PSetExAsync<T>(String key, Int64 milliseconds, T value)
        {
            await _redisClient.PSetExAsync<T>(key, milliseconds, value);
        }

        public static async Task SetAsync<T>(String key, T value, Int32 timeoutSeconds)
        {
            await _redisClient.SetAsync<T>(key, value, timeoutSeconds);
        }

        public static async Task SetAsync<T>(String key, T value, Boolean keepTtl)
        {
            await _redisClient.SetAsync<T>(key, value, keepTtl);
        }

        public static async Task<Boolean> SetNxAsync<T>(String key, T value, Int32 timeoutSeconds)
        {
            var result = await _redisClient.SetNxAsync<T>(key, value, timeoutSeconds);
            return result;
        }

        public static async Task<Boolean> SetXxAsync<T>(String key, T value, Int32 timeoutSeconds)
        {
            var result = await _redisClient.SetXxAsync<T>(key, value, timeoutSeconds);
            return result;
        }

        public static async Task<Boolean> SetXxAsync<T>(String key, T value, Boolean keepTtl)
        {
            var result = await _redisClient.SetXxAsync<T>(key, value, keepTtl);
            return result;
        }

        public static async Task<String> SetAsync<T>(String key, T value, TimeSpan timeout, Boolean keepTtl, Boolean nx, Boolean xx, Boolean get)
        {
            var result = await _redisClient.SetAsync<T>(key, value, timeout, keepTtl, nx, xx, get);
            return result;
        }

        public static async Task<Int64> SetBitAsync(String key, Int64 offset, Boolean value)
        {
            var result = await _redisClient.SetBitAsync(key, offset, value);
            return result;
        }

        public static async Task<ZMember[]> ZRevRangeWithScoresAsync(String key, Decimal start, Decimal stop)
        {
            var result = await _redisClient.ZRevRangeWithScoresAsync(key, start, stop);
            return result;
        }

        public static async Task<String[]> ZRevRangeByLexAsync(String key, Decimal max, Decimal min, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRevRangeByLexAsync(key, max, min, offset, count);
            return result;
        }

        public static async Task<String[]> ZRevRangeByLexAsync(String key, String max, String min, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRevRangeByLexAsync(key, max, min, offset, count);
            return result;
        }

        public static async Task<String[]> ZRevRangeByScoreAsync(String key, Decimal max, Decimal min, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRevRangeByScoreAsync(key, max, min, offset, count);
            return result;
        }

        public static async Task<String[]> ZRevRangeByScoreAsync(String key, String max, String min, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRevRangeByScoreAsync(key, max, min, offset, count);
            return result;
        }

        public static async Task<ZMember[]> ZRevRangeByScoreWithScoresAsync(String key, Decimal max, Decimal min, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRevRangeByScoreWithScoresAsync(key, max, min, offset, count);
            return result;
        }

        public static async Task<ZMember[]> ZRevRangeByScoreWithScoresAsync(String key, String max, String min, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRevRangeByScoreWithScoresAsync(key, max, min, offset, count);
            return result;
        }

        public static async Task<Int64> ZRevRankAsync(String key, String member)
        {
            var result = await _redisClient.ZRevRankAsync(key, member);
            return result;
        }

        public static async Task<Decimal> ZScoreAsync(String key, String member)
        {
            var result = await _redisClient.ZScoreAsync(key, member);
            return result;
        }

        public static async Task<Int64> ZUnionStoreAsync(String destination, String[] keys, Int32[] weights, Nullable<ZAggregate> aggregate)
        {
            var result = await _redisClient.ZUnionStoreAsync(destination, keys, weights, aggregate);
            return result;
        }

        public static ZMember BZPopMin(String key, Int32 timeoutSeconds)
        {
            var result = _redisClient.BZPopMin(key, timeoutSeconds);
            return result;
        }

        public static KeyValue<ZMember> BZPopMin(String[] keys, Int32 timeoutSeconds)
        {
            var result = _redisClient.BZPopMin(keys, timeoutSeconds);
            return result;
        }

        public static ZMember BZPopMax(String key, Int32 timeoutSeconds)
        {
            var result = _redisClient.BZPopMax(key, timeoutSeconds);
            return result;
        }

        public static KeyValue<ZMember> BZPopMax(String[] keys, Int32 timeoutSeconds)
        {
            var result = _redisClient.BZPopMax(keys, timeoutSeconds);
            return result;
        }

        public static Int64 ZAdd(String key, Decimal score, String member, Object[] scoreMembers)
        {
            var result = _redisClient.ZAdd(key, score, member, scoreMembers);
            return result;
        }

        public static Int64 ZAdd(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = _redisClient.ZAdd(key, scoreMembers, than, ch);
            return result;
        }

        public static Int64 ZAddNx(String key, Decimal score, String member, Object[] scoreMembers)
        {
            var result = _redisClient.ZAddNx(key, score, member, scoreMembers);
            return result;
        }

        public static Int64 ZAddNx(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = _redisClient.ZAddNx(key, scoreMembers, than, ch);
            return result;
        }

        public static Int64 ZAddXx(String key, Decimal score, String member, Object[] scoreMembers)
        {
            var result = _redisClient.ZAddXx(key, score, member, scoreMembers);
            return result;
        }

        public static Int64 ZAddXx(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = _redisClient.ZAddXx(key, scoreMembers, than, ch);
            return result;
        }

        public static Int64 ZCard(String key)
        {
            var result = _redisClient.ZCard(key);
            return result;
        }

        public static Int64 ZCount(String key, Decimal min, Decimal max)
        {
            var result = _redisClient.ZCount(key, min, max);
            return result;
        }

        public static Int64 ZCount(String key, String min, String max)
        {
            var result = _redisClient.ZCount(key, min, max);
            return result;
        }

        public static Decimal ZIncrBy(String key, Decimal increment, String member)
        {
            var result = _redisClient.ZIncrBy(key, increment, member);
            return result;
        }

        public static Int64 ZInterStore(String destination, String[] keys, Int32[] weights, Nullable<ZAggregate> aggregate)
        {
            var result = _redisClient.ZInterStore(destination, keys, weights, aggregate);
            return result;
        }

        public static Int64 ZLexCount(String key, String min, String max)
        {
            var result = _redisClient.ZLexCount(key, min, max);
            return result;
        }

        public static ZMember ZPopMin(String key)
        {
            var result = _redisClient.ZPopMin(key);
            return result;
        }

        public static ZMember[] ZPopMin(String key, Int32 count)
        {
            var result = _redisClient.ZPopMin(key, count);
            return result;
        }

        public static ZMember ZPopMax(String key)
        {
            var result = _redisClient.ZPopMax(key);
            return result;
        }

        public static ZMember[] ZPopMax(String key, Int32 count)
        {
            var result = _redisClient.ZPopMax(key, count);
            return result;
        }

        public static String[] ZRange(String key, Decimal start, Decimal stop)
        {
            var result = _redisClient.ZRange(key, start, stop);
            return result;
        }

        public static ZMember[] ZRangeWithScores(String key, Decimal start, Decimal stop)
        {
            var result = _redisClient.ZRangeWithScores(key, start, stop);
            return result;
        }

        public static String[] ZRangeByLex(String key, String min, String max, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRangeByLex(key, min, max, offset, count);
            return result;
        }

        public static String[] ZRangeByScore(String key, Decimal min, Decimal max, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRangeByScore(key, min, max, offset, count);
            return result;
        }

        public static String[] ZRangeByScore(String key, String min, String max, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRangeByScore(key, min, max, offset, count);
            return result;
        }

        public static ZMember[] ZRangeByScoreWithScores(String key, Decimal min, Decimal max, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRangeByScoreWithScores(key, min, max, offset, count);
            return result;
        }

        public static ZMember[] ZRangeByScoreWithScores(String key, String min, String max, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRangeByScoreWithScores(key, min, max, offset, count);
            return result;
        }

        public static Int64 ZRank(String key, String member)
        {
            var result = _redisClient.ZRank(key, member);
            return result;
        }

        public static Int64 ZRem(String key, String[] members)
        {
            var result = _redisClient.ZRem(key, members);
            return result;
        }

        public static Int64 ZRemRangeByLex(String key, String min, String max)
        {
            var result = _redisClient.ZRemRangeByLex(key, min, max);
            return result;
        }

        public static Int64 ZRemRangeByRank(String key, Int64 start, Int64 stop)
        {
            var result = _redisClient.ZRemRangeByRank(key, start, stop);
            return result;
        }

        public static Int64 ZRemRangeByScore(String key, Decimal min, Decimal max)
        {
            var result = _redisClient.ZRemRangeByScore(key, min, max);
            return result;
        }

        public static Int64 ZRemRangeByScore(String key, String min, String max)
        {
            var result = _redisClient.ZRemRangeByScore(key, min, max);
            return result;
        }

        public static String[] ZRevRange(String key, Decimal start, Decimal stop)
        {
            var result = _redisClient.ZRevRange(key, start, stop);
            return result;
        }

        public static ZMember[] ZRevRangeWithScores(String key, Decimal start, Decimal stop)
        {
            var result = _redisClient.ZRevRangeWithScores(key, start, stop);
            return result;
        }

        public static String[] ZRevRangeByLex(String key, Decimal max, Decimal min, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRevRangeByLex(key, max, min, offset, count);
            return result;
        }

        public static String[] ZRevRangeByLex(String key, String max, String min, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRevRangeByLex(key, max, min, offset, count);
            return result;
        }

        public static String[] ZRevRangeByScore(String key, Decimal max, Decimal min, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRevRangeByScore(key, max, min, offset, count);
            return result;
        }

        public static String[] ZRevRangeByScore(String key, String max, String min, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRevRangeByScore(key, max, min, offset, count);
            return result;
        }

        public static ZMember[] ZRevRangeByScoreWithScores(String key, Decimal max, Decimal min, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRevRangeByScoreWithScores(key, max, min, offset, count);
            return result;
        }

        public static ZMember[] ZRevRangeByScoreWithScores(String key, String max, String min, Int32 offset, Int32 count)
        {
            var result = _redisClient.ZRevRangeByScoreWithScores(key, max, min, offset, count);
            return result;
        }

        public static Int64 ZRevRank(String key, String member)
        {
            var result = _redisClient.ZRevRank(key, member);
            return result;
        }

        public static Decimal ZScore(String key, String member)
        {
            var result = _redisClient.ZScore(key, member);
            return result;
        }

        public static Int64 ZUnionStore(String destination, String[] keys, Int32[] weights, Nullable<ZAggregate> aggregate)
        {
            var result = _redisClient.ZUnionStore(destination, keys, weights, aggregate);
            return result;
        }

        public static async Task<Int64> XAckAsync(String key, String group, String[] id)
        {
            var result = await _redisClient.XAckAsync(key, group, id);
            return result;
        }

        public static async Task<String> XAddAsync<T>(String key, String field, T value, Object[] fieldValues)
        {
            var result = await _redisClient.XAddAsync<T>(key, field, value, fieldValues);
            return result;
        }

        public static async Task<String> XAddAsync<T>(String key, Int64 maxlen, String id, String field, T value, Object[] fieldValues)
        {
            var result = await _redisClient.XAddAsync<T>(key, maxlen, id, field, value, fieldValues);
            return result;
        }

        public static async Task<String> XAddAsync<T>(String key, Dictionary<String, T> fieldValues)
        {
            var result = await _redisClient.XAddAsync<T>(key, fieldValues);
            return result;
        }

        public static async Task<String> XAddAsync<T>(String key, Int64 maxlen, String id, Dictionary<String, T> fieldValues)
        {
            var result = await _redisClient.XAddAsync<T>(key, maxlen, id, fieldValues);
            return result;
        }

        public static async Task<StreamsEntry[]> XClaimAsync(String key, String group, String consumer, Int64 minIdleTime, String[] id)
        {
            var result = await _redisClient.XClaimAsync(key, group, consumer, minIdleTime, id);
            return result;
        }

        public static async Task<StreamsEntry[]> XClaimAsync(String key, String group, String consumer, Int64 minIdleTime, String[] id, Int64 idle, Int64 retryCount, Boolean force)
        {
            var result = await _redisClient.XClaimAsync(key, group, consumer, minIdleTime, id, idle, retryCount, force);
            return result;
        }

        public static async Task<String[]> XClaimJustIdAsync(String key, String group, String consumer, Int64 minIdleTime, String[] id)
        {
            var result = await _redisClient.XClaimJustIdAsync(key, group, consumer, minIdleTime, id);
            return result;
        }

        public static async Task<String[]> XClaimJustIdAsync(String key, String group, String consumer, Int64 minIdleTime, String[] id, Int64 idle, Int64 retryCount, Boolean force)
        {
            var result = await _redisClient.XClaimJustIdAsync(key, group, consumer, minIdleTime, id, idle, retryCount, force);
            return result;
        }

        public static async Task<Int64> XDelAsync(String key, String[] id)
        {
            var result = await _redisClient.XDelAsync(key, id);
            return result;
        }

        public static async Task XGroupCreateAsync(String key, String group, String id, Boolean MkStream)
        {
            await _redisClient.XGroupCreateAsync(key, group, id, MkStream);
        }

        public static async Task XGroupSetIdAsync(String key, String group, String id)
        {
            await _redisClient.XGroupSetIdAsync(key, group, id);
        }

        public static async Task<Boolean> XGroupDestroyAsync(String key, String group)
        {
            var result = await _redisClient.XGroupDestroyAsync(key, group);
            return result;
        }

        public static async Task XGroupCreateConsumerAsync(String key, String group, String consumer)
        {
            await _redisClient.XGroupCreateConsumerAsync(key, group, consumer);
        }

        public static async Task<Int64> XGroupDelConsumerAsync(String key, String group, String consumer)
        {
            var result = await _redisClient.XGroupDelConsumerAsync(key, group, consumer);
            return result;
        }

        public static async Task<StreamsXInfoStreamResult> XInfoStreamAsync(String key)
        {
            var result = await _redisClient.XInfoStreamAsync(key);
            return result;
        }

        public static async Task<StreamsXInfoStreamFullResult> XInfoStreamFullAsync(String key, Int64 count)
        {
            var result = await _redisClient.XInfoStreamFullAsync(key, count);
            return result;
        }

        public static async Task<StreamsXInfoGroupsResult[]> XInfoGroupsAsync(String key)
        {
            var result = await _redisClient.XInfoGroupsAsync(key);
            return result;
        }

        public static async Task<StreamsXInfoConsumersResult[]> XInfoConsumersAsync(String key, String group)
        {
            var result = await _redisClient.XInfoConsumersAsync(key, group);
            return result;
        }

        public static async Task<Int64> XLenAsync(String key)
        {
            var result = await _redisClient.XLenAsync(key);
            return result;
        }

        public static async Task<String[]> SRandMemberAsync(String key, Int32 count)
        {
            var result = await _redisClient.SRandMemberAsync(key, count);
            return result;
        }

        public static async Task<T[]> SRandMemberAsync<T>(String key, Int32 count)
        {
            var result = await _redisClient.SRandMemberAsync<T>(key, count);
            return result;
        }

        public static async Task<Int64> SRemAsync(String key, Object[] members)
        {
            var result = await _redisClient.SRemAsync(key, members);
            return result;
        }

        public static async Task<ScanResult<String>> SScanAsync(String key, Int64 cursor, String pattern, Int64 count)
        {
            var result = await _redisClient.SScanAsync(key, cursor, pattern, count);
            return result;
        }

        public static async Task<String[]> SUnionAsync(params string[] keys)
        {
            var result = await _redisClient.SUnionAsync(keys);
            return result;
        }

        public static async Task<T[]> SUnionAsync<T>(params string[] keys)
        {
            var result = await _redisClient.SUnionAsync<T>(keys);
            return result;
        }

        public static async Task<Int64> SUnionStoreAsync(String destination, String[] keys)
        {
            var result = await _redisClient.SUnionStoreAsync(destination, keys);
            return result;
        }

        public static Int64 SAdd(String key, Object[] members)
        {
            var result = _redisClient.SAdd(key, members);
            return result;
        }

        public static Int64 SCard(String key)
        {
            var result = _redisClient.SCard(key);
            return result;
        }

        public static String[] SDiff(params string[] keys)
        {
            var result = _redisClient.SDiff(keys);
            return result;
        }

        public static T[] SDiff<T>(params string[] keys)
        {
            var result = _redisClient.SDiff<T>(keys);
            return result;
        }

        public static Int64 SDiffStore(String destination, String[] keys)
        {
            var result = _redisClient.SDiffStore(destination, keys);
            return result;
        }

        public static String[] SInter(params string[] keys)
        {
            var result = _redisClient.SInter(keys);
            return result;
        }

        public static T[] SInter<T>(params string[] keys)
        {
            var result = _redisClient.SInter<T>(keys);
            return result;
        }

        public static Int64 SInterStore(String destination, String[] keys)
        {
            var result = _redisClient.SInterStore(destination, keys);
            return result;
        }

        public static Boolean SIsMember<T>(String key, T member)
        {
            var result = _redisClient.SIsMember<T>(key, member);
            return result;
        }

        public static String[] SMembers(String key)
        {
            var result = _redisClient.SMembers(key);
            return result;
        }

        public static T[] SMembers<T>(String key)
        {
            var result = _redisClient.SMembers<T>(key);
            return result;
        }

        public static Boolean[] SMIsMember<T>(String key, Object[] members)
        {
            var result = _redisClient.SMIsMember<T>(key, members);
            return result;
        }

        public static Boolean SMove<T>(String source, String destination, T member)
        {
            var result = _redisClient.SMove<T>(source, destination, member);
            return result;
        }

        public static String SPop(String key)
        {
            var result = _redisClient.SPop(key);
            return result;
        }

        public static T SPop<T>(String key)
        {
            var result = _redisClient.SPop<T>(key);
            return result;
        }

        public static String[] SPop(String key, Int32 count)
        {
            var result = _redisClient.SPop(key, count);
            return result;
        }

        public static T[] SPop<T>(String key, Int32 count)
        {
            var result = _redisClient.SPop<T>(key, count);
            return result;
        }

        public static String SRandMember(String key)
        {
            var result = _redisClient.SRandMember(key);
            return result;
        }

        public static T SRandMember<T>(String key)
        {
            var result = _redisClient.SRandMember<T>(key);
            return result;
        }

        public static String[] SRandMember(String key, Int32 count)
        {
            var result = _redisClient.SRandMember(key, count);
            return result;
        }

        public static T[] SRandMember<T>(String key, Int32 count)
        {
            var result = _redisClient.SRandMember<T>(key, count);
            return result;
        }

        public static Int64 SRem(String key, Object[] members)
        {
            var result = _redisClient.SRem(key, members);
            return result;
        }

        public static ScanResult<String> SScan(String key, Int64 cursor, String pattern, Int64 count)
        {
            var result = _redisClient.SScan(key, cursor, pattern, count);
            return result;
        }

        public static String[] SUnion(params string[] keys)
        {
            var result = _redisClient.SUnion(keys);
            return result;
        }

        public static T[] SUnion<T>(params string[] keys)
        {
            var result = _redisClient.SUnion<T>(keys);
            return result;
        }

        public static Int64 SUnionStore(String destination, String[] keys)
        {
            var result = _redisClient.SUnionStore(destination, keys);
            return result;
        }

        public static async Task<ZMember> BZPopMinAsync(String key, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BZPopMinAsync(key, timeoutSeconds);
            return result;
        }

        public static async Task<KeyValue<ZMember>> BZPopMinAsync(String[] keys, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BZPopMinAsync(keys, timeoutSeconds);
            return result;
        }

        public static async Task<ZMember> BZPopMaxAsync(String key, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BZPopMaxAsync(key, timeoutSeconds);
            return result;
        }

        public static async Task<KeyValue<ZMember>> BZPopMaxAsync(String[] keys, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BZPopMaxAsync(keys, timeoutSeconds);
            return result;
        }

        public static async Task<Int64> ZAddAsync(String key, Decimal score, String member, Object[] scoreMembers)
        {
            var result = await _redisClient.ZAddAsync(key, score, member, scoreMembers);
            return result;
        }

        public static async Task<Int64> ZAddAsync(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = await _redisClient.ZAddAsync(key, scoreMembers, than, ch);
            return result;
        }

        public static async Task<Int64> ZAddNxAsync(String key, Decimal score, String member, Object[] scoreMembers)
        {
            var result = await _redisClient.ZAddNxAsync(key, score, member, scoreMembers);
            return result;
        }

        public static async Task<Int64> ZAddNxAsync(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = await _redisClient.ZAddNxAsync(key, scoreMembers, than, ch);
            return result;
        }

        public static async Task<Int64> ZAddXxAsync(String key, Decimal score, String member, Object[] scoreMembers)
        {
            var result = await _redisClient.ZAddXxAsync(key, score, member, scoreMembers);
            return result;
        }

        public static async Task<Int64> ZAddXxAsync(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = await _redisClient.ZAddXxAsync(key, scoreMembers, than, ch);
            return result;
        }

        public static async Task<Int64> ZCardAsync(String key)
        {
            var result = await _redisClient.ZCardAsync(key);
            return result;
        }

        public static async Task<Int64> ZCountAsync(String key, Decimal min, Decimal max)
        {
            var result = await _redisClient.ZCountAsync(key, min, max);
            return result;
        }

        public static async Task<Int64> ZCountAsync(String key, String min, String max)
        {
            var result = await _redisClient.ZCountAsync(key, min, max);
            return result;
        }

        public static async Task<Decimal> ZIncrByAsync(String key, Decimal increment, String member)
        {
            var result = await _redisClient.ZIncrByAsync(key, increment, member);
            return result;
        }

        public static async Task<Int64> ZInterStoreAsync(String destination, String[] keys, Int32[] weights, Nullable<ZAggregate> aggregate)
        {
            var result = await _redisClient.ZInterStoreAsync(destination, keys, weights, aggregate);
            return result;
        }

        public static async Task<Int64> ZLexCountAsync(String key, String min, String max)
        {
            var result = await _redisClient.ZLexCountAsync(key, min, max);
            return result;
        }

        public static async Task<ZMember> ZPopMinAsync(String key)
        {
            var result = await _redisClient.ZPopMinAsync(key);
            return result;
        }

        public static async Task<ZMember[]> ZPopMinAsync(String key, Int32 count)
        {
            var result = await _redisClient.ZPopMinAsync(key, count);
            return result;
        }

        public static async Task<ZMember> ZPopMaxAsync(String key)
        {
            var result = await _redisClient.ZPopMaxAsync(key);
            return result;
        }

        public static async Task<ZMember[]> ZPopMaxAsync(String key, Int32 count)
        {
            var result = await _redisClient.ZPopMaxAsync(key, count);
            return result;
        }

        public static async Task<String[]> ZRangeAsync(String key, Decimal start, Decimal stop)
        {
            var result = await _redisClient.ZRangeAsync(key, start, stop);
            return result;
        }

        public static async Task<ZMember[]> ZRangeWithScoresAsync(String key, Decimal start, Decimal stop)
        {
            var result = await _redisClient.ZRangeWithScoresAsync(key, start, stop);
            return result;
        }

        public static async Task<String[]> ZRangeByLexAsync(String key, String min, String max, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRangeByLexAsync(key, min, max, offset, count);
            return result;
        }

        public static async Task<String[]> ZRangeByScoreAsync(String key, Decimal min, Decimal max, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRangeByScoreAsync(key, min, max, offset, count);
            return result;
        }

        public static async Task<String[]> ZRangeByScoreAsync(String key, String min, String max, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRangeByScoreAsync(key, min, max, offset, count);
            return result;
        }

        public static async Task<ZMember[]> ZRangeByScoreWithScoresAsync(String key, Decimal min, Decimal max, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRangeByScoreWithScoresAsync(key, min, max, offset, count);
            return result;
        }

        public static async Task<ZMember[]> ZRangeByScoreWithScoresAsync(String key, String min, String max, Int32 offset, Int32 count)
        {
            var result = await _redisClient.ZRangeByScoreWithScoresAsync(key, min, max, offset, count);
            return result;
        }

        public static async Task<Int64> ZRankAsync(String key, String member)
        {
            var result = await _redisClient.ZRankAsync(key, member);
            return result;
        }

        public static async Task<Int64> ZRemAsync(String key, String[] members)
        {
            var result = await _redisClient.ZRemAsync(key, members);
            return result;
        }

        public static async Task<Int64> ZRemRangeByLexAsync(String key, String min, String max)
        {
            var result = await _redisClient.ZRemRangeByLexAsync(key, min, max);
            return result;
        }

        public static async Task<Int64> ZRemRangeByRankAsync(String key, Int64 start, Int64 stop)
        {
            var result = await _redisClient.ZRemRangeByRankAsync(key, start, stop);
            return result;
        }

        public static async Task<Int64> ZRemRangeByScoreAsync(String key, Decimal min, Decimal max)
        {
            var result = await _redisClient.ZRemRangeByScoreAsync(key, min, max);
            return result;
        }

        public static async Task<Int64> ZRemRangeByScoreAsync(String key, String min, String max)
        {
            var result = await _redisClient.ZRemRangeByScoreAsync(key, min, max);
            return result;
        }

        public static async Task<String[]> ZRevRangeAsync(String key, Decimal start, Decimal stop)
        {
            var result = await _redisClient.ZRevRangeAsync(key, start, stop);
            return result;
        }

        public static Int64 PubSubNumPat(String message)
        {
            var result = _redisClient.PubSubNumPat(message);
            return result;
        }

        public static void PUnSubscribe(params string[] pattern)
        {
            _redisClient.PUnSubscribe(pattern);
        }

        public static IDisposable Subscribe(String[] channels, Action<String, Object> handler)
        {
            var result = _redisClient.Subscribe(channels, handler);
            return result;
        }

        public static IDisposable Subscribe(String channel, Action<String, Object> handler)
        {
            var result = _redisClient.Subscribe(channel, handler);
            return result;
        }

        public static void UnSubscribe(params string[] channels)
        {
            _redisClient.UnSubscribe(channels);
        }

        public static async Task<Object> EvalAsync(String script, String[] keys, Object[] arguments)
        {
            var result = await _redisClient.EvalAsync(script, keys, arguments);
            return result;
        }

        public static async Task<Object> EvalShaAsync(String sha1, String[] keys, Object[] arguments)
        {
            var result = await _redisClient.EvalShaAsync(sha1, keys, arguments);
            return result;
        }

        public static async Task<Boolean> ScriptExistsAsync(String sha1)
        {
            var result = await _redisClient.ScriptExistsAsync(sha1);
            return result;
        }

        public static async Task<Boolean[]> ScriptExistsAsync(params string[] sha1)
        {
            var result = await _redisClient.ScriptExistsAsync(sha1);
            return result;
        }

        public static async Task ScriptFlushAsync()
        {
            await _redisClient.ScriptFlushAsync();
        }

        public static async Task ScriptKillAsync()
        {
            await _redisClient.ScriptKillAsync();
        }

        public static async Task<String> ScriptLoadAsync(String script)
        {
            var result = await _redisClient.ScriptLoadAsync(script);
            return result;
        }

        public static Object Eval(String script, String[] keys, Object[] arguments)
        {
            var result = _redisClient.Eval(script, keys, arguments);
            return result;
        }

        public static Object EvalSha(String sha1, String[] keys, Object[] arguments)
        {
            var result = _redisClient.EvalSha(sha1, keys, arguments);
            return result;
        }

        public static Boolean ScriptExists(String sha1)
        {
            var result = _redisClient.ScriptExists(sha1);
            return result;
        }

        public static Boolean[] ScriptExists(params string[] sha1)
        {
            var result = _redisClient.ScriptExists(sha1);
            return result;
        }

        public static void ScriptFlush()
        {
            _redisClient.ScriptFlush();
        }

        public static void ScriptKill()
        {
            _redisClient.ScriptKill();
        }

        public static String ScriptLoad(String script)
        {
            var result = _redisClient.ScriptLoad(script);
            return result;
        }

        public static String[] AclCat(String categoryname)
        {
            var result = _redisClient.AclCat(categoryname);
            return result;
        }

        public static Int64 AclDelUser(params string[] username)
        {
            var result = _redisClient.AclDelUser(username);
            return result;
        }

        public static String AclGenPass(Int32 bits)
        {
            var result = _redisClient.AclGenPass(bits);
            return result;
        }

        public static AclGetUserResult AclGetUser(String username)
        {
            var result = _redisClient.AclGetUser(username);
            return result;
        }

        public static String[] AclList()
        {
            var result = _redisClient.AclList();
            return result;
        }

        public static void AclLoad()
        {
            _redisClient.AclLoad();
        }

        public static LogResult[] AclLog(Int64 count)
        {
            var result = _redisClient.AclLog(count);
            return result;
        }

        public static void AclSave()
        {
            _redisClient.AclSave();
        }

        public static void AclSetUser(String username, String[] rule)
        {
            _redisClient.AclSetUser(username, rule);
        }

        public static String[] AclUsers()
        {
            var result = _redisClient.AclUsers();
            return result;
        }

        public static String AclWhoami()
        {
            var result = _redisClient.AclWhoami();
            return result;
        }

        public static String BgRewriteAof()
        {
            var result = _redisClient.BgRewriteAof();
            return result;
        }

        public static String BgSave(String schedule)
        {
            var result = _redisClient.BgSave(schedule);
            return result;
        }

        public static Object[] Command()
        {
            var result = _redisClient.Command();
            return result;
        }

        public static Int64 CommandCount()
        {
            var result = _redisClient.CommandCount();
            return result;
        }

        public static String[] CommandGetKeys(params string[] command)
        {
            var result = _redisClient.CommandGetKeys(command);
            return result;
        }

        public static Object[] CommandInfo(params string[] command)
        {
            var result = _redisClient.CommandInfo(command);
            return result;
        }

        public static Dictionary<String, String> ConfigGet(String parameter)
        {
            var result = _redisClient.ConfigGet(parameter);
            return result;
        }

        public static void ConfigResetStat()
        {
            _redisClient.ConfigResetStat();
        }

        public static void ConfigRewrite()
        {
            _redisClient.ConfigRewrite();
        }

        public static void ConfigSet<T>(String parameter, T value)
        {
            _redisClient.ConfigSet<T>(parameter, value);
        }

        public static Int64 DbSize()
        {
            var result = _redisClient.DbSize();
            return result;
        }

        public static String DebugObject(String key)
        {
            var result = _redisClient.DebugObject(key);
            return result;
        }

        public static void FlushAll(Boolean isasync)
        {
            _redisClient.FlushAll(isasync);
        }

        public static void FlushDb(Boolean isasync)
        {
            _redisClient.FlushDb(isasync);
        }

        public static String Info(String section)
        {
            var result = _redisClient.Info(section);
            return result;
        }

        public static DateTime LastSave()
        {
            var result = _redisClient.LastSave();
            return result;
        }

        public static String LatencyDoctor()
        {
            var result = _redisClient.LatencyDoctor();
            return result;
        }

        public static String MemoryDoctor()
        {
            var result = _redisClient.MemoryDoctor();
            return result;
        }

        public static String MemoryMallocStats()
        {
            var result = _redisClient.MemoryMallocStats();
            return result;
        }

        public static void MemoryPurge()
        {
            _redisClient.MemoryPurge();
        }

        public static Dictionary<String, Object> MemoryStats()
        {
            var result = _redisClient.MemoryStats();
            return result;
        }

        public static Int64 MemoryUsage(String key, Int64 count)
        {
            var result = _redisClient.MemoryUsage(key, count);
            return result;
        }

        public static void ReplicaOf(String host, Int32 port)
        {
            _redisClient.ReplicaOf(host, port);
        }

        public static RoleResult Role()
        {
            var result = _redisClient.Role();
            return result;
        }

        public static void Save()
        {
            _redisClient.Save();
        }

        public static void SlaveOf(String host, Int32 port)
        {
            _redisClient.SlaveOf(host, port);
        }

        public static Object SlowLog(String subcommand, String[] argument)
        {
            var result = _redisClient.SlowLog(subcommand, argument);
            return result;
        }

        public static void SwapDb(Int32 index1, Int32 index2)
        {
            _redisClient.SwapDb(index1, index2);
        }

        public static DateTime Time()
        {
            var result = _redisClient.Time();
            return result;
        }

        public static async Task<Int64> SAddAsync(String key, Object[] members)
        {
            var result = await _redisClient.SAddAsync(key, members);
            return result;
        }

        public static async Task<Int64> SCardAsync(String key)
        {
            var result = await _redisClient.SCardAsync(key);
            return result;
        }

        public static async Task<String[]> SDiffAsync(params string[] keys)
        {
            var result = await _redisClient.SDiffAsync(keys);
            return result;
        }

        public static async Task<T[]> SDiffAsync<T>(params string[] keys)
        {
            var result = await _redisClient.SDiffAsync<T>(keys);
            return result;
        }

        public static async Task<Int64> SDiffStoreAsync(String destination, String[] keys)
        {
            var result = await _redisClient.SDiffStoreAsync(destination, keys);
            return result;
        }

        public static async Task<String[]> SInterAsync(params string[] keys)
        {
            var result = await _redisClient.SInterAsync(keys);
            return result;
        }

        public static async Task<T[]> SInterAsync<T>(params string[] keys)
        {
            var result = await _redisClient.SInterAsync<T>(keys);
            return result;
        }

        public static async Task<Int64> SInterStoreAsync(String destination, String[] keys)
        {
            var result = await _redisClient.SInterStoreAsync(destination, keys);
            return result;
        }

        public static async Task<Boolean> SIsMemberAsync<T>(String key, T member)
        {
            var result = await _redisClient.SIsMemberAsync<T>(key, member);
            return result;
        }

        public static async Task<String[]> SMembersAsync(String key)
        {
            var result = await _redisClient.SMembersAsync(key);
            return result;
        }

        public static async Task<T[]> SMembersAsync<T>(String key)
        {
            var result = await _redisClient.SMembersAsync<T>(key);
            return result;
        }

        public static async Task<Boolean[]> SMIsMemberAsync<T>(String key, Object[] members)
        {
            var result = await _redisClient.SMIsMemberAsync<T>(key, members);
            return result;
        }

        public static async Task<Boolean> SMoveAsync<T>(String source, String destination, T member)
        {
            var result = await _redisClient.SMoveAsync<T>(source, destination, member);
            return result;
        }

        public static async Task<String> SPopAsync(String key)
        {
            var result = await _redisClient.SPopAsync(key);
            return result;
        }

        public static async Task<T> SPopAsync<T>(String key)
        {
            var result = await _redisClient.SPopAsync<T>(key);
            return result;
        }

        public static async Task<String[]> SPopAsync(String key, Int32 count)
        {
            var result = await _redisClient.SPopAsync(key, count);
            return result;
        }

        public static async Task<T[]> SPopAsync<T>(String key, Int32 count)
        {
            var result = await _redisClient.SPopAsync<T>(key, count);
            return result;
        }

        public static async Task<String> SRandMemberAsync(String key)
        {
            var result = await _redisClient.SRandMemberAsync(key);
            return result;
        }

        public static async Task<T> SRandMemberAsync<T>(String key)
        {
            var result = await _redisClient.SRandMemberAsync<T>(key);
            return result;
        }

        public static T BRPop<T>(String key, Int32 timeoutSeconds)
        {
            var result = _redisClient.BRPop<T>(key, timeoutSeconds);
            return result;
        }

        public static KeyValue<String> BRPop(String[] keys, Int32 timeoutSeconds)
        {
            var result = _redisClient.BRPop(keys, timeoutSeconds);
            return result;
        }

        public static KeyValue<T> BRPop<T>(String[] keys, Int32 timeoutSeconds)
        {
            var result = _redisClient.BRPop<T>(keys, timeoutSeconds);
            return result;
        }

        public static String BRPopLPush(String source, String destination, Int32 timeoutSeconds)
        {
            var result = _redisClient.BRPopLPush(source, destination, timeoutSeconds);
            return result;
        }

        public static T BRPopLPush<T>(String source, String destination, Int32 timeoutSeconds)
        {
            var result = _redisClient.BRPopLPush<T>(source, destination, timeoutSeconds);
            return result;
        }

        public static String LIndex(String key, Int64 index)
        {
            var result = _redisClient.LIndex(key, index);
            return result;
        }

        public static T LIndex<T>(String key, Int64 index)
        {
            var result = _redisClient.LIndex<T>(key, index);
            return result;
        }

        public static Int64 LInsert(String key, InsertDirection direction, Object pivot, Object element)
        {
            var result = _redisClient.LInsert(key, direction, pivot, element);
            return result;
        }

        public static Int64 LLen(String key)
        {
            var result = _redisClient.LLen(key);
            return result;
        }

        public static String LPop(String key)
        {
            var result = _redisClient.LPop(key);
            return result;
        }

        public static T LPop<T>(String key)
        {
            var result = _redisClient.LPop<T>(key);
            return result;
        }

        public static Int64 LPos<T>(String key, T element, Int32 rank)
        {
            var result = _redisClient.LPos<T>(key, element, rank);
            return result;
        }

        public static Int64[] LPos<T>(String key, T element, Int32 rank, Int32 count, Int32 maxLen)
        {
            var result = _redisClient.LPos<T>(key, element, rank, count, maxLen);
            return result;
        }

        public static Int64 LPush(String key, Object[] elements)
        {
            var result = _redisClient.LPush(key, elements);
            return result;
        }

        public static Int64 LPushX(String key, Object[] elements)
        {
            var result = _redisClient.LPushX(key, elements);
            return result;
        }

        public static String[] LRange(String key, Int64 start, Int64 stop)
        {
            var result = _redisClient.LRange(key, start, stop);
            return result;
        }

        public static T[] LRange<T>(String key, Int64 start, Int64 stop)
        {
            var result = _redisClient.LRange<T>(key, start, stop);
            return result;
        }

        public static Int64 LRem<T>(String key, Int64 count, T element)
        {
            var result = _redisClient.LRem<T>(key, count, element);
            return result;
        }

        public static void LSet<T>(String key, Int64 index, T element)
        {
            _redisClient.LSet<T>(key, index, element);
        }

        public static void LTrim(String key, Int64 start, Int64 stop)
        {
            _redisClient.LTrim(key, start, stop);
        }

        public static String RPop(String key)
        {
            var result = _redisClient.RPop(key);
            return result;
        }

        public static T RPop<T>(String key)
        {
            var result = _redisClient.RPop<T>(key);
            return result;
        }

        public static String RPopLPush(String source, String destination)
        {
            var result = _redisClient.RPopLPush(source, destination);
            return result;
        }

        public static T RPopLPush<T>(String source, String destination)
        {
            var result = _redisClient.RPopLPush<T>(source, destination);
            return result;
        }

        public static Int64 RPush(String key, Object[] elements)
        {
            var result = _redisClient.RPush(key, elements);
            return result;
        }

        public static Int64 RPushX(String key, Object[] elements)
        {
            var result = _redisClient.RPushX(key, elements);
            return result;
        }

        public static String BfReserve(String key, Decimal errorRate, Int64 capacity, Int32 expansion, Boolean nonScaling)
        {
            var result = _redisClient.BfReserve(key, errorRate, capacity, expansion, nonScaling);
            return result;
        }

        public static Boolean BfAdd(String key, String item)
        {
            var result = _redisClient.BfAdd(key, item);
            return result;
        }

        public static Boolean[] BfMAdd(String key, String[] items)
        {
            var result = _redisClient.BfMAdd(key, items);
            return result;
        }

        public static String BfInsert(String key, String[] items, Nullable<Int64> capacity, String error, Int32 expansion, Boolean noCreate, Boolean nonScaling)
        {
            var result = _redisClient.BfInsert(key, items, capacity, error, expansion, noCreate, nonScaling);
            return result;
        }

        public static Boolean BfExists(String key, String item)
        {
            var result = _redisClient.BfExists(key, item);
            return result;
        }

        public static Boolean[] BfMExists(String key, String[] items)
        {
            var result = _redisClient.BfMExists(key, items);
            return result;
        }

        public static ScanResult<Byte[]> BfScanDump(String key, Int64 iter)
        {
            var result = _redisClient.BfScanDump(key, iter);
            return result;
        }

        public static String BfLoadChunk(String key, Int64 iter, Byte[] data)
        {
            var result = _redisClient.BfLoadChunk(key, iter, data);
            return result;
        }

        public static Dictionary<String, String> BfInfo(String key)
        {
            var result = _redisClient.BfInfo(key);
            return result;
        }

        public static RedisClient.LockController Lock(String name, Int32 timeoutSeconds, Boolean autoDelay)
        {
            var result = _redisClient.Lock(name, timeoutSeconds, autoDelay);
            return result;
        }

        public static String CmsInitByDim(String key, Int64 width, Int64 depth)
        {
            var result = _redisClient.CmsInitByDim(key, width, depth);
            return result;
        }

        public static String CmsInitByProb(String key, Decimal error, Decimal probability)
        {
            var result = _redisClient.CmsInitByProb(key, error, probability);
            return result;
        }

        public static Int64 CmsIncrBy(String key, String item, Int64 increment)
        {
            var result = _redisClient.CmsIncrBy(key, item, increment);
            return result;
        }

        public static Int64[] CmsIncrBy(String key, Dictionary<String, Int64> itemIncrements)
        {
            var result = _redisClient.CmsIncrBy(key, itemIncrements);
            return result;
        }

        public static Int64[] CmsQuery(String key, String[] items)
        {
            var result = _redisClient.CmsQuery(key, items);
            return result;
        }

        public static String CmsMerge(String dest, Int64 numKeys, String[] src, Int64[] weights)
        {
            var result = _redisClient.CmsMerge(dest, numKeys, src, weights);
            return result;
        }

        public static Dictionary<String, String> CmsInfo(String key)
        {
            var result = _redisClient.CmsInfo(key);
            return result;
        }

        public static String CfReserve(String key, Int64 capacity, Nullable<Int64> bucketSize, Nullable<Int64> maxIterations, Nullable<Int32> expansion)
        {
            var result = _redisClient.CfReserve(key, capacity, bucketSize, maxIterations, expansion);
            return result;
        }

        public static Boolean CfAdd(String key, String item)
        {
            var result = _redisClient.CfAdd(key, item);
            return result;
        }

        public static Boolean CfAddNx(String key, String item)
        {
            var result = _redisClient.CfAddNx(key, item);
            return result;
        }

        public static String CfInsert(String key, String[] items, Nullable<Int64> capacity, Boolean noCreate)
        {
            var result = _redisClient.CfInsert(key, items, capacity, noCreate);
            return result;
        }

        public static String CfInsertNx(String key, String[] items, Nullable<Int64> capacity, Boolean noCreate)
        {
            var result = _redisClient.CfInsertNx(key, items, capacity, noCreate);
            return result;
        }

        public static Boolean CfExists(String key, String item)
        {
            var result = _redisClient.CfExists(key, item);
            return result;
        }

        public static Boolean CfDel(String key, String item)
        {
            var result = _redisClient.CfDel(key, item);
            return result;
        }

        public static Int64 CfCount(String key, String item)
        {
            var result = _redisClient.CfCount(key, item);
            return result;
        }

        public static ScanResult<Byte[]> CfScanDump(String key, Int64 iter)
        {
            var result = _redisClient.CfScanDump(key, iter);
            return result;
        }

        public static String CfLoadChunk(String key, Int64 iter, Byte[] data)
        {
            var result = _redisClient.CfLoadChunk(key, iter, data);
            return result;
        }

        public static Dictionary<String, String> CfInfo(String key)
        {
            var result = _redisClient.CfInfo(key);
            return result;
        }

        public static String TopkReserve(String key, Int64 topk, Int64 width, Int64 depth, Decimal decay)
        {
            var result = _redisClient.TopkReserve(key, topk, width, depth, decay);
            return result;
        }

        public static String[] TopkAdd(String key, String[] items)
        {
            var result = _redisClient.TopkAdd(key, items);
            return result;
        }

        public static String TopkIncrBy(String key, String item, Int64 increment)
        {
            var result = _redisClient.TopkIncrBy(key, item, increment);
            return result;
        }

        public static String[] TopkIncrBy(String key, Dictionary<String, Int64> itemIncrements)
        {
            var result = _redisClient.TopkIncrBy(key, itemIncrements);
            return result;
        }

        public static Boolean[] TopkQuery(String key, String[] items)
        {
            var result = _redisClient.TopkQuery(key, items);
            return result;
        }

        public static Int64[] TopkCount(String key, String[] items)
        {
            var result = _redisClient.TopkCount(key, items);
            return result;
        }

        public static String[] TopkList(String key)
        {
            var result = _redisClient.TopkList(key);
            return result;
        }

        public static Dictionary<String, String> TopkInfo(String key)
        {
            var result = _redisClient.TopkInfo(key);
            return result;
        }

        public static async Task<Int64> PublishAsync(String channel, String message)
        {
            var result = await _redisClient.PublishAsync(channel, message);
            return result;
        }

        public static async Task<String[]> PubSubChannelsAsync(String pattern)
        {
            var result = await _redisClient.PubSubChannelsAsync(pattern);
            return result;
        }

        public static async Task<Int64> PubSubNumSubAsync(String channel)
        {
            var result = await _redisClient.PubSubNumSubAsync(channel);
            return result;
        }

        public static async Task<Int64[]> PubSubNumSubAsync(params string[] channels)
        {
            var result = await _redisClient.PubSubNumSubAsync(channels);
            return result;
        }

        public static async Task<Int64> PubSubNumPatAsync(String message)
        {
            var result = await _redisClient.PubSubNumPatAsync(message);
            return result;
        }

        public static IDisposable PSubscribe(String pattern, Action<String, Object> handler)
        {
            var result = _redisClient.PSubscribe(pattern, handler);
            return result;
        }

        public static IDisposable PSubscribe(String[] pattern, Action<String, Object> handler)
        {
            var result = _redisClient.PSubscribe(pattern, handler);
            return result;
        }

        public static Int64 Publish(String channel, String message)
        {
            var result = _redisClient.Publish(channel, message);
            return result;
        }

        public static String[] PubSubChannels(String pattern)
        {
            var result = _redisClient.PubSubChannels(pattern);
            return result;
        }

        public static Int64 PubSubNumSub(String channel)
        {
            var result = _redisClient.PubSubNumSub(channel);
            return result;
        }

        public static Int64[] PubSubNumSub(params string[] channels)
        {
            var result = _redisClient.PubSubNumSub(channels);
            return result;
        }

        public static async Task<Int64> SortStoreAsync(String storeDestination, String key, String byPattern, Int64 offset, Int64 count, String[] getPatterns, Nullable<Collation> collation, Boolean alpha)
        {
            var result = await _redisClient.SortStoreAsync(storeDestination, key, byPattern, offset, count, getPatterns, collation, alpha);
            return result;
        }

        public static async Task<Int64> TouchAsync(params string[] keys)
        {
            var result = await _redisClient.TouchAsync(keys);
            return result;
        }

        public static async Task<Int64> TtlAsync(String key)
        {
            var result = await _redisClient.TtlAsync(key);
            return result;
        }

        public static async Task<KeyType> TypeAsync(String key)
        {
            var result = await _redisClient.TypeAsync(key);
            return result;
        }

        public static async Task<Int64> UnLinkAsync(params string[] keys)
        {
            var result = await _redisClient.UnLinkAsync(keys);
            return result;
        }

        public static async Task<Int64> WaitAsync(Int64 numreplicas, Int64 timeoutMilliseconds)
        {
            var result = await _redisClient.WaitAsync(numreplicas, timeoutMilliseconds);
            return result;
        }

        public static Int64 Del(params string[] keys)
        {
            var result = _redisClient.Del(keys);
            return result;
        }

        public static Byte[] Dump(String key)
        {
            var result = _redisClient.Dump(key);
            return result;
        }

        public static Boolean Exists(String key)
        {
            var result = _redisClient.Exists(key);
            return result;
        }

        public static Int64 Exists(params string[] keys)
        {
            var result = _redisClient.Exists(keys);
            return result;
        }

        public static Boolean Expire(String key, Int32 seconds)
        {
            var result = _redisClient.Expire(key, seconds);
            return result;
        }

        public static Boolean ExpireAt(String key, DateTime timestamp)
        {
            var result = _redisClient.ExpireAt(key, timestamp);
            return result;
        }

        public static String[] Keys(String pattern)
        {
            var result = _redisClient.Keys(pattern);
            return result;
        }

        public static void Migrate(String host, Int32 port, String key, Int32 destinationDb, Int64 timeoutMilliseconds, Boolean copy, Boolean replace, String authPassword, String auth2Username, String auth2Password, String[] keys)
        {
            _redisClient.Migrate(host, port, key, destinationDb, timeoutMilliseconds, copy, replace, authPassword, auth2Username, auth2Password, keys);
        }

        public static Boolean Move(String key, Int32 db)
        {
            var result = _redisClient.Move(key, db);
            return result;
        }

        public static Nullable<Int64> ObjectRefCount(String key)
        {
            var result = _redisClient.ObjectRefCount(key);
            return result;
        }

        public static Int64 ObjectIdleTime(String key)
        {
            var result = _redisClient.ObjectIdleTime(key);
            return result;
        }

        public static String ObjectEncoding(String key)
        {
            var result = _redisClient.ObjectEncoding(key);
            return result;
        }

        public static Nullable<Int64> ObjectFreq(String key)
        {
            var result = _redisClient.ObjectFreq(key);
            return result;
        }

        public static Boolean Persist(String key)
        {
            var result = _redisClient.Persist(key);
            return result;
        }

        public static Boolean PExpire(String key, Int32 milliseconds)
        {
            var result = _redisClient.PExpire(key, milliseconds);
            return result;
        }

        public static Boolean PExpireAt(String key, DateTime timestamp)
        {
            var result = _redisClient.PExpireAt(key, timestamp);
            return result;
        }

        public static Int64 PTtl(String key)
        {
            var result = _redisClient.PTtl(key);
            return result;
        }

        public static String RandomKey()
        {
            var result = _redisClient.RandomKey();
            return result;
        }

        public static void Rename(String key, String newkey)
        {
            _redisClient.Rename(key, newkey);
        }

        public static Boolean RenameNx(String key, String newkey)
        {
            var result = _redisClient.RenameNx(key, newkey);
            return result;
        }

        public static void Restore(String key, Byte[] serializedValue)
        {
            _redisClient.Restore(key, serializedValue);
        }

        public static void Restore(String key, Int32 ttl, Byte[] serializedValue, Boolean replace, Boolean absTtl, Nullable<Int32> idleTimeSeconds, Nullable<Decimal> frequency)
        {
            _redisClient.Restore(key, ttl, serializedValue, replace, absTtl, idleTimeSeconds, frequency);
        }

        public static ScanResult<String> Scan(Int64 cursor, String pattern, Int64 count, String type)
        {
            var result = _redisClient.Scan(cursor, pattern, count, type);
            return result;
        }

        public static IEnumerable<String[]> Scan(String pattern, Int64 count, String type)
        {
            var result = _redisClient.Scan(pattern, count, type);
            return result;
        }

        public static String[] Sort(String key, String byPattern, Int64 offset, Int64 count, String[] getPatterns, Nullable<Collation> collation, Boolean alpha)
        {
            var result = _redisClient.Sort(key, byPattern, offset, count, getPatterns, collation, alpha);
            return result;
        }

        public static Int64 SortStore(String storeDestination, String key, String byPattern, Int64 offset, Int64 count, String[] getPatterns, Nullable<Collation> collation, Boolean alpha)
        {
            var result = _redisClient.SortStore(storeDestination, key, byPattern, offset, count, getPatterns, collation, alpha);
            return result;
        }

        public static Int64 Touch(params string[] keys)
        {
            var result = _redisClient.Touch(keys);
            return result;
        }

        public static Int64 Ttl(String key)
        {
            var result = _redisClient.Ttl(key);
            return result;
        }

        public static KeyType Type(String key)
        {
            var result = _redisClient.Type(key);
            return result;
        }

        public static Int64 UnLink(params string[] keys)
        {
            var result = _redisClient.UnLink(keys);
            return result;
        }

        public static Int64 Wait(Int64 numreplicas, Int64 timeoutMilliseconds)
        {
            var result = _redisClient.Wait(numreplicas, timeoutMilliseconds);
            return result;
        }

        public static async Task<String> BLPopAsync(String key, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BLPopAsync(key, timeoutSeconds);
            return result;
        }

        public static async Task<T> BLPopAsync<T>(String key, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BLPopAsync<T>(key, timeoutSeconds);
            return result;
        }

        public static async Task<KeyValue<String>> BLPopAsync(String[] keys, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BLPopAsync(keys, timeoutSeconds);
            return result;
        }

        public static async Task<KeyValue<T>> BLPopAsync<T>(String[] keys, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BLPopAsync<T>(keys, timeoutSeconds);
            return result;
        }

        public static async Task<String> BRPopAsync(String key, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BRPopAsync(key, timeoutSeconds);
            return result;
        }

        public static async Task<T> BRPopAsync<T>(String key, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BRPopAsync<T>(key, timeoutSeconds);
            return result;
        }

        public static async Task<KeyValue<String>> BRPopAsync(String[] keys, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BRPopAsync(keys, timeoutSeconds);
            return result;
        }

        public static async Task<KeyValue<T>> BRPopAsync<T>(String[] keys, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BRPopAsync<T>(keys, timeoutSeconds);
            return result;
        }

        public static async Task<String> BRPopLPushAsync(String source, String destination, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BRPopLPushAsync(source, destination, timeoutSeconds);
            return result;
        }

        public static async Task<T> BRPopLPushAsync<T>(String source, String destination, Int32 timeoutSeconds)
        {
            var result = await _redisClient.BRPopLPushAsync<T>(source, destination, timeoutSeconds);
            return result;
        }

        public static async Task<String> LIndexAsync(String key, Int64 index)
        {
            var result = await _redisClient.LIndexAsync(key, index);
            return result;
        }

        public static async Task<T> LIndexAsync<T>(String key, Int64 index)
        {
            var result = await _redisClient.LIndexAsync<T>(key, index);
            return result;
        }

        public static async Task<Int64> LInsertAsync(String key, InsertDirection direction, Object pivot, Object element)
        {
            var result = await _redisClient.LInsertAsync(key, direction, pivot, element);
            return result;
        }

        public static async Task<Int64> LLenAsync(String key)
        {
            var result = await _redisClient.LLenAsync(key);
            return result;
        }

        public static async Task<String> LPopAsync(String key)
        {
            var result = await _redisClient.LPopAsync(key);
            return result;
        }

        public static async Task<T> LPopAsync<T>(String key)
        {
            var result = await _redisClient.LPopAsync<T>(key);
            return result;
        }

        public static async Task<Int64> LPosAsync<T>(String key, T element, Int32 rank)
        {
            var result = await _redisClient.LPosAsync<T>(key, element, rank);
            return result;
        }

        public static async Task<Int64[]> LPosAsync<T>(String key, T element, Int32 rank, Int32 count, Int32 maxLen)
        {
            var result = await _redisClient.LPosAsync<T>(key, element, rank, count, maxLen);
            return result;
        }

        public static async Task<Int64> LPushAsync(String key, Object[] elements)
        {
            var result = await _redisClient.LPushAsync(key, elements);
            return result;
        }

        public static async Task<Int64> LPushXAsync(String key, Object[] elements)
        {
            var result = await _redisClient.LPushXAsync(key, elements);
            return result;
        }

        public static async Task<String[]> LRangeAsync(String key, Int64 start, Int64 stop)
        {
            var result = await _redisClient.LRangeAsync(key, start, stop);
            return result;
        }

        public static async Task<T[]> LRangeAsync<T>(String key, Int64 start, Int64 stop)
        {
            var result = await _redisClient.LRangeAsync<T>(key, start, stop);
            return result;
        }

        public static async Task<Int64> LRemAsync<T>(String key, Int64 count, T element)
        {
            var result = await _redisClient.LRemAsync<T>(key, count, element);
            return result;
        }

        public static async Task LSetAsync<T>(String key, Int64 index, T element)
        {
            await _redisClient.LSetAsync<T>(key, index, element);
        }

        public static async Task LTrimAsync(String key, Int64 start, Int64 stop)
        {
            await _redisClient.LTrimAsync(key, start, stop);
        }

        public static async Task<String> RPopAsync(String key)
        {
            var result = await _redisClient.RPopAsync(key);
            return result;
        }

        public static async Task<T> RPopAsync<T>(String key)
        {
            var result = await _redisClient.RPopAsync<T>(key);
            return result;
        }

        public static async Task<String> RPopLPushAsync(String source, String destination)
        {
            var result = await _redisClient.RPopLPushAsync(source, destination);
            return result;
        }

        public static async Task<T> RPopLPushAsync<T>(String source, String destination)
        {
            var result = await _redisClient.RPopLPushAsync<T>(source, destination);
            return result;
        }

        public static async Task<Int64> RPushAsync(String key, Object[] elements)
        {
            var result = await _redisClient.RPushAsync(key, elements);
            return result;
        }

        public static async Task<Int64> RPushXAsync(String key, Object[] elements)
        {
            var result = await _redisClient.RPushXAsync(key, elements);
            return result;
        }

        public static String BLPop(String key, Int32 timeoutSeconds)
        {
            var result = _redisClient.BLPop(key, timeoutSeconds);
            return result;
        }

        public static T BLPop<T>(String key, Int32 timeoutSeconds)
        {
            var result = _redisClient.BLPop<T>(key, timeoutSeconds);
            return result;
        }

        public static KeyValue<String> BLPop(String[] keys, Int32 timeoutSeconds)
        {
            var result = _redisClient.BLPop(keys, timeoutSeconds);
            return result;
        }

        public static KeyValue<T> BLPop<T>(String[] keys, Int32 timeoutSeconds)
        {
            var result = _redisClient.BLPop<T>(keys, timeoutSeconds);
            return result;
        }

        public static String BRPop(String key, Int32 timeoutSeconds)
        {
            var result = _redisClient.BRPop(key, timeoutSeconds);
            return result;
        }

        public static async Task<Dictionary<String, T>> HGetAllAsync<T>(String key)
        {
            var result = await _redisClient.HGetAllAsync<T>(key);
            return result;
        }

        public static async Task<Int64> HIncrByAsync(String key, String field, Int64 increment)
        {
            var result = await _redisClient.HIncrByAsync(key, field, increment);
            return result;
        }

        public static async Task<Decimal> HIncrByFloatAsync(String key, String field, Decimal increment)
        {
            var result = await _redisClient.HIncrByFloatAsync(key, field, increment);
            return result;
        }

        public static async Task<String[]> HKeysAsync(String key)
        {
            var result = await _redisClient.HKeysAsync(key);
            return result;
        }

        public static async Task<Int64> HLenAsync(String key)
        {
            var result = await _redisClient.HLenAsync(key);
            return result;
        }

        public static async Task<String[]> HMGetAsync(String key, String[] fields)
        {
            var result = await _redisClient.HMGetAsync(key, fields);
            return result;
        }

        public static async Task<T[]> HMGetAsync<T>(String key, String[] fields)
        {
            var result = await _redisClient.HMGetAsync<T>(key, fields);
            return result;
        }

        public static async Task HMSetAsync<T>(String key, String field, T value, Object[] fieldValues)
        {
            await _redisClient.HMSetAsync<T>(key, field, value, fieldValues);
        }

        public static async Task HMSetAsync<T>(String key, Dictionary<String, T> keyValues)
        {
            await _redisClient.HMSetAsync<T>(key, keyValues);
        }

        public static async Task<ScanResult<String>> HScanAsync(String key, Int64 cursor, String pattern, Int64 count)
        {
            var result = await _redisClient.HScanAsync(key, cursor, pattern, count);
            return result;
        }

        public static async Task<Int64> HSetAsync<T>(String key, String field, T value, Object[] fieldValues)
        {
            var result = await _redisClient.HSetAsync<T>(key, field, value, fieldValues);
            return result;
        }

        public static async Task<Int64> HSetAsync<T>(String key, Dictionary<String, T> keyValues)
        {
            var result = await _redisClient.HSetAsync<T>(key, keyValues);
            return result;
        }

        public static async Task<Boolean> HSetNxAsync<T>(String key, String field, T value)
        {
            var result = await _redisClient.HSetNxAsync<T>(key, field, value);
            return result;
        }

        public static async Task<Int64> HStrLenAsync(String key, String field)
        {
            var result = await _redisClient.HStrLenAsync(key, field);
            return result;
        }

        public static async Task<String[]> HValsAsync(String key)
        {
            var result = await _redisClient.HValsAsync(key);
            return result;
        }

        public static async Task<T[]> HValsAsync<T>(String key)
        {
            var result = await _redisClient.HValsAsync<T>(key);
            return result;
        }

        public static Int64 HDel(String key, String[] fields)
        {
            var result = _redisClient.HDel(key, fields);
            return result;
        }

        public static Boolean HExists(String key, String field)
        {
            var result = _redisClient.HExists(key, field);
            return result;
        }

        public static String HGet(String key, String field)
        {
            var result = _redisClient.HGet(key, field);
            return result;
        }

        public static T HGet<T>(String key, String field)
        {
            var result = _redisClient.HGet<T>(key, field);
            return result;
        }

        public static Dictionary<String, String> HGetAll(String key)
        {
            var result = _redisClient.HGetAll(key);
            return result;
        }

        public static Dictionary<String, T> HGetAll<T>(String key)
        {
            var result = _redisClient.HGetAll<T>(key);
            return result;
        }

        public static Int64 HIncrBy(String key, String field, Int64 increment)
        {
            var result = _redisClient.HIncrBy(key, field, increment);
            return result;
        }

        public static Decimal HIncrByFloat(String key, String field, Decimal increment)
        {
            var result = _redisClient.HIncrByFloat(key, field, increment);
            return result;
        }

        public static String[] HKeys(String key)
        {
            var result = _redisClient.HKeys(key);
            return result;
        }

        public static Int64 HLen(String key)
        {
            var result = _redisClient.HLen(key);
            return result;
        }

        public static String[] HMGet(String key, String[] fields)
        {
            var result = _redisClient.HMGet(key, fields);
            return result;
        }

        public static T[] HMGet<T>(String key, String[] fields)
        {
            var result = _redisClient.HMGet<T>(key, fields);
            return result;
        }

        public static void HMSet<T>(String key, String field, T value, Object[] fieldValues)
        {
            _redisClient.HMSet<T>(key, field, value, fieldValues);
        }

        public static void HMSet<T>(String key, Dictionary<String, T> keyValues)
        {
            _redisClient.HMSet<T>(key, keyValues);
        }

        public static ScanResult<String> HScan(String key, Int64 cursor, String pattern, Int64 count)
        {
            var result = _redisClient.HScan(key, cursor, pattern, count);
            return result;
        }

        public static Int64 HSet<T>(String key, String field, T value, Object[] fieldValues)
        {
            var result = _redisClient.HSet<T>(key, field, value, fieldValues);
            return result;
        }

        public static Int64 HSet<T>(String key, Dictionary<String, T> keyValues)
        {
            var result = _redisClient.HSet<T>(key, keyValues);
            return result;
        }

        public static Boolean HSetNx<T>(String key, String field, T value)
        {
            var result = _redisClient.HSetNx<T>(key, field, value);
            return result;
        }

        public static Int64 HStrLen(String key, String field)
        {
            var result = _redisClient.HStrLen(key, field);
            return result;
        }

        public static String[] HVals(String key)
        {
            var result = _redisClient.HVals(key);
            return result;
        }

        public static T[] HVals<T>(String key)
        {
            var result = _redisClient.HVals<T>(key);
            return result;
        }

        public static async Task<Boolean> PfAddAsync(String key, Object[] elements)
        {
            var result = await _redisClient.PfAddAsync(key, elements);
            return result;
        }

        public static async Task<Int64> PfCountAsync(params string[] keys)
        {
            var result = await _redisClient.PfCountAsync(keys);
            return result;
        }

        public static async Task PfMergeAsync(String destkey, String[] sourcekeys)
        {
            await _redisClient.PfMergeAsync(destkey, sourcekeys);
        }

        public static Boolean PfAdd(String key, Object[] elements)
        {
            var result = _redisClient.PfAdd(key, elements);
            return result;
        }

        public static Int64 PfCount(params string[] keys)
        {
            var result = _redisClient.PfCount(keys);
            return result;
        }

        public static void PfMerge(String destkey, String[] sourcekeys)
        {
            _redisClient.PfMerge(destkey, sourcekeys);
        }

        public static async Task<Int64> DelAsync(params string[] keys)
        {
            var result = await _redisClient.DelAsync(keys);
            return result;
        }

        public static async Task<Byte[]> DumpAsync(String key)
        {
            var result = await _redisClient.DumpAsync(key);
            return result;
        }

        public static async Task<Boolean> ExistsAsync(String key)
        {
            var result = await _redisClient.ExistsAsync(key);
            return result;
        }

        public static async Task<Int64> ExistsAsync(params string[] keys)
        {
            var result = await _redisClient.ExistsAsync(keys);
            return result;
        }

        public static async Task<Boolean> ExpireAsync(String key, Int32 seconds)
        {
            var result = await _redisClient.ExpireAsync(key, seconds);
            return result;
        }

        public static async Task<Boolean> ExpireAtAsync(String key, DateTime timestamp)
        {
            var result = await _redisClient.ExpireAtAsync(key, timestamp);
            return result;
        }

        public static async Task<String[]> KeysAsync(String pattern)
        {
            var result = await _redisClient.KeysAsync(pattern);
            return result;
        }

        public static async Task MigrateAsync(String host, Int32 port, String key, Int32 destinationDb, Int64 timeoutMilliseconds, Boolean copy, Boolean replace, String authPassword, String auth2Username, String auth2Password, String[] keys)
        {
            await _redisClient.MigrateAsync(host, port, key, destinationDb, timeoutMilliseconds, copy, replace, authPassword, auth2Username, auth2Password, keys);
        }

        public static async Task<Boolean> MoveAsync(String key, Int32 db)
        {
            var result = await _redisClient.MoveAsync(key, db);
            return result;
        }

        public static async Task<Nullable<Int64>> ObjectRefCountAsync(String key)
        {
            var result = await _redisClient.ObjectRefCountAsync(key);
            return result;
        }

        public static async Task<Int64> ObjectIdleTimeAsync(String key)
        {
            var result = await _redisClient.ObjectIdleTimeAsync(key);
            return result;
        }

        public static async Task<String> ObjectEncodingAsync(String key)
        {
            var result = await _redisClient.ObjectEncodingAsync(key);
            return result;
        }

        public static async Task<Nullable<Int64>> ObjectFreqAsync(String key)
        {
            var result = await _redisClient.ObjectFreqAsync(key);
            return result;
        }

        public static async Task<Boolean> PersistAsync(String key)
        {
            var result = await _redisClient.PersistAsync(key);
            return result;
        }

        public static async Task<Boolean> PExpireAsync(String key, Int32 milliseconds)
        {
            var result = await _redisClient.PExpireAsync(key, milliseconds);
            return result;
        }

        public static async Task<Boolean> PExpireAtAsync(String key, DateTime timestamp)
        {
            var result = await _redisClient.PExpireAtAsync(key, timestamp);
            return result;
        }

        public static async Task<Int64> PTtlAsync(String key)
        {
            var result = await _redisClient.PTtlAsync(key);
            return result;
        }

        public static async Task<String> RandomKeyAsync()
        {
            var result = await _redisClient.RandomKeyAsync();
            return result;
        }

        public static async Task RenameAsync(String key, String newkey)
        {
            await _redisClient.RenameAsync(key, newkey);
        }

        public static async Task<Boolean> RenameNxAsync(String key, String newkey)
        {
            var result = await _redisClient.RenameNxAsync(key, newkey);
            return result;
        }

        public static async Task RestoreAsync(String key, Byte[] serializedValue)
        {
            await _redisClient.RestoreAsync(key, serializedValue);
        }

        public static async Task RestoreAsync(String key, Int32 ttl, Byte[] serializedValue, Boolean replace, Boolean absTtl, Nullable<Int32> idleTimeSeconds, Nullable<Decimal> frequency)
        {
            await _redisClient.RestoreAsync(key, ttl, serializedValue, replace, absTtl, idleTimeSeconds, frequency);
        }

        public static async Task<ScanResult<String>> ScanAsync(Int64 cursor, String pattern, Int64 count, String type)
        {
            var result = await _redisClient.ScanAsync(cursor, pattern, count, type);
            return result;
        }

        public static async Task<String[]> SortAsync(String key, String byPattern, Int64 offset, Int64 count, String[] getPatterns, Nullable<Collation> collation, Boolean alpha)
        {
            var result = await _redisClient.SortAsync(key, byPattern, offset, count, getPatterns, collation, alpha);
            return result;
        }

        public static Object Call(CommandPacket cmd)
        {
            var result = _redisClient.Call(cmd);
            return result;
        }

        public static async Task<Object> CallAsync(CommandPacket cmd)
        {
            var result = await _redisClient.CallAsync(cmd);
            return result;
        }

        public static RedisClient.PipelineHook StartPipe()
        {
            var result = _redisClient.StartPipe();
            return result;
        }

        public static RedisClient.DatabaseHook GetDatabase(Nullable<Int32> index)
        {
            var result = _redisClient.GetDatabase(index);
            return result;
        }

        public static RedisClient.TransactionHook Multi()
        {
            var result = _redisClient.Multi();
            return result;
        }

        public static void Auth(String password)
        {
            _redisClient.Auth(password);
        }

        public static void Auth(String username, String password)
        {
            _redisClient.Auth(username, password);
        }

        public static void ClientCaching(Confirm confirm)
        {
            _redisClient.ClientCaching(confirm);
        }

        public static String ClientGetName()
        {
            var result = _redisClient.ClientGetName();
            return result;
        }

        public static Int64 ClientGetRedir()
        {
            var result = _redisClient.ClientGetRedir();
            return result;
        }

        public static Int64 ClientId()
        {
            var result = _redisClient.ClientId();
            return result;
        }

        public static void ClientKill(String ipport)
        {
            _redisClient.ClientKill(ipport);
        }

        public static Int64 ClientKill(String ipport, Nullable<Int64> clientid, Nullable<ClientType> type, String username, String addr, Nullable<Confirm> skipme)
        {
            var result = _redisClient.ClientKill(ipport, clientid, type, username, addr, skipme);
            return result;
        }

        public static String ClientList(Nullable<ClientType> type)
        {
            var result = _redisClient.ClientList(type);
            return result;
        }

        public static void ClientPause(Int64 timeoutMilliseconds)
        {
            _redisClient.ClientPause(timeoutMilliseconds);
        }

        public static void ClientReply(ClientReplyType type)
        {
            _redisClient.ClientReply(type);
        }

        public static void ClientSetName(String connectionName)
        {
            _redisClient.ClientSetName(connectionName);
        }

        public static void ClientTracking(Boolean on_off, Nullable<Int64> redirect, String[] prefix, Boolean bcast, Boolean optin, Boolean optout, Boolean noloop)
        {
            _redisClient.ClientTracking(on_off, redirect, prefix, bcast, optin, optout, noloop);
        }

        public static Boolean ClientUnBlock(Int64 clientid, Nullable<ClientUnBlockType> type)
        {
            var result = _redisClient.ClientUnBlock(clientid, type);
            return result;
        }

        public static String Echo(String message)
        {
            var result = _redisClient.Echo(message);
            return result;
        }

        public static Dictionary<String, Object> Hello(String protover, String username, String password, String clientname)
        {
            var result = _redisClient.Hello(protover, username, password, clientname);
            return result;
        }

        public static String Ping(String message)
        {
            var result = _redisClient.Ping(message);
            return result;
        }

        public static void Quit()
        {
            _redisClient.Quit();
        }

        public static void Select(Int32 index)
        {
            _redisClient.Select(index);
        }

        public static async Task<Int64> GeoAddAsync(String key, GeoMember[] members)
        {
            var result = await _redisClient.GeoAddAsync(key, members);
            return result;
        }

        public static async Task<Nullable<Decimal>> GeoDistAsync(String key, String member1, String member2, GeoUnit unit)
        {
            var result = await _redisClient.GeoDistAsync(key, member1, member2, unit);
            return result;
        }

        public static async Task<String> GeoHashAsync(String key, String member)
        {
            var result = await _redisClient.GeoHashAsync(key, member);
            return result;
        }

        public static async Task<String[]> GeoHashAsync(String key, String[] members)
        {
            var result = await _redisClient.GeoHashAsync(key, members);
            return result;
        }

        public static async Task<GeoMember> GeoPosAsync(String key, String member)
        {
            var result = await _redisClient.GeoPosAsync(key, member);
            return result;
        }

        public static async Task<GeoMember[]> GeoPosAsync(String key, String[] members)
        {
            var result = await _redisClient.GeoPosAsync(key, members);
            return result;
        }

        public static async Task<GeoRadiusResult[]> GeoRadiusAsync(String key, Decimal longitude, Decimal latitude, Decimal radius, GeoUnit unit, Boolean withdoord, Boolean withdist, Boolean withhash, Nullable<Int64> count, Nullable<Collation> collation)
        {
            var result = await _redisClient.GeoRadiusAsync(key, longitude, latitude, radius, unit, withdoord, withdist, withhash, count, collation);
            return result;
        }

        public static async Task<Int64> GeoRadiusStoreAsync(String key, Decimal longitude, Decimal latitude, Decimal radius, GeoUnit unit, Nullable<Int64> count, Nullable<Collation> collation, String storekey, String storedistkey)
        {
            var result = await _redisClient.GeoRadiusStoreAsync(key, longitude, latitude, radius, unit, count, collation, storekey, storedistkey);
            return result;
        }

        public static async Task<GeoRadiusResult[]> GeoRadiusByMemberAsync(String key, String member, Decimal radius, GeoUnit unit, Boolean withdoord, Boolean withdist, Boolean withhash, Nullable<Int64> count, Nullable<Collation> collation)
        {
            var result = await _redisClient.GeoRadiusByMemberAsync(key, member, radius, unit, withdoord, withdist, withhash, count, collation);
            return result;
        }

        public static async Task<Int64> GeoRadiusByMemberStoreAsync(String key, String member, Decimal radius, GeoUnit unit, Nullable<Int64> count, Nullable<Collation> collation, String storekey, String storedistkey)
        {
            var result = await _redisClient.GeoRadiusByMemberStoreAsync(key, member, radius, unit, count, collation, storekey, storedistkey);
            return result;
        }

        public static Int64 GeoAdd(String key, GeoMember[] members)
        {
            var result = _redisClient.GeoAdd(key, members);
            return result;
        }

        public static Nullable<Decimal> GeoDist(String key, String member1, String member2, GeoUnit unit)
        {
            var result = _redisClient.GeoDist(key, member1, member2, unit);
            return result;
        }

        public static String GeoHash(String key, String member)
        {
            var result = _redisClient.GeoHash(key, member);
            return result;
        }

        public static String[] GeoHash(String key, String[] members)
        {
            var result = _redisClient.GeoHash(key, members);
            return result;
        }

        public static GeoMember GeoPos(String key, String member)
        {
            var result = _redisClient.GeoPos(key, member);
            return result;
        }

        public static GeoMember[] GeoPos(String key, String[] members)
        {
            var result = _redisClient.GeoPos(key, members);
            return result;
        }

        public static GeoRadiusResult[] GeoRadius(String key, Decimal longitude, Decimal latitude, Decimal radius, GeoUnit unit, Boolean withdoord, Boolean withdist, Boolean withhash, Nullable<Int64> count, Nullable<Collation> collation)
        {
            var result = _redisClient.GeoRadius(key, longitude, latitude, radius, unit, withdoord, withdist, withhash, count, collation);
            return result;
        }

        public static Int64 GeoRadiusStore(String key, Decimal longitude, Decimal latitude, Decimal radius, GeoUnit unit, Nullable<Int64> count, Nullable<Collation> collation, String storekey, String storedistkey)
        {
            var result = _redisClient.GeoRadiusStore(key, longitude, latitude, radius, unit, count, collation, storekey, storedistkey);
            return result;
        }

        public static GeoRadiusResult[] GeoRadiusByMember(String key, String member, Decimal radius, GeoUnit unit, Boolean withdoord, Boolean withdist, Boolean withhash, Nullable<Int64> count, Nullable<Collation> collation)
        {
            var result = _redisClient.GeoRadiusByMember(key, member, radius, unit, withdoord, withdist, withhash, count, collation);
            return result;
        }

        public static Int64 GeoRadiusByMemberStore(String key, String member, Decimal radius, GeoUnit unit, Nullable<Int64> count, Nullable<Collation> collation, String storekey, String storedistkey)
        {
            var result = _redisClient.GeoRadiusByMemberStore(key, member, radius, unit, count, collation, storekey, storedistkey);
            return result;
        }

        public static async Task<Int64> HDelAsync(String key, String[] fields)
        {
            var result = await _redisClient.HDelAsync(key, fields);
            return result;
        }

        public static async Task<Boolean> HExistsAsync(String key, String field)
        {
            var result = await _redisClient.HExistsAsync(key, field);
            return result;
        }

        public static async Task<String> HGetAsync(String key, String field)
        {
            var result = await _redisClient.HGetAsync(key, field);
            return result;
        }

        public static async Task<T> HGetAsync<T>(String key, String field)
        {
            var result = await _redisClient.HGetAsync<T>(key, field);
            return result;
        }

        public static async Task<Dictionary<String, String>> HGetAllAsync(String key)
        {
            var result = await _redisClient.HGetAllAsync(key);
            return result;
        }

    }
}

