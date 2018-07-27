using System;
using System.Threading;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы с выполняемыми делегатами
    /// </summary>
    public static class ActionsHelper
    {
        /// <summary>
        /// Выполнение метода с определнным количеством попыток в случае превышения оперативной памяти 
        /// </summary>
        /// <param name="action">Метод</param>
        /// <param name="numTries">Количество попыток</param>
        /// <param name="shouldExecuteGarbageCollectionAfterAction">Запусть Garbage Collector после выполнения</param>
        public static void ExecuteMemoryAware(this Action action, int numTries, bool shouldExecuteGarbageCollectionAfterAction)
        {
            Exception innerException = null;
            while (numTries > 0)
            {
                try
                {
                    action();
                    if (shouldExecuteGarbageCollectionAfterAction)
                    {
                        GC.Collect();
                        GC.WaitForFullGCComplete();
                    }
                    return;
                }
                catch (OutOfMemoryException ex)
                {
                    innerException = ex;
                    GC.Collect();
                    numTries--;
                }
            }
            throw new OutOfMemoryException("Out of memory.", innerException);
        }

        /// <summary>
        /// Выполнение метода с определнным количеством попыток в случае превышения оперативной памяти 
        /// </summary>
        /// <typeparam name="TResult">Тип результата</typeparam>
        /// <param name="func">Функция</param>
        /// <param name="numTries">Количество попыток</param>
        /// <param name="shouldExecuteGarbageCollectionAfterAction">Запусть Garbage Collector после выполнения</param>
        /// <returns></returns>
        public static TResult ExecuteMemoryAware<TResult>(this Func<TResult> func, int numTries, bool shouldExecuteGarbageCollectionAfterAction)
        {
            TResult result = default(TResult);
            ((Action)(() => result = func())).ExecuteMemoryAware(numTries, shouldExecuteGarbageCollectionAfterAction);
            return result;
        }

        /// <summary>
        /// Выполнение метода, не вызывающее исключения
        /// </summary>
        /// <param name="action">Метод</param>
        public static void ExecuteWithEmptyCatch(this Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Выполнение метода с вызвом обработчика в случае выпадения исключения
        /// </summary>
        /// <param name="action">Метод</param>
        /// <param name="errorAction">Метод в случае исключения</param>
        public static void ExecuteWithErrorAction(this Action action, Action<Exception> errorAction)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                errorAction(exception);
            }
        }

        /// <summary>
        /// Вызов обработчика события
        /// </summary>
        /// <param name="eventHandler">Обработчик события</param>
        /// <param name="sender">Объект, вызвавший событие</param>
        public static void Raise(this EventHandler eventHandler, object sender)
        {
            eventHandler?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Вызов обработчика события
        /// </summary>
        /// <param name="eventHandler">Обработчик события</param>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="eventArgs">Аргументы</param>
        public static void Raise(this EventHandler eventHandler, object sender, EventArgs eventArgs)
        {
            eventHandler?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Вызов обработчика события
        /// </summary>
        /// <typeparam name="TEventArgs">Тип аргументов</typeparam>
        /// <param name="eventHandler">Обработчик события</param>
        /// <param name="sender">Объект, вызвавший событие</param>
        /// <param name="eventArgs">Аргументы</param>
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            eventHandler?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Попытка выполнения метода несколько раз по таймауту
        /// </summary>
        /// <param name="action">Метод</param>
        /// <param name="numberOfTimes">Количество вызовов</param>
        /// <param name="timeout">Таймаут</param>
        /// <param name="throwException">Выдавать исключение</param>
        /// <returns></returns>
        public static bool TryExecuteManyTimesWithTimeoutOrThrow(this Action action, int numberOfTimes, TimeSpan timeout, bool throwException)
        {
            Exception innerException = null;
            for (int i = 0; i < numberOfTimes; i++)
            {
                try
                {
                    action();
                    return true;
                }
                catch (Exception ex)
                {
                    innerException = ex;
                    Thread.Sleep(timeout);
                }
            }

            if (throwException)
                throw new InvalidOperationException("Failed to execute action.", innerException);

            return false;
        }
    }
}
