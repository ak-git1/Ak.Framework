using System;
using System.Linq.Expressions;
using System.ServiceModel;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы с исключениями
    /// </summary>
    public static class ThrowHelper
    {
        /// <summary>
        /// Проверка, что выражение не null
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="propertyExpression">Выражение</param>
        public static void CheckNotNull<T>(Expression<Func<T>> propertyExpression)
        {
            T arg = propertyExpression.Compile()();
            MemberExpression body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new InvalidOperationException("Can't get member expression");

            string name = body.Member.Name;
            CheckNotNull(arg, name);
        }

        /// <summary>
        /// Проверка, что аргумент не null
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <param name="argName">Название аргумента</param>
        public static void CheckNotNull(object arg, string argName)
        {
            if (arg.IsNull())
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// Проверка на то, что аргумент не null или не пробел
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <param name="argName">Название аргумента</param>
        public static void CheckNotNullOrWhiteSpace(string arg, string argName)
        {
            if (arg.IsNullOrWhiteSpace())
                throw new ArgumentException("'" + argName + "' shouldn't be unset here.");
        }

        /// <summary>
        /// Проверка, что аргумент null
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <param name="argName">Название аргумента</param>
        public static void CheckNull(object arg, string argName)
        {
            if (arg.IsNotNull())
                throw new ArgumentException("'" + arg + "' should be null here.");
        }

        /// <summary>
        /// Проверка, что аргумент true
        /// </summary>
        /// <param name="condition">Условие</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        public static void CheckTrue(bool condition, string errorMessage)
        {
            if (!condition)
                throw new ArgumentException(errorMessage);
        }

        /// <summary>
        /// Проверка, что аргумент true или выдача FaultException
        /// </summary>
        /// <param name="condition">Условие</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        public static void CheckTrueOrThrowFault(bool condition, string errorMessage)
        {
            if (!condition)
                throw new FaultException(errorMessage);
        }

        /// <summary>
        /// Проверка типа
        /// </summary>
        /// <param name="arg">Аргумент</param>
        /// <param name="argName">Название аргумента</param>
        /// <param name="desiredType">Ожидаемый тип</param>
        public static void CheckType(object arg, string argName, Type desiredType)
        {
            CheckNotNull(arg, "arg");
            CheckNotNull(desiredType, "desiredType");
            if (arg.GetType() != desiredType)
            {
                throw new WrongTypeException(argName + " should have " + desiredType.Name + " type here, but it is " + arg.GetType().Name);
            }
        }

        /// <summary>
        /// Исключение для некорректного типа
        /// </summary>
        public class WrongTypeException : Exception
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public WrongTypeException()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="message">Сообщение</param>
            public WrongTypeException(string message) : base(message)
            {
            }
        }
    }
}
