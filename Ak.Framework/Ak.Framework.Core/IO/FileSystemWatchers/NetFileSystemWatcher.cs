using System;
using System.IO;
using System.Timers;
using Ak.Framework.Core.IO.FileSystemWatchers.Enums;
using Ak.Framework.Core.IO.FileSystemWatchers.EventArgs;

namespace Ak.Framework.Core.IO.FileSystemWatchers
{
    /// <summary>
    /// Наблюдатель событий файловой системы.
    /// Создает события при потере подключения к источнику.
    /// </summary>
    /// <seealso cref="System.IO.FileSystemWatcher" />
    [System.ComponentModel.DesignerCategory("Code")]
    public class NetFileSystemWatcher : FileSystemWatcher
    {
        #region Переменные и константы

        /// <summary>
        /// Количество секунд, через которое выполняется проверка соединения (по умолчанию)
        /// </summary>
        private const int DefaultCheckConnectionIntervalSeconds = 60 * 30;

        /// <summary>
        /// Объект блокировки
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Таймер
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Интервал проверки соединения
        /// </summary>
        private TimeSpan _checkConnectionInterval;

        #endregion

        #region Свойства

        /// <summary>
        /// Состояние соединения
        /// </summary>
        public FileSystemWatcherConnectionStates ConnectionState { get; private set; }

        /// <summary>
        /// Количество итераций, в котором объект остается в текущем состоянии
        /// </summary>
        protected long InStateCounter { get; private set; }

        /// <summary>
        /// Интервал проверки соединения
        /// </summary>
        private TimeSpan CheckConnectionInterval
        {
            get => _checkConnectionInterval;
            set
            {
                if (value.TotalMilliseconds < 1000)
                    throw new ArgumentException("Value too low", "checkConnectionInterval");
                _checkConnectionInterval = value;
            }
        }

        #endregion

        #region События

        /// <summary>
        /// Событие, возникающее, если теряется соединение с сетевой директории или происходит пересоединение
        /// </summary>
        [IODescription("FSW_ConnectionStateChanged")]
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public NetFileSystemWatcher()
        {
            Initialize();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path">Путь к директории для мониторинга</param>
        public NetFileSystemWatcher(string path) : base(path)
        {
            Initialize();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path">Путь к директории для мониторинга</param>
        /// <param name="filter">Фильтр названия файлов</param>
        public NetFileSystemWatcher(string path, string filter) : base(path, filter)
        {
            Initialize();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="checkConnectionInterval">Интервал проверки соединения</param>
        public NetFileSystemWatcher(TimeSpan checkConnectionInterval)
        {
            Initialize(checkConnectionInterval);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="checkConnectionInterval">Интервал проверки соединения</param>
        /// <param name="path">Путь к директории для мониторинга</param>
        public NetFileSystemWatcher(TimeSpan checkConnectionInterval, string path) : base(path)
        {
            Initialize(checkConnectionInterval);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="checkConnectionInterval">Интервал проверки соединения</param>
        /// <param name="path">Путь к директории для мониторинга</param>
        /// <param name="filter">Фильтр названия файлов</param>
        public NetFileSystemWatcher(TimeSpan checkConnectionInterval, string path, string filter)
            : base(path, filter)
        {
            Initialize(checkConnectionInterval);
        }

        #endregion

        #region Деструктор

        /// <summary>
        /// Деструктор
        /// </summary>
        /// <param name="disposing">Признак освобождения ресурсов</param>
        protected override void Dispose(bool disposing)
        {
            if (_timer != null)
            {
                Error -= HandleErrors;
                Changed -= ResetTimer;
                Deleted -= ResetTimer;
                Created -= ResetTimer;
                Renamed -= ResetTimer;

                _timer.Enabled = false;
                _timer.Elapsed -= ConnectCheckTimerElapsed;
                _timer = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Инициализация объектов
        /// </summary>
        private void Initialize()
        {
            Initialize(TimeSpan.FromSeconds(DefaultCheckConnectionIntervalSeconds));
        }

        /// <summary>
        /// Инициализация объектов
        /// </summary>
        /// <param name="checkConnectionInterval">Интервал проверки соединения</param>
        private void Initialize(TimeSpan checkConnectionInterval)
        {
            CheckConnectionInterval = checkConnectionInterval;
            ConnectionState = FileSystemWatcherConnectionStates.Connected;

            Error += HandleErrors;
            Changed += ResetTimer;
            Deleted += ResetTimer;
            Created += ResetTimer;
            Renamed += ResetTimer;

            _timer = new Timer(GetTimerInterval(ConnectionState));
            _timer.Elapsed += ConnectCheckTimerElapsed;
            _timer.Enabled = true;
        }

        /// <summary>
        /// Проверка соединения
        /// </summary>
        private void CheckConnection()
        {
            if (EnableRaisingEvents || ConnectionState == FileSystemWatcherConnectionStates.Disconnected)
            {
                _timer.Enabled = false;
                try
                {
                    EnableRaisingEvents = false;
                    EnableRaisingEvents = true;

                    SetConnectionState(FileSystemWatcherConnectionStates.Connected);
                }
                catch
                {
                    SetConnectionState(FileSystemWatcherConnectionStates.Disconnected);
                }
                _timer.Interval = GetTimerInterval(ConnectionState);
                _timer.Enabled = true;
            }
        }

        /// <summary>
        /// Установка состояния соединения
        /// </summary>
        /// <param name="newState">Новое состояние</param>
        private void SetConnectionState(FileSystemWatcherConnectionStates newState)
        {
            if (ConnectionState != newState)
            {
                bool stateChanged = false;

                lock (_lock)
                {
                    if (ConnectionState != newState)
                    {
                        InStateCounter = 0;
                        ConnectionState = newState;
                        stateChanged = true;
                    }
                }

                if (stateChanged)
                    OnConnectionStateChanged(new ConnectionStateChangedEventArgs(newState));
            }
            else
            {
                InStateCounter++;
                if (newState == FileSystemWatcherConnectionStates.Connected)
                    OnConnectionStateChanged(new ConnectionStateChangedEventArgs(FileSystemWatcherConnectionStates.Reconnected));
            }
        }

        /// <summary>
        /// Получение интервала таймера
        /// </summary>
        /// <param name="newState">Новое состояние</param>
        /// <returns></returns>
        private double GetTimerInterval(FileSystemWatcherConnectionStates newState)
        {
            switch (newState)
            {
                case FileSystemWatcherConnectionStates.Connected:
                    return CheckConnectionInterval.TotalMilliseconds;

                default:
                    return Math.Min(TimeSpan.FromSeconds(InStateCounter + 1).TotalMilliseconds, CheckConnectionInterval.TotalMilliseconds);
            }
        }

        #endregion

        #region Обработчики событий

        private void ResetTimer(object sender, FileSystemEventArgs e)
        {
            _timer.Enabled = false;
            _timer.Enabled = true;
        }

        private void ConnectCheckTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            CheckConnection();
        }

        private void HandleErrors(object sender, ErrorEventArgs e)
        {
            SetConnectionState(FileSystemWatcherConnectionStates.Disconnected);
        }

        protected virtual void OnConnectionStateChanged(ConnectionStateChangedEventArgs eventArgs)
        {
            EventHandler<ConnectionStateChangedEventArgs> temp = ConnectionStateChanged;
            temp?.Invoke(this, eventArgs);
        }

        #endregion
    }
}
