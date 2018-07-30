using System;
using System.Diagnostics;
using System.Threading;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Threading
{
    /// <summary>
    /// Фабрика для CanellationToken
    /// </summary>
    public static class CancellationTokenFactory
    {
        /// <summary>
        /// Обновление CanellationToken
        /// </summary>
        /// <param name="cancellation">CancellationTokenSource</param>
        /// <param name="syncLock">Блокировщик</param>
        /// <returns></returns>
        public static CancellationToken UpdateToken(ref CancellationTokenSource cancellation, object syncLock)
        {
            lock (syncLock)
            {
                CancellationTokenSource source;
                try
                {
                    cancellation.IfNotNull(delegate (CancellationTokenSource c) {
                        c.Cancel();
                        c.Dispose();
                    });
                }
                catch (Exception exception)
                {
                    Debug.WriteLine($"Failed to dispose current cancellation. Reason: {exception.Message}");
                }
                cancellation = source = new CancellationTokenSource();
                return source.Token;
            }
        }
    }
}
