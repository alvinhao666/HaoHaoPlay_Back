using FreeRedis;
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

        public static void AddClient(RedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public static void CreateInstance(ConnectionStringBuilder sentinelConnectionString, string[] sentinels,
            bool rw_splitting)
        {
            _redisClient = new RedisClient(sentinelConnectionString, sentinels, rw_splitting);
        }

        public static void CreateInstance(ConnectionStringBuilder[] connectionStrings,
            Func<string, string> redirectRule)
        {
            _redisClient = new RedisClient(connectionStrings, redirectRule);
        }

        public static void CreateInstance(params ConnectionStringBuilder[] clusterConnectionStrings)
        {
            _redisClient = new RedisClient(clusterConnectionStrings);
        }

        public static void CreateInstance(ConnectionStringBuilder connectionString,
            params ConnectionStringBuilder[] slaveConnectionStrings)
        {
            _redisClient = new RedisClient(connectionString, slaveConnectionStrings);
        }


        public static String Set<T>(String key, T value, TimeSpan timeout, Boolean keepTtl, Boolean nx, Boolean xx,
            Boolean get)
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

        public static ZMember[] ZRevRangeByScoreWithScores(String key, Decimal max, Decimal min, Int32 offset,
            Int32 count)
        {
            var result = _redisClient.ZRevRangeByScoreWithScores(key, max, min, offset, count);
            return result;
        }

        public static ZMember[] ZRevRangeByScoreWithScores(String key, String max, String min, Int32 offset,
            Int32 count)
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

        public static Int64 ZUnionStore(String destination, String[] keys, Int32[] weights,
            Nullable<ZAggregate> aggregate)
        {
            var result = _redisClient.ZUnionStore(destination, keys, weights, aggregate);
            return result;
        }

        public static Int64 XAck(String key, String group, params String[] id)
        {
            var result = _redisClient.XAck(key, group, id);
            return result;
        }

        public static String XAdd<T>(String key, String field, T value, params Object[] fieldValues)
        {
            var result = _redisClient.XAdd<T>(key, field, value, fieldValues);
            return result;
        }

        public static String XAdd<T>(String key, Int64 maxlen, String id, String field, T value,
            params Object[] fieldValues)
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

        public static StreamsEntry[] XClaim(String key, String group, String consumer, Int64 minIdleTime,
            params String[] id)
        {
            var result = _redisClient.XClaim(key, group, consumer, minIdleTime, id);
            return result;
        }

        public static StreamsEntry[] XClaim(String key, String group, String consumer, Int64 minIdleTime, String[] id,
            Int64 idle, Int64 retryCount, Boolean force)
        {
            var result = _redisClient.XClaim(key, group, consumer, minIdleTime, id, idle, retryCount, force);
            return result;
        }

        public static String[] XClaimJustId(String key, String group, String consumer, Int64 minIdleTime,
            params String[] id)
        {
            var result = _redisClient.XClaimJustId(key, group, consumer, minIdleTime, id);
            return result;
        }

        public static String[] XClaimJustId(String key, String group, String consumer, Int64 minIdleTime, String[] id,
            Int64 idle, Int64 retryCount, Boolean force)
        {
            var result = _redisClient.XClaimJustId(key, group, consumer, minIdleTime, id, idle, retryCount, force);
            return result;
        }

        public static Int64 XDel(String key, params String[] id)
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

        public static StreamsXPendingConsumerResult[] XPending(String key, String group, String start, String end,
            Int64 count, String consumer)
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

        public static StreamsEntryResult[] XRead(Int64 count, Int64 block, String key, String id,
            params String[] keyIds)
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

        public static StreamsEntryResult[] XReadGroup(String group, String consumer, Int64 count, Int64 block,
            Boolean noack, String key, String id, params String[] keyIds)
        {
            var result = _redisClient.XReadGroup(group, consumer, count, block, noack, key, id, keyIds);
            return result;
        }

        public static StreamsEntryResult[] XReadGroup(String group, String consumer, Int64 count, Int64 block,
            Boolean noack, Dictionary<String, String> keyIds)
        {
            var result = _redisClient.XReadGroup(group, consumer, count, block, noack, keyIds);
            return result;
        }

        public static Int64 XTrim(String key, Int64 count)
        {
            var result = _redisClient.XTrim(key, count);
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

        public static Int64 BitOp(BitOpOperation operation, String destkey, params String[] keys)
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

        public static String[] MGet(String[] keys)
        {
            var result = _redisClient.MGet(keys);
            return result;
        }

        public static T[] MGet<T>(String[] keys)
        {
            var result = _redisClient.MGet<T>(keys);
            return result;
        }

        public static void MSet(String key, Object value, params Object[] keyValues)
        {
            _redisClient.MSet(key, value, keyValues);
        }

        public static void MSet<T>(Dictionary<String, T> keyValues)
        {
            _redisClient.MSet<T>(keyValues);
        }

        public static Boolean MSetNx(String key, Object value, params Object[] keyValues)
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

        public static Object SlowLog(String subcommand, params String[] argument)
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

        public static Int64 SAdd(String key, params Object[] members)
        {
            var result = _redisClient.SAdd(key, members);
            return result;
        }

        public static Int64 SCard(String key)
        {
            var result = _redisClient.SCard(key);
            return result;
        }

        public static String[] SDiff(params String[] keys)
        {
            var result = _redisClient.SDiff(keys);
            return result;
        }

        public static T[] SDiff<T>(params String[] keys)
        {
            var result = _redisClient.SDiff<T>(keys);
            return result;
        }

        public static Int64 SDiffStore(String destination, params String[] keys)
        {
            var result = _redisClient.SDiffStore(destination, keys);
            return result;
        }

        public static String[] SInter(params String[] keys)
        {
            var result = _redisClient.SInter(keys);
            return result;
        }

        public static T[] SInter<T>(params String[] keys)
        {
            var result = _redisClient.SInter<T>(keys);
            return result;
        }

        public static Int64 SInterStore(String destination, params String[] keys)
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

        public static Boolean[] SMIsMember<T>(String key, params Object[] members)
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

        public static String[] SUnion(params String[] keys)
        {
            var result = _redisClient.SUnion(keys);
            return result;
        }

        public static T[] SUnion<T>(params String[] keys)
        {
            var result = _redisClient.SUnion<T>(keys);
            return result;
        }

        public static Int64 SUnionStore(String destination, params String[] keys)
        {
            var result = _redisClient.SUnionStore(destination, keys);
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

        public static Int64 ZAdd(String key, Decimal score, String member, params Object[] scoreMembers)
        {
            var result = _redisClient.ZAdd(key, score, member, scoreMembers);
            return result;
        }

        public static Int64 ZAdd(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = _redisClient.ZAdd(key, scoreMembers, than, ch);
            return result;
        }

        public static Int64 ZAddNx(String key, Decimal score, String member, params Object[] scoreMembers)
        {
            var result = _redisClient.ZAddNx(key, score, member, scoreMembers);
            return result;
        }

        public static Int64 ZAddNx(String key, ZMember[] scoreMembers, Nullable<ZAddThan> than, Boolean ch)
        {
            var result = _redisClient.ZAddNx(key, scoreMembers, than, ch);
            return result;
        }

        public static Int64 ZAddXx(String key, Decimal score, String member, params Object[] scoreMembers)
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

        public static Int64 ZInterStore(String destination, String[] keys, Int32[] weights,
            Nullable<ZAggregate> aggregate)
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

        public static Int64 ZRem(String key, params String[] members)
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

        public static String BfReserve(String key, Decimal errorRate, Int64 capacity, Int32 expansion,
            Boolean nonScaling)
        {
            var result = _redisClient.BfReserve(key, errorRate, capacity, expansion, nonScaling);
            return result;
        }

        public static Boolean BfAdd(String key, String item)
        {
            var result = _redisClient.BfAdd(key, item);
            return result;
        }

        public static Boolean[] BfMAdd(String key, params String[] items)
        {
            var result = _redisClient.BfMAdd(key, items);
            return result;
        }

        public static String BfInsert(String key, String[] items, Nullable<Int64> capacity, String error,
            Int32 expansion, Boolean noCreate, Boolean nonScaling)
        {
            var result = _redisClient.BfInsert(key, items, capacity, error, expansion, noCreate, nonScaling);
            return result;
        }

        public static Boolean BfExists(String key, String item)
        {
            var result = _redisClient.BfExists(key, item);
            return result;
        }

        public static Boolean[] BfMExists(String key, params String[] items)
        {
            var result = _redisClient.BfMExists(key, items);
            return result;
        }

        public static ScanResult<Byte[]> BfScanDump(String key, Int64 iter)
        {
            var result = _redisClient.BfScanDump(key, iter);
            return result;
        }

        public static String BfLoadChunk(String key, Int64 iter, params Byte[] data)
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

        public static Int64[] CmsQuery(String key, params String[] items)
        {
            var result = _redisClient.CmsQuery(key, items);
            return result;
        }

        public static String CmsMerge(String dest, Int64 numKeys, String[] src, params Int64[] weights)
        {
            var result = _redisClient.CmsMerge(dest, numKeys, src, weights);
            return result;
        }

        public static Dictionary<String, String> CmsInfo(String key)
        {
            var result = _redisClient.CmsInfo(key);
            return result;
        }

        public static String CfReserve(String key, Int64 capacity, Nullable<Int64> bucketSize,
            Nullable<Int64> maxIterations, Nullable<Int32> expansion)
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

        public static String CfLoadChunk(String key, Int64 iter, params Byte[] data)
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

        public static String[] TopkAdd(String key, params String[] items)
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

        public static Boolean[] TopkQuery(String key, params String[] items)
        {
            var result = _redisClient.TopkQuery(key, items);
            return result;
        }

        public static Int64[] TopkCount(String key, params String[] items)
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

        public static Int64[] PubSubNumSub(params String[] channels)
        {
            var result = _redisClient.PubSubNumSub(channels);
            return result;
        }

        public static Int64 PubSubNumPat(String message)
        {
            var result = _redisClient.PubSubNumPat(message);
            return result;
        }

        public static void PUnSubscribe(params String[] pattern)
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

        public static void UnSubscribe(params String[] channels)
        {
            _redisClient.UnSubscribe(channels);
        }

        public static Object Eval(String script, String[] keys, params Object[] arguments)
        {
            var result = _redisClient.Eval(script, keys, arguments);
            return result;
        }

        public static Object EvalSha(String sha1, String[] keys, params Object[] arguments)
        {
            var result = _redisClient.EvalSha(sha1, keys, arguments);
            return result;
        }

        public static Boolean ScriptExists(String sha1)
        {
            var result = _redisClient.ScriptExists(sha1);
            return result;
        }

        public static Boolean[] ScriptExists(params String[] sha1)
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

        public static Int64 AclDelUser(params String[] username)
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

        public static void AclSetUser(String username, params String[] rule)
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

        public static String[] CommandGetKeys(params String[] command)
        {
            var result = _redisClient.CommandGetKeys(command);
            return result;
        }

        public static Object[] CommandInfo(params String[] command)
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

        public static Boolean PfAdd(String key, params Object[] elements)
        {
            var result = _redisClient.PfAdd(key, elements);
            return result;
        }

        public static Int64 PfCount(params String[] keys)
        {
            var result = _redisClient.PfCount(keys);
            return result;
        }

        public static void PfMerge(String destkey, params String[] sourcekeys)
        {
            _redisClient.PfMerge(destkey, sourcekeys);
        }

        public static Int64 Del(params String[] keys)
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

        public static Int64 Exists(params String[] keys)
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

        public static void Migrate(String host, Int32 port, String key, Int32 destinationDb, Int64 timeoutMilliseconds,
            Boolean copy, Boolean replace, String authPassword, String auth2Username, String auth2Password,
            params String[] keys)
        {
            _redisClient.Migrate(host, port, key, destinationDb, timeoutMilliseconds, copy, replace, authPassword,
                auth2Username, auth2Password, keys);
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

        public static void Restore(String key, params Byte[] serializedValue)
        {
            _redisClient.Restore(key, serializedValue);
        }

        public static void Restore(String key, Int32 ttl, Byte[] serializedValue, Boolean replace, Boolean absTtl,
            Nullable<Int32> idleTimeSeconds, Nullable<Decimal> frequency)
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

        public static String[] Sort(String key, String byPattern, Int64 offset, Int64 count, String[] getPatterns,
            Nullable<Collation> collation, Boolean alpha)
        {
            var result = _redisClient.Sort(key, byPattern, offset, count, getPatterns, collation, alpha);
            return result;
        }

        public static Int64 SortStore(String storeDestination, String key, String byPattern, Int64 offset, Int64 count,
            String[] getPatterns, Nullable<Collation> collation, Boolean alpha)
        {
            var result = _redisClient.SortStore(storeDestination, key, byPattern, offset, count, getPatterns, collation,
                alpha);
            return result;
        }

        public static Int64 Touch(params String[] keys)
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

        public static Int64 UnLink(params String[] keys)
        {
            var result = _redisClient.UnLink(keys);
            return result;
        }

        public static Int64 Wait(Int64 numreplicas, Int64 timeoutMilliseconds)
        {
            var result = _redisClient.Wait(numreplicas, timeoutMilliseconds);
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

        public static Int64 LPush(String key, params Object[] elements)
        {
            var result = _redisClient.LPush(key, elements);
            return result;
        }

        public static Int64 LPushX(String key, params Object[] elements)
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

        public static Int64 RPush(String key, params Object[] elements)
        {
            var result = _redisClient.RPush(key, elements);
            return result;
        }

        public static Int64 RPushX(String key, params Object[] elements)
        {
            var result = _redisClient.RPushX(key, elements);
            return result;
        }

        public static Object Call(CommandPacket cmd)
        {
            var result = _redisClient.Call(cmd);
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

        public static Int64 ClientKill(String ipport, Nullable<Int64> clientid, Nullable<ClientType> type,
            String username, String addr, Nullable<Confirm> skipme)
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

        public static void ClientTracking(Boolean on_off, Nullable<Int64> redirect, String[] prefix, Boolean bcast,
            Boolean optin, Boolean optout, Boolean noloop)
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

        public static Dictionary<String, Object> Hello(String protover, String username, String password,
            String clientname)
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

        public static Int64 GeoAdd(String key, params GeoMember[] members)
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

        public static String[] GeoHash(String key, params String[] members)
        {
            var result = _redisClient.GeoHash(key, members);
            return result;
        }

        public static GeoMember GeoPos(String key, String member)
        {
            var result = _redisClient.GeoPos(key, member);
            return result;
        }

        public static GeoMember[] GeoPos(String key, params String[] members)
        {
            var result = _redisClient.GeoPos(key, members);
            return result;
        }

        public static GeoRadiusResult[] GeoRadius(String key, Decimal longitude, Decimal latitude, Decimal radius,
            GeoUnit unit, Boolean withdoord, Boolean withdist, Boolean withhash, Nullable<Int64> count,
            Nullable<Collation> collation)
        {
            var result = _redisClient.GeoRadius(key, longitude, latitude, radius, unit, withdoord, withdist, withhash,
                count, collation);
            return result;
        }

        public static Int64 GeoRadiusStore(String key, Decimal longitude, Decimal latitude, Decimal radius,
            GeoUnit unit, Nullable<Int64> count, Nullable<Collation> collation, String storekey, String storedistkey)
        {
            var result = _redisClient.GeoRadiusStore(key, longitude, latitude, radius, unit, count, collation, storekey,
                storedistkey);
            return result;
        }

        public static GeoRadiusResult[] GeoRadiusByMember(String key, String member, Decimal radius, GeoUnit unit,
            Boolean withdoord, Boolean withdist, Boolean withhash, Nullable<Int64> count, Nullable<Collation> collation)
        {
            var result = _redisClient.GeoRadiusByMember(key, member, radius, unit, withdoord, withdist, withhash, count,
                collation);
            return result;
        }

        public static Int64 GeoRadiusByMemberStore(String key, String member, Decimal radius, GeoUnit unit,
            Nullable<Int64> count, Nullable<Collation> collation, String storekey, String storedistkey)
        {
            var result =
                _redisClient.GeoRadiusByMemberStore(key, member, radius, unit, count, collation, storekey,
                    storedistkey);
            return result;
        }

        public static Int64 HDel(String key, params String[] fields)
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

        public static String[] HMGet(String key, params String[] fields)
        {
            var result = _redisClient.HMGet(key, fields);
            return result;
        }

        public static T[] HMGet<T>(String key, params String[] fields)
        {
            var result = _redisClient.HMGet<T>(key, fields);
            return result;
        }

        public static void HMSet<T>(String key, String field, T value, params Object[] fieldValues)
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

        public static Int64 HSet<T>(String key, String field, T value, params Object[] fieldValues)
        {
            var result = _redisClient.HSet<T>(key, field, value, fieldValues);
            return result;
        }

        public static Int64 HSet<T>(String key, Dictionary<String, T> keyValues)
        {
            var result = _redisClient.HSet<T>(key, keyValues);
            return result;
        }
    }
}