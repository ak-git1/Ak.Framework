using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Ak.Framework.Core.Utils;

namespace Ak.Framework.Imaging.Extensions
{
    /// <summary>
    /// Расширения для Bitmap
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Конвертация Bitmap в BitmapSource
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(this Bitmap bitmap)
        {
            BitmapSource bitmapSource;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            using (Bitmap tempBitmap = new Bitmap(bitmap))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                tempBitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0L;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                bitmapSource = bitmapImage;
            }

            return bitmapSource;
        }

        /// <summary>
        /// Полное клонирование Bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="pixelFormat">Формат пикселей</param>
        /// <returns></returns>
        public static Bitmap CloneSmart(this Bitmap bitmap, PixelFormat pixelFormat)
        {
            Bitmap result;
            GarbageCollector.Collect();

            using (Bitmap b = new Bitmap(bitmap))
            {
                b.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
                result = b.Clone(new Rectangle(Point.Empty, new Size(b.Width, b.Height)), pixelFormat);
            }

            return result;
        }
    }
}
