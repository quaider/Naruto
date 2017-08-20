using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Naruto.Runtime.Caching
{
    /// <summary>
    /// 缓存实现基类
    /// </summary>
    public abstract class CacheBase : ICache
    {
        public string Name { get; }

        public TimeSpan DefaultSlidingExpireTime { get; set; }

        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        protected readonly object SyncObj = new object();

        /// <summary>
        /// 异步同步对象(使用Async and Task Helpers 可以在同步块如lock{}中使用异步调用)
        /// </summary>
        private readonly AsyncLock _asyncLock = new AsyncLock();

        //todo 添加日志记录


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">缓存名称</param>
        public CacheBase(string name)
        {
            Name = name;
            DefaultSlidingExpireTime = TimeSpan.FromHours(1);
        }

        public virtual object Get(string key, Func<string, object> factroy)
        {
            object item = null;

            try
            {
                item = GetOrDefault(key);
            }
            catch (Exception ex)
            {
                //todo log
            }

            if (item == null)
            {
                lock (SyncObj)
                {
                    try
                    {
                        item = GetOrDefault(key);
                    }
                    catch (Exception ex)
                    {
                        //todo log
                    }

                    if (item == null)
                    {
                        item = factroy(key);

                        if (item == null) return null;

                        try
                        {
                            Set(key, item);
                        }
                        catch (Exception ex)
                        {
                            //todo log
                        }
                    }
                }
            }

            return item;
        }

        public virtual async Task<object> GetAsync(string key, Func<string, Task<object>> factory)
        {
            object item = null;
            try
            {
                item = await GetOrDefaultAsync(key);
            }
            catch (Exception ex)
            {

            }

            if (item == null)
            {
                using (await _asyncLock.LockAsync())
                {
                    try
                    {
                        item = await GetOrDefaultAsync(key);
                    }
                    catch (Exception ex)
                    {

                    }

                    if (item == null)
                    {
                        item = await factory(key);

                        if (item == null) return null;

                        try
                        {
                            await SetAsync(key, item);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return item;
        }

        public abstract object GetOrDefault(string key);

        public virtual Task<object> GetOrDefaultAsync(string key)
        {
            return Task.FromResult(GetOrDefault(key));
        }

        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = default(TimeSpan?), TimeSpan? absoluteExpireTime = default(TimeSpan?));

        public virtual Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = default(TimeSpan?), TimeSpan? absoluteExpireTime = default(TimeSpan?))
        {
            Set(key, value, slidingExpireTime, absoluteExpireTime);
            return Task.FromResult(0);
        }

        public abstract void Remove(string key);

        public virtual Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.FromResult(0);
        }

        public abstract void Clear();

        public Task ClearAsync()
        {
            Clear();
            return Task.FromResult(0);
        }

        public virtual void Dispose()
        {

        }
    }
}
