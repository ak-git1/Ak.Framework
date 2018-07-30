using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ak.Framework.Threading
{
    /// <summary>
    /// Планировщик заданий
    /// </summary>
    public class LimitedConcurrencyScheduler : TaskScheduler
    {
        #region Переменные

        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        private readonly LinkedList<Task> _tasks = new LinkedList<Task>();

        private readonly int _maxDegreeOfParallelism;

        private int _delegatesQueuedOrRunning = 0;

        public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="maxDegreeOfParallelism">Максимальный уровень параллелизма</param>
        public LimitedConcurrencyScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1)
                throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");

            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        #endregion

        #region Методы

        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_tasks)
                return _tasks.ToArray();
        }

        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(o =>
            {
                _currentThreadIsProcessingItems = true;
                try
                {
                    while (true)
                    {
                        Task value;
                        lock (_tasks)
                        {
                            if (_tasks.Count != 0)
                            {
                                value = _tasks.First.Value;
                                _tasks.RemoveFirst();
                            }
                            else
                            {
                                Interlocked.Decrement(ref _delegatesQueuedOrRunning);
                                break;
                            }
                        }
                        TryExecuteTask(value);
                    }
                }
                finally
                {
                    _currentThreadIsProcessingItems = false;
                }
            }, null);
        }

        protected sealed override void QueueTask(Task task)
        {
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    Interlocked.Increment(ref _delegatesQueuedOrRunning);
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks)
            {
                return _tasks.Remove(task);
            }
        }

        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            bool flag;
            if (_currentThreadIsProcessingItems)
            {
                if (taskWasPreviouslyQueued)
                    TryDequeue(task);
                
                flag = TryExecuteTask(task);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        #endregion        
    }
}
