using System;

namespace Ak.Framework.Wpf.Messaging.Interfaces
{
    /// <summary>
    /// Интерфейс класса для работы с сообщениями
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        /// <param name="handler">Обработчик</param>
        /// <param name="registerIfExists">Регистрация при существовании</param>
        /// <returns></returns>
        IMessenger Register(Enum message, MessageHandler handler, bool registerIfExists = true);

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="handler">Обработчик</param>
        /// <param name="registerIfExists">Регистрация при существовании</param>
        /// <returns></returns>
        IMessenger Register(string message, MessageHandler handler, bool registerIfExists = true);

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        void Send(Enum message);

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        /// <param name="args">Параметры</param>
        void Send(Enum message, params object[] args);

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="args">Параметры</param>
        void Send(string message, params object[] args);

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Значение инумератора для данного сообщения</param>
        /// <param name="info">Сообщение</param>
        /// <param name="args">Параметры</param>
        void Send(Enum message, Message info, params object[] args);

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="info">Сообщение</param>
        /// <param name="args">Параметры</param>
        void Send(string message, Message info, params object[] args);

        /// <summary>
        /// Отключение регистрации
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="handler">Обработчик</param>
        /// <returns></returns>
        IMessenger Unregister(Enum message, MessageHandler handler);

        /// <summary>
        /// Отключение регистрации
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="handler">Обработчик</param>
        /// <returns></returns>
        IMessenger Unregister(string message, MessageHandler handler);
    }
}
