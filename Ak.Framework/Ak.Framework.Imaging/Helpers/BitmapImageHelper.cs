using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Ak.Framework.Imaging.Helpers
{
    /// <summary>
    /// Класс для работы с BitmapImage
    /// </summary>
    public class BitmapImageHelper
    {
        /// <summary>
        /// Получение BitmapImage из потока
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImageFromStream(Stream stream)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        /// <summary>
        /// Получение BitmapImage из строки пути к файлу
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImageFromPath(string path)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(path);
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
