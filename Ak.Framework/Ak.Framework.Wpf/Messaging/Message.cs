using System;
using System.Runtime.Serialization;
using Ak.Framework.Core.Helpers;

namespace Ak.Framework.Wpf.Messaging
{
    /// <summary>
    /// Сообщение
    /// </summary>
    [DataContract, KnownType("GetDerivedTypes")]
    public class Message
    {
        #region Свойства

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Отправитель
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Значение
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime TimeCreated { get; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public Message()
        {
            TimeCreated = DateTime.Now;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="value">Состояние</param>
        public Message(object value) : this()
        {
            Value = value;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="value">Значение</param>
        public Message(object sender, object value) : this(value)
        {
            Value = value;
            Sender = sender;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Наименование</param>
        public Message(string name) : this()
        {
            Name = name;
        }

        #endregion

        #region Публичные методы

        /// <summary>
        /// Сорздание пустого сообщения
        /// </summary>
        /// <returns></returns>
        public static Message CreateEmpty()
        {
            return new Message();
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Получение унаследованных типов
        /// </summary>
        /// <returns></returns>
        private static Type[] GetDerivedTypes()
        {
            return ReflectionHelper.GetTypesDerivedFrom(typeof(Message));
        }

        #endregion
    }
}