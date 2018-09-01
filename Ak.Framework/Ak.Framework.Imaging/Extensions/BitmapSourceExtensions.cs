using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace Ak.Framework.Imaging.Extensions
{
    /// <summary>
    /// Расширения для BitmapSource
    /// </summary>
    public static class BitmapSourceExtensions
    {
        /// <summary>
        /// Конвертация в Bitmap
        /// </summary>
        /// <param name="bitmapSource">BitmapSource</param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this BitmapSource bitmapSource)
        {
            Bitmap bmp = new Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            bitmapSource.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride); bmp.UnlockBits(data);
            return bmp;
        }
    }
}
