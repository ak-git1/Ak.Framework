using System;
using System.Collections;
using System.Collections.Generic;

namespace Ak.Framework.Core.Collections
{
    /// <summary>
    /// Упорядоченная очередь фиксированного размера
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    /// <seealso cref="System.Collections.ICollection" />
    public class FixedCapacityIndexedQueue<T> : IEnumerable<T>, ICollection
    {
        #region Переменные

        /// <summary>
        /// Массив
        /// </summary>
        private readonly T[] _array;

        /// <summary>
        /// Оставшееся место
        /// </summary>
        private int _spaceLeft;

        /// <summary>
        /// Версия
        /// </summary>
        private int _version;

        #endregion

        #region Свойства

        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Признак того, что очередь заполнена
        /// </summary>
        public bool IsQueueFilledUp => _spaceLeft == 0;

        /// <summary>
        /// Признак того, что очередь синхронизирована 
        /// </summary>
        [Obsolete("Not supported")]
        public bool IsSynchronized => throw new NotSupportedException();

        /// <summary>
        /// Получение элемента по индексу
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException();                
                return _array[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException();
                _array[index] = value;
            }
        }

        /// <summary>
        /// Получение нального элемента
        /// </summary>
        public T Peek => this[0];

        /// <summary>
        /// SyncRoot
        /// </summary>
        [Obsolete("Not supported")]
        public object SyncRoot => throw new NotSupportedException();

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="capacity">Количество элементов</param>
        public FixedCapacityIndexedQueue(int capacity)
        {
            _array = new T[capacity];
            _spaceLeft = capacity;
            _version = 0;
            Count = 0;
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Копирование элементов, начиная с индекса
        /// </summary>
        /// <param name="array">Массив</param>
        /// <param name="index">Индекс</param>
        [Obsolete("Not implemented")]
        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Извлечение из очереди
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("Failed to dequeue object because the queue is empty");           
            return Dequeue(0);
        }

        /// <summary>
        /// Извлечение из очереди
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns></returns>
        public T Dequeue(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("The index of element to remove must be within the number of elements");
            
            T t = _array[index];
            for (int i = index + 1; i < Count; i++)
            {
                _array[i - 1] = _array[i];
            }
            FixedCapacityIndexedQueue<T> fixedCapacityIndexedQueue = this;
            fixedCapacityIndexedQueue.Count = fixedCapacityIndexedQueue.Count - 1;
            _array[Count] = default(T);
            FixedCapacityIndexedQueue<T> fixedCapacityIndexedQueue1 = this;
            fixedCapacityIndexedQueue1._spaceLeft = fixedCapacityIndexedQueue1._spaceLeft + 1;
            FixedCapacityIndexedQueue<T> fixedCapacityIndexedQueue2 = this;
            fixedCapacityIndexedQueue2._version = fixedCapacityIndexedQueue2._version + 1;
            return t;
        }

        /// <summary>
        /// Добавление в очередь
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <returns></returns>
        public bool Enqueue(T obj)
        {
            bool flag;
            if (_spaceLeft != 0)
            {
                T[] tArray = _array;
                FixedCapacityIndexedQueue<T> fixedCapacityIndexedQueue = this;
                int num = fixedCapacityIndexedQueue.Count;
                int num1 = num;
                fixedCapacityIndexedQueue.Count = num + 1;
                tArray[num1] = obj;
                FixedCapacityIndexedQueue<T> fixedCapacityIndexedQueue1 = this;
                fixedCapacityIndexedQueue1._spaceLeft = fixedCapacityIndexedQueue1._spaceLeft - 1;
                FixedCapacityIndexedQueue<T> fixedCapacityIndexedQueue2 = this;
                fixedCapacityIndexedQueue2._version = fixedCapacityIndexedQueue2._version + 1;
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Добавление в очредь, даже если нет места
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="dequeued">Признак того, что было выполнено предварительное освобождение очереди</param>
        /// <returns></returns>
        public T EnqueueAnyWay(T obj, out bool dequeued)
        {
            dequeued = Count == _array.Length;
            T t = dequeued ? Dequeue() : default(T);
            Enqueue(obj);
            return t;
        }

        /// <summary>
        /// Добавление в очредь, даже если нет места
        /// </summary>
        /// <param name="obj">Объект</param>
        /// <param name="indexToRemove">Индекс элемента для удаления</param>
        /// <param name="dequeued">Признак того, что было выполнено предварительное освобождение очереди</param>
        /// <returns></returns>
        public T EnqueueAnyWay(T obj, int indexToRemove, out bool dequeued)
        {
            if (indexToRemove < 0 || indexToRemove >= Count)
                throw new ArgumentOutOfRangeException("The index of element to remove must be within the number of elements");

            dequeued = Count == _array.Length;
            T t = dequeued ? Dequeue(indexToRemove) : default(T);
            Enqueue(obj);
            return t;
        }

        /// <summary>
        /// Получение инумератора
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new QueueEnumerator<T>(this);
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Получение инумератора
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _array.GetEnumerator();
        }

        #endregion

        #region Структуры 

        /// <summary>
        /// Инумератор очереди
        /// </summary>
        /// <typeparam name="TEnumerator">Тип</typeparam>
        /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
        /// <seealso cref="System.Collections.ICollection" />
        internal struct QueueEnumerator<TEnumerator> : IEnumerator<TEnumerator>
        {
            #region Переменные 

            /// <summary>
            /// Очередь
            /// </summary>
            private readonly FixedCapacityIndexedQueue<TEnumerator> _queue;

            /// <summary>
            /// Текущий индекс
            /// </summary>
            private int _currentIndex;

            /// <summary>
            /// Количество элементов
            /// </summary>
            private readonly int _valuesCount;

            /// <summary>
            /// Версия
            /// </summary>
            private readonly int _version;

            #endregion

            #region Свойства

            /// <summary>
            /// Текущий элемент
            /// </summary>
            public TEnumerator Current => _currentIndex <= -1 || _currentIndex >= _valuesCount ? default(TEnumerator) : _queue._array[_currentIndex];

            /// <summary>
            /// Текущий элемент
            /// </summary>
            object IEnumerator.Current => ((IEnumerator<TEnumerator>)this).Current;

            #endregion

            #region Конструктор

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="queue">Очередь</param>
            public QueueEnumerator(FixedCapacityIndexedQueue<TEnumerator> queue)
            {
                _queue = queue;
                _currentIndex = -1;
                _valuesCount = _queue.Count;
                _version = _queue._version;
            }

            #endregion

            #region Публичные методы

            /// <summary>
            /// Деструктор
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Переход к следующему элементу
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (_queue._version != _version)
                    throw new InvalidOperationException("Failed to enumerate _queue items. Collection was changed");
                
                QueueEnumerator<TEnumerator> queueEnumerator = this;
                int num = queueEnumerator._currentIndex + 1;
                queueEnumerator._currentIndex = num;
                return num < _valuesCount;
            }

            /// <summary>
            /// Сброс
            /// </summary>
            [Obsolete("Not implemented")]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            #endregion
        }

        #endregion
    }
}