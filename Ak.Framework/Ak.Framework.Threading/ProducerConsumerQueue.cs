using System;
using System.Collections.Generic;
using System.Threading;
using Ak.Framework.Core.Extensions;
using Ak.Framework.Core.Helpers;
using NLog;

namespace Ak.Framework.Threading
{
    /// <summary>
    /// Потокобезопасная очередь
    /// </summary>
    /// <typeparam name="TTask">Действие</typeparam>
    public class ProducerConsumerQueue<TTask> : IDisposable
    {
        #region Переменные

        private readonly List<TTask> _taskList = new List<TTask>();
        private readonly object _taskListLocker = new object();
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Action<TTask, CancellationToken> _consumerAction;
        private readonly Action<Exception> _exceptionAction;

        #endregion

        #region Свойства

        public int TaskCount
        {
            get
            {
                lock (_taskListLocker)
                    return _taskList.Count;
            }
        }

        public TimeSpan SleepTimeout { get; set; }

        public IEnumerable<TTask> Tasks
        {
            get
            {
                lock (_taskListLocker)
                    return _taskList.AsReadOnly();
            }
        }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="consumerAction">Действие</param>
        public ProducerConsumerQueue(Action<TTask, CancellationToken> consumerAction)
        {
            ThrowHelper.CheckNotNull(consumerAction, "consumerAction");
            SleepTimeout = 2.Seconds();
            _consumerAction = consumerAction;
            Thread workingThread = new Thread(ConsumeItems)
            {
                IsBackground = true
            };
            workingThread.Start();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cosumerAction">Действие</param>
        /// <param name="exceptionAction">Действие в случае исключения</param>
        public ProducerConsumerQueue(Action<TTask, CancellationToken> cosumerAction, Action<Exception> exceptionAction)
          : this(cosumerAction)
        {
            _exceptionAction = exceptionAction;
        }

        #endregion

        #region Публичные методы

        public void AddTask(TTask task)
        {
            lock (_taskListLocker)
                _taskList.Add(task);
        }

        public void AddUrgentTask(TTask task)
        {
            lock (_taskListLocker)
                _taskList.Insert(0, task);
        }

        public bool RemoveTask(TTask task)
        {
            lock (_taskListLocker)
                return _taskList.Remove(task);
        }

        public bool ContainsTask(TTask task)
        {
            lock (_taskListLocker)
                return _taskList.Contains(task);
        }

        public void ClearQueue()
        {
            lock (_taskListLocker)
                _taskList.Clear();
        }

        public void Dispose()
        {
            _tokenSource?.Cancel();
        }

        #endregion

        #region Приватные методы

        private void ConsumeItems()
        {
            while (!_tokenSource.IsCancellationRequested)
            {
                try
                {
                    TTask task = default(TTask);
                    bool flag;
                    do
                    {
                        lock (_taskListLocker)
                        {
                            flag = _taskList.Count > 0;
                            if (flag)
                            {
                                task = _taskList[0];
                                _taskList.RemoveAt(0);
                            }
                        }
                        if (flag)
                        {
                            try
                            {
                                _consumerAction(task, _tokenSource.Token);
                                task = default(TTask);
                            }
                            catch (Exception ex)
                            {
                                if (_exceptionAction != null)
                                    _exceptionAction(ex);
                                else
                                    _logger.Error(ex, "Top-level exception in consumer method, and no exception handler is registered.");
                            }
                        }
                    }
                    while (flag);
                    Thread.Sleep(SleepTimeout);
                }
                catch (Exception ex)
                {
                    if (_exceptionAction != null)
                        _exceptionAction(ex);
                    else
                        _logger.Error(ex, "Exception in processing thread");
                }
            }
        }

        #endregion
    }
}
