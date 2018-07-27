using System;
using System.Linq.Expressions;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширение для получения названий членов класса.
    /// </summary>
    public static class StaticReflection
    {
        /// <summary>
        /// Получить название члена класса.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="instance">Экземпляр.</param>
        /// <param name="expression">Выражение.</param>
        /// <returns></returns>
        public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression);
        }

        /// <summary>
        /// Получить название члена класса.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="expression">Выражение.</param>
        /// <returns></returns>
        public static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
                throw new ArgumentException("The expression cannot be null.");

            return GetMemberName(expression.Body);
        }

        /// <summary>
        /// Получить название члена класса.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="instance">Экземпляр.</param>
        /// <param name="expression">Выражение.</param>
        /// <returns></returns>
        public static string GetMemberName<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetMemberName(expression);
        }

        /// <summary>
        /// Получить название члена класса.
        /// </summary>
        /// <typeparam name="T">Тип.</typeparam>
        /// <param name="expression">Выражение.</param>
        /// <returns></returns>
        public static string GetMemberName<T>(Expression<Action<T>> expression)
        {
            if (expression == null)
                throw new ArgumentException("The expression cannot be null.");

            return GetMemberName(expression.Body);
        }

        /// <summary>
        /// Получить название члена класса.
        /// </summary>
        /// <param name="expression">Выражение.</param>
        /// <returns></returns>
        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
                throw new ArgumentException("The expression cannot be null.");

            if (expression is MemberExpression memberExpression)
                return memberExpression.Member.Name;

            if (expression is MethodCallExpression methodCallExpression)
                return methodCallExpression.Method.Name;

            if (expression is UnaryExpression unaryExpression)
                return GetMemberName(unaryExpression);

            throw new ArgumentException("Invalid expression");
        }

        /// <summary>
        /// Получить название члена класса.
        /// </summary>
        /// <param name="unaryExpression">Выражение.</param>
        /// <returns></returns>
        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
                return methodExpression.Method.Name;

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
    }
}
