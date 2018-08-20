using System;
using System.Collections.Generic;
using System.Threading;
using Ak.Framework.Wpf.Messaging.Interfaces;

namespace Ak.Framework.Wpf.Messaging
{
    /// <summary>
    /// Класс для работы с сообщениями
    /// </summary>
    public class Messenger : IMessenger
    {
        #region Переменные

        /// <summary>
        /// Экземпляр класса
        /// </summary>
        private static Messenger _instance;

        /// <summary>
        /// Блокирующий объект
        /// </summary>
        private static readonly object InstanceLock = new object();

        /// <summary>
        /// Обработчики
        /// </summary>
        private readonly Dictionary<string, MessageHandler> _handlers = new Dictionary<string, MessageHandler>(1);

        /// <summary>
        /// Блокировщик доступак ресурсам
        /// </summary>
        private readonly ReaderWriterLockSlim _syncLock = new ReaderWriterLockSlim();

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        private Messenger()
        {
        }

        #endregion

        #region Свойства

        /// <summary>
        /// Экземпляр объекта в виде синглтона
        /// </summary>
        public static Messenger Instance
        {
            get
            {
                lock (InstanceLock)
                    return _instance ?? (_instance = new Messenger());
            }
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="handler">Обработчик</param>
        /// <param name="registerIfExists">Регистрация при существовании</param>
        /// <returns></returns>
        public IMessenger Register(string message, MessageHandler handler, bool registerIfExists = true)
        {
            IMessenger messenger;
            if (handler == null)
                throw new ArgumentNullException("handler");

            _syncLock.EnterUpgradeableReadLock();
            try
            {
                if (!_handlers.TryGetValue(message, out MessageHandler messageHandler))
                {
                    _syncLock.EnterWriteLock();
                    try
                    {
                        _handlers[message] = handler;
                        messenger = this;
                        return messenger;
                    }
                    finally
                    {
                        _syncLock.ExitWriteLock();
                    }
                }

                if (registerIfExists)
                {
                    messageHandler += handler;
                    _handlers[message] = messageHandler;
                }
            }
            finally
            {
                _syncLock.ExitUpgradeableReadLock();
            }
            messenger = this;
            return messenger;
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        /// <param name="handler">Обработчик</param>
        /// <param name="registerIfExists">Регистрация при существовании</param>
        /// <returns></returns>
        public IMessenger Register(Enum message, MessageHandler handler, bool registerIfExists = true)
        {
            IMessenger messenger = Register(MessageEnumToString(message), handler, registerIfExists);
            return messenger;
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="info">Сообщение</param>
        /// <param name="args">Параметры</param>
        public void Send(string message, Message info, params object[] args)
        {
            if (_handlers.TryGetValue(message, out MessageHandler messageHandler))
                messageHandler(info, args);
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        /// <param name="info">Сообщение</param>
        /// <param name="args">Параметры</param>
        public void Send(Enum message, Message info, params object[] args)
        {
            Send(MessageEnumToString(message), info, args);
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Параметры</param>
        public void Send(string message, params object[] args)
        {
            Send(message, new Message(), args);
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        /// <param name="args">Параметры</param>
        public void Send(Enum message, params object[] args)
        {
            Send(MessageEnumToString(message), args);
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        public void Send(Enum message)
        {
            Send(message, new object());
        }

        /// <summary>
        /// Отключение регистрации
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="handler">Обработчик</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">handler</exception>
        public IMessenger Unregister(string message, MessageHandler handler)
        {
            IMessenger messenger;
            if (handler == null)
                throw new ArgumentNullException("handler");

            _syncLock.EnterUpgradeableReadLock();
            try
            {
                MessageHandler messageHandler;
                if (_handlers.TryGetValue(message, out messageHandler))
                {
                    messageHandler -= handler;
                    _handlers[message] = messageHandler;
                    if (messageHandler == null)
                    {
                        _syncLock.EnterWriteLock();
                        try
                        {
                            _handlers.Remove(message);
                        }
                        finally
                        {
                            _syncLock.ExitWriteLock();
                        }
                    }
                }
                else
                {
                    messenger = this;
                    return messenger;
                }
            }
            finally
            {
                _syncLock.ExitUpgradeableReadLock();
            }
            messenger = this;
            return messenger;
        }

        /// <summary>
        /// Отключение регистрации
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="handler">Обработчик</param>
        /// <returns></returns>
        public IMessenger Unregister(Enum message, MessageHandler handler)
        {
            return Unregister(MessageEnumToString(message), handler);
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Преобразование строки в значение инумератора
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        private static string MessageEnumToString(Enum message)
        {
            return $"{message.GetType()}.{message}";
        }

        #endregion        
    }

}
