using System;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения над стандартными методами дробных чисел.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Округление
        /// </summary>
        /// <param name="num">Число</param>
        /// <param name="digitsAfterComma">Количество цифр дробной части в возвращаемом значении</param>
        /// <returns>Число, ближайшее к параметру num, количество цифр дробной части которого равно digitsAfterComma</returns>
        public static double Round(this double num, int digitsAfterComma = 2)
        {
            return Math.Round(num, digitsAfterComma);
        }

        /// <summary>
        /// Проверка на равенство
        /// </summary>
        /// <param name="lhs">Элемент  для сравнения 1</param>
        /// <param name="rhs">Элемент  для сравнения 2</param>
        /// <returns></returns>
        public static bool IsEqual(this double lhs, double rhs)
        {
            return Math.Abs(lhs - rhs) < double.Epsilon;
        }

        /// <summary>
        /// Проверка на равенство
        /// </summary>
        /// <param name="lhs">Элемент  для сравнения 1</param>
        /// <param name="rhs">Элемент  для сравнения 2</param>
        /// <returns></returns>
        public static bool IsEqual(this double lhs, float rhs)
        {
            return Math.Abs(lhs - rhs) < double.Epsilon;
        }

        /// <summary>
        /// Проверка на равенство
        /// </summary>
        /// <param name="lhs">Элемент  для сравнения 1</param>
        /// <param name="rhs">Элемент  для сравнения 2</param>
        /// <returns></returns>
        public static bool IsEqual(this float lhs, double rhs)
        {
            return Math.Abs(lhs - rhs) < double.Epsilon;
        }

        /// <summary>
        /// Проверка на равенство
        /// </summary>
        /// <param name="lhs">Элемент  для сравнения 1</param>
        /// <param name="rhs">Элемент  для сравнения 2</param>
        /// <returns></returns>
        public static bool IsEqual(this float lhs, float rhs)
        {
            return Math.Abs(lhs - rhs) < double.Epsilon;
        }
    }
}
