using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Ak.Framework.Wpf.Converters
{
    /// <summary>
    /// Конвертация Bitmap в BitmapSource
    /// </summary>
    /// <seealso cref="Ak.Framework.Wpf.Converters.ValueConverterBase" />
    public class BitmapToBitmapSourceConverter : ValueConverterBase
    {
        /// <summary>
        /// Преобразование Bitmap в BitmapSource
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns></returns>
        public static BitmapSource Convert(Bitmap bitmap)
        {
            BitmapSource source;
            if (bitmap == null)
            {
                return null;
            }
            lock (bitmap)
            {
                IntPtr hbitmap = bitmap.GetHbitmap();
                try
                {
                    source = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(hbitmap);
                }
            }
            return source;
        }

        /// <summary>
        /// Конвертация значений
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (!(value is Bitmap))
            {
                throw new ArgumentException();
            }
            Bitmap bitmap = value as Bitmap;
            return Convert(bitmap);
        }

        /// <summary>
        /// Обратная конвертация
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="targetType">Целевой тип</param>
        /// <param name="parameter">Параметр конвертации</param>
        /// <param name="culture">Локаль</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="hObject">Объект</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}
