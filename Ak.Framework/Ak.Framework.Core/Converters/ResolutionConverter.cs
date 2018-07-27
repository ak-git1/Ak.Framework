namespace Ak.Framework.Core.Converters
{
    /// <summary>
    /// Конвертер значения разрешения изображения
    /// </summary>
    public class ResolutionConverter
    {
        #region Константы

        /// <summary>
        /// Количество мм в точках
        /// </summary>
        private const double MmsToPoint = 2.83464575;

        /// <summary>
        /// Количество мм в дюйме
        /// </summary>
        private const double MmsToInch = 25.4;

        #endregion

        /// <summary>
        /// Преобразование мм в точки
        /// </summary>
        /// <param name="mm">Миллиметры</param>
        /// <returns></returns>
        public static double MmToPoints(float mm)
        {
            return mm * MmsToPoint;
        }

        /// <summary>
        /// Преобразование мм в пиксели
        /// </summary>
        /// <param name="mm">Миллиметры</param>
        /// <param name="resolution">Разрешение</param>
        /// <returns></returns>
        public static double MmToPx(double mm, float resolution)
        {
            return mm * resolution / MmsToInch;
        }

        /// <summary>
        /// Преобразование пикселей в мм
        /// </summary>
        /// <param name="px">Пиксели</param>
        /// <param name="resolution">Разрешение</param>
        /// <returns></returns>
        public static double PxToMm(double px, float resolution)
        {
            return px / resolution * MmsToInch;
        }

        /// <summary>
        /// Преобразование пикселей в точки
        /// </summary>
        /// <param name="px">Пиксели</param>
        /// <param name="resolution">Разрешение</param>
        /// <returns></returns>
        public static double PxToPoints(double px, float resolution)
        {
            double mm = PxToMm(px, resolution);
            return mm * MmsToPoint;
        }
    }
}
