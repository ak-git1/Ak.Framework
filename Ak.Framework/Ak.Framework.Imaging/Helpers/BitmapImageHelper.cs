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
        public static BitmapImage GetBitmapFromStream(Stream stream)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }
    }
}
