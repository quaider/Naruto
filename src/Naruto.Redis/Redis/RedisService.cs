using Naruto.Redis.Providers;
using Naruto.Serializer;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naruto.Redis
{
    public class RedisService
    {
        private readonly IRedisDatabaseProvider _provider;
        private readonly Lazy<IDatabase> _database;

        public RedisService(IRedisDatabaseProvider redisDatabaseProvider)
        {
            _provider = redisDatabaseProvider;
            _database = new Lazy<IDatabase>(GetDataBase);
        }

        IDatabase GetDataBase()
        {
            return _provider.GetDatabase();
        }

        #region String

        public void StringSet(string key, object value, TimeSpan? expiry = null)
        {
            _database.Value.StringSet(key, JsonSerialization.Serialize(value), expiry);
        }

        public void StringSet<T>(string key, T value, TimeSpan? expiry = null)
        {
            _database.Value.StringSet(key, JsonSerialization.Serialize(value), expiry);
        }

        public object StringGet(string key)
        {
            var objbyte = _database.Value.StringGet(key);
            return objbyte.HasValue ? JsonSerialization.Deserialize(objbyte) : null;
        }

        public T StringGetOrDefault<T>(string key)
        {
            var objbyte = _database.Value.StringGet(key);
            return objbyte.HasValue ? JsonSerialization.Deserialize<T>(objbyte) : default(T);
        }

        #region Increment and Decrement

        public double StringIncrement(string key, double val = 1)
        {
            return _database.Value.StringIncrement(key, val);
        }

        public long StringIncrement(string key, long val = 1)
        {
            return _database.Value.StringIncrement(key, val);
        }

        public double StringDecrement(string key, double val = 1)
        {
            return _database.Value.StringDecrement(key, val);
        }

        public long StringDecrement(string key, long val = 1)
        {
            return _database.Value.StringDecrement(key, val);
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
            _database.Value.ListRemove(key, JsonSerialization.Serialize(value));
        }

        public List<T> ListRange<T>(string key, long start, long stop)
        {
            var values = _database.Value.ListRange(key, start, stop);
            return values.Select(f => JsonSerialization.Deserialize<T>(f)).ToList();
        }

        /// <summary>
        /// 模拟队列
        /// </summary>
        public void ListLeftPush<T>(string key, T value)
        {
            _database.Value.ListLeftPush(key, JsonSerialization.Serialize(value));
        }

        public T ListLeftPop<T>(string key)
        {
            var value = _database.Value.ListLeftPop(key);

            return JsonSerialization.Deserialize<T>(value);
        }

        /// <summary>
        /// 模拟栈
        /// </summary>
        public void ListRightPush<T>(string key, T value)
        {
            _database.Value.ListRightPush(key, JsonSerialization.Serialize(value));
        }

        public T ListRightPop<T>(string key)
        {
            var value = _database.Value.ListRightPop(key);

            return JsonSerialization.Deserialize<T>(value);
        }

        public long ListLength(string key)
        {
            return _database.Value.ListLength(key);
        }

        #endregion

        #region Hash

        public bool HashExists(string key, string field)
        {
            return _database.Value.HashExists(key, field);
        }

        public void HashSet<T>(string key, string field, T value)
        {
            _database.Value.HashSet(key, field, JsonSerialization.Serialize(value));
        }

        public T HashGet<T>(string key, string field)
        {
            var value = _database.Value.HashGet(key, field);

            return JsonSerialization.Deserialize<T>(value);
        }

        public List<T> HashGetAll<T>(string key)
        {
            var list = new List<T>();
            var entries = _database.Value.HashGetAll(key);
            foreach (var entry in entries)
            {
                list.Add(JsonSerialization.Deserialize<T>(entry.Value));
            }

            return list;
        }

        public double HashIncrement(string key, string field, double val = 1)
        {
            return _database.Value.HashIncrement(key, field, val);
        }

        public long HashIncrement(string key, string field, long val = 1)
        {
            return _database.Value.HashIncrement(key, field, val);
        }

        public double HashDecrement(string key, string field, double val = 1)
        {
            return _database.Value.HashDecrement(key, field, val);
        }

        public long HashDecrement(string key, string field, long val = 1)
        {
            return _database.Value.HashDecrement(key, field, val);
        }

        public void HashDelete(string key, string field)
        {
            _database.Value.HashDelete(key, field);
        }

        public void HashDelete(string key, params string[] fields)
        {
            foreach (var field in fields)
            {
                _database.Value.HashDelete(key, field);
            }
        }

        #endregion

        #region Sorted Set

        public void SortedSetAdd<T>(string key, T value, double score)
        {
            _database.Value.SortedSetAdd(key, JsonSerialization.Serialize(value), score);
        }

        public void SortedSetRemove<T>(string key, T value)
        {
            _database.Value.SortedSetRemove(key, JsonSerialization.Serialize(value));
        }

        public List<T> SortedSetRangeByRank<T>(string key, long start = 0, long stop = -1)
        {
            var values = _database.Value.SortedSetRangeByRank(key, start, stop);

            return values.Select(f => JsonSerialization.Deserialize<T>(f)).ToList();
        }

        public long SortedSetLength(string key)
        {
            return _database.Value.SortedSetLength(key);
        }

        #endregion

        #region Key

        public void Remove(string key)
        {
            _database.Value.KeyDelete(key);
        }

        public bool KeyExists(string key)
        {
            return _database.Value.KeyExists(key);
        }

        public void KeyDeleteWithPrefix(string prefix)
        {
            _database.Value.KeyDeleteWithPrefix(prefix);
        }

        public int KeyCount(string prefix)
        {
            return _database.Value.KeyCount(prefix);
        }

        public void Clear()
        {
            _database.Value.KeyDeleteWithPrefix("*");
        }

        #endregion
    }
}
