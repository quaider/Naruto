using Naruto.Dependency.Abstraction;
using Naruto.Redis.Providers;
using Naruto.Runtime.Caching.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naruto.Redis
{
    public class RedisService
    {
        private readonly IDatabase _database;
        private readonly IRedisValueSerializer _serializer;

        public RedisService(IRedisDatabaseProvider redisDatabaseProvider, IRedisValueSerializer redisValueSerializer)
        {
            _database = redisDatabaseProvider.GetDatabase();
            _serializer = redisValueSerializer;
        }

        #region String

        public void StringSet(string key, object value, TimeSpan? expiry = null)
        {
            _database.StringSet(key, _serializer.Serialize(value, value.GetType()), expiry);
        }

        public void StringSet<T>(string key, T value, TimeSpan? expiry = null)
        {
            _database.StringSet(key, _serializer.Serialize(value, value.GetType()), expiry);
        }

        public object StringGet(string key)
        {
            var objbyte = _database.StringGet(key);
            return objbyte.HasValue ? _serializer.Deserialize(objbyte) : null;
        }

        public T StringGetOrDefault<T>(string key)
        {
            var objbyte = _database.StringGet(key);
            return objbyte.HasValue ? _serializer.Deserialize<T>(objbyte) : default(T);
        }

        #region Increment and Decrement

        public double StringIncrement(string key, double val = 1)
        {
            return _database.StringIncrement(key, val);
        }

        public long StringIncrement(string key, long val = 1)
        {
            return _database.StringIncrement(key, val);
        }

        public double StringDecrement(string key, double val = 1)
        {
            return _database.StringDecrement(key, val);
        }

        public long StringDecrement(string key, long val = 1)
        {
            return _database.StringDecrement(key, val);
        }

        #endregion

        #endregion

        #region List

        /// <summary>
        /// 移除指定List且指定值的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">list name</param>
        /// <param name="value">list value</param>
        public void ListRemove<T>(string key, T value)
        {
            _database.ListRemove(key, _serializer.Serialize(value, typeof(T)));
        }

        public List<T> ListRange<T>(string key, long start, long stop)
        {
            var values = _database.ListRange(key, start, stop);
            return values.Select(f => _serializer.Deserialize<T>(f)).ToList();
        }

        /// <summary>
        /// 模拟队列
        /// </summary>
        public void ListLeftPush<T>(string key, T value)
        {
            _database.ListLeftPush(key, _serializer.Serialize(value, typeof(T)));
        }

        public T ListLeftPop<T>(string key)
        {
            var value = _database.ListLeftPop(key);

            return _serializer.Deserialize<T>(value);
        }

        /// <summary>
        /// 模拟栈
        /// </summary>
        public void ListRightPush<T>(string key, T value)
        {
            _database.ListRightPush(key, _serializer.Serialize(value, typeof(T)));
        }

        public T ListRightPop<T>(string key)
        {
            var value = _database.ListRightPop(key);

            return _serializer.Deserialize<T>(value);
        }

        public long ListLength(string key)
        {
            return _database.ListLength(key);
        }

        #endregion

        #region Hash

        public bool HashExists(string key, string field)
        {
            return _database.HashExists(key, field);
        }

        public void HashSet<T>(string key, string field, T value)
        {
            _database.HashSet(key, field, _serializer.Serialize(value, typeof(T)));
        }

        public T HashGet<T>(string key, string field)
        {
            var value = _database.HashGet(key, field);

            return _serializer.Deserialize<T>(value);
        }

        public List<T> HashGetAll<T>(string key)
        {
            var list = new List<T>();
            var entries = _database.HashGetAll(key);
            foreach (var entry in entries)
            {
                list.Add(_serializer.Deserialize<T>(entry.Value));
            }

            return list;
        }

        public double HashIncrement(string key, string field, double val = 1)
        {
            return _database.HashIncrement(key, field, val);
        }

        public long HashIncrement(string key, string field, long val = 1)
        {
            return _database.HashIncrement(key, field, val);
        }

        public double HashDecrement(string key, string field, double val = 1)
        {
            return _database.HashDecrement(key, field, val);
        }

        public long HashDecrement(string key, string field, long val = 1)
        {
            return _database.HashDecrement(key, field, val);
        }

        public void HashDelete(string key, string field)
        {
            _database.HashDelete(key, field);
        }

        public void HashDelete(string key, params string[] fields)
        {
            foreach (var field in fields)
            {
                _database.HashDelete(key, field);
            }
        }

        #endregion

        #region Sorted Set

        public void SortedSetAdd<T>(string key, T value, double score)
        {
            _database.SortedSetAdd(key, _serializer.Serialize(value, typeof(T)), score);
        }

        public void SortedSetRemove<T>(string key, T value)
        {
            _database.SortedSetRemove(key, _serializer.Serialize(value, typeof(T)));
        }

        public List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1)
        {
            var values = _database.SortedSetRangeByRank(key, start, stop);

            return values.Select(f => _serializer.Deserialize<T>(f)).ToList();
        }

        public long SortedSetLength(string key)
        {
            return _database.SortedSetLength(key);
        }

        #endregion

        #region Key

        public void Remove(string key)
        {
            _database.KeyDelete(key);
        }

        public bool KeyExists(string key)
        {
            return _database.KeyExists(key);
        }

        public void KeyDeleteWithPrefix(string prefix)
        {
            _database.KeyDeleteWithPrefix(prefix);
        }

        public int KeyCount(string prefix)
        {
            return _database.KeyCount(prefix);
        }

        public void Clear()
        {
            _database.KeyDeleteWithPrefix("*");
        }

        #endregion

        #region Helpers

        public virtual string Serialize(object value, Type type)
        {
            return _serializer.Serialize(value, type);
        }

        public virtual object Deserialize(RedisValue objbyte)
        {
            return _serializer.Deserialize(objbyte);
        }

        #endregion
    }
}
