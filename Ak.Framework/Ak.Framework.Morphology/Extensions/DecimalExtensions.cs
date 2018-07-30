using Humanizer;

namespace Ak.Framework.Morphology.Extensions
{
    /// <summary>
    /// Расширения для работы с числовыми значениями
    /// </summary>
    public static class DecimalExtensions
    {
        #region Публичные методы

        /// <summary>
        /// Преобразование для рублей 
        /// </summary>
        /// <param name="money">Денежное значение</param>
        /// <param name="useCoins">Включать копейки</param>
        /// <returns></returns>
        public static string ToRurText(this decimal money, bool useCoins = true)
        {
            return ConvertToCurrencyString(money, "рубль", "рубля", "рублей", "копейка", "копейки", "копеек", useCoins);
        }

        /// <summary>
        /// Преобразование для долларов
        /// </summary>
        /// <param name="money">Денежное значение</param>
        /// <param name="useCoins">Включать центы</param>
        /// <returns></returns>
        public static string ToUsdText(this decimal money, bool useCoins = true)
        {
            return ConvertToCurrencyString(money, "доллар США", "доллара США", "долларов США", "цент", "цента", "центов");
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Преобразование числа в текстовое представление с добавлением валюты
        /// </summary>
        /// <returns></returns>
        private static string ConvertToCurrencyString(decimal money, string word1, string word234, string wordmore, string sword1, string sword234, string swordmore, bool useCoins = true)
        {
            money = decimal.Round(money, 2);
            decimal decintpart = decimal.Truncate(money);
            int intpart = (int)(decintpart);
            string str = intpart.ToWords() + " ";
            byte endpart = (byte)(intpart % 100);
            if (endpart > 19) endpart = (byte)(endpart % 10);
            switch (endpart)
            {
                case 1:
                    str += word1;
                    break;

                case 2:
                case 3:
                case 4:
                    str += word234;
                    break;

                default:
                    str += wordmore;
                    break;
            }

            if (useCoins)
            {
                byte fracpart = decimal.ToByte((money - decintpart) * 100M);
                str += " " + ((fracpart < 10) ? "0" : "") + fracpart + " ";
                if (fracpart > 19) fracpart = (byte)(fracpart % 10);

                switch (fracpart)
                {
                    case 1:
                        str += sword1;
                        break;

                    case 2:
                    case 3:
                    case 4:
                        str += sword234;
                        break;
                    default:
                        str += swordmore;
                        break;
                }

            }

            return str;
        }

        #endregion
    }
}