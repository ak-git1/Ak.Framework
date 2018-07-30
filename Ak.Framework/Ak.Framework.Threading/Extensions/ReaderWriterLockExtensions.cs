using System;
using System.Threading;

namespace Ak.Framework.Threading.Extensions
{
    /// <summary>
    /// Расширения для блокировщика доступа к ресурсам
    /// </summary>
    public static class ReaderWriterLockExtensions
    {
        public static void ExecuteWithReadLock(this ReaderWriterLockSlim rwlock, Action action)
        {
            rwlock.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                rwlock.ExitReadLock();
            }
        }

        public static TResult ExecuteWithReadLock<TResult>(this ReaderWriterLockSlim rwlock, Func<TResult> func)
        {
            TResult result = default(TResult);
            rwlock.ExecuteWithReadLock(delegate {
                result = func();
            });
            return result;
        }

        public static void ExecuteWithUpgradeableReadLock(this ReaderWriterLockSlim rwlock, Action action)
        {
            rwlock.EnterUpgradeableReadLock();
            try
            {
                action();
            }
            finally
            {
                rwlock.ExitUpgradeableReadLock();
            }
        }

        public static TResult ExecuteWithUpgradeableReadLock<TResult>(this ReaderWriterLockSlim rwlock, Func<TResult> func)
        {
            TResult local;
            rwlock.EnterUpgradeableReadLock();
            try
            {
                local = func();
            }
            finally
            {
                rwlock.ExitUpgradeableReadLock();
            }
            return local;
        }

        public static void ExecuteWithUpgradeableReadLock(this ReaderWriterLockSlim rwlock, Func<bool> readLockedFunc, Action writerAction)
        {
            rwlock.EnterUpgradeableReadLock();
            try
            {
                if (readLockedFunc())
                {
                    try
                    {
                        rwlock.EnterWriteLock();
                        writerAction();
                    }
                    finally
                    {
                        rwlock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                rwlock.ExitUpgradeableReadLock();
            }
        }

        public static void ExecuteWithWriteLock(this ReaderWriterLockSlim rwlock, Action action)
        {
            rwlock.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        public static TResult ExecuteWithWriteLock<TResult>(this ReaderWriterLockSlim rwlock, Func<TResult> func)
        {
            TResult result = default(TResult);
            rwlock.ExecuteWithWriteLock(delegate {
                result = func();
            });
            return result;
        }
    }
}
