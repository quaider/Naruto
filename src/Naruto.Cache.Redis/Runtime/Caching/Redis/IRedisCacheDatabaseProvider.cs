using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Runtime.Caching.Redis
{
    /// <summary>
    /// 用于获取 <see cref="IDatabase"/>
    /// </summary>
    public interface IRedisCacheDatabaseProvider
    {
        /// <summary>
        /// <see cref="IDatabase"/>
        /// </summary>
        IDatabase GetDatabase();
    }
}
