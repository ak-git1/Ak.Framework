using System.Threading;
using System.Windows.Threading;

namespace Ak.Framework.Threading
{
    /// <summary>
    /// Поток в диспетчере
    /// </summary>
    public class DispatcherThread
    {
        #region Переменные и константы

        /// <summary>
        /// Диспетчер потоков
        /// </summary>
        private Dispatcher _dispatcher;

        /// <summary>
        /// Объект-блокировщик
        /// </summary>
        private readonly object _dispatcherLock;

        #endregion

        #region Свойства

        /// <summary>
        /// Диспетчер
        /// </summary>
        public Dispatcher Dispatcher
        {
            get
            {
                lock (_dispatcherLock)
                {
                    return _dispatcher;
                }
            }
        }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="isBackgroundThread">Поток работает в фоне</param>
        public DispatcherThread(bool isBackgroundThread = true)
        {
            _dispatcherLock = new object();

            ThreadStart start = delegate
            {
                lock (_dispatcherLock)
                {
                    _dispatcher = Dispatcher.CurrentDispatcher;
                }
                Dispatcher.Run();
            };

            Thread thread2 = new Thread(start)
            {
                IsBackground = isBackgroundThread
            };

            thread2.Start();

            while (Dispatcher == null)
                Thread.Sleep(10);
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Деструктор
        /// </summary>
        public void Dispose()
        {
            _dispatcher.InvokeShutdown();
        }

        #endregion
    }
}
