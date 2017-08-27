using System;
using System.Threading;

namespace Naruto.Resources
{
    /// <summary>
    /// 提供针对资源的写独占锁 <see cref="ReaderWriterLockSlim"/>
    /// </summary>
    public class ResourceWriteLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        public ResourceWriteLock(ReaderWriterLockSlim lockSlim)
        {
            _rwLock = lockSlim;
            _rwLock.EnterWriteLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }
    }
}
