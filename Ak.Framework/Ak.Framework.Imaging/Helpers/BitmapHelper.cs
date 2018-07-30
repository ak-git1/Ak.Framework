using System.Drawing;
using System.IO;

namespace Ak.Framework.Imaging.Helpers
{
    /// <summary>
    /// Класс для работы с Bitmap
    /// </summary>
    public class BitmapHelper
    {
        /// <summary>
        /// Создание незаблокированного изображения из файла
        /// </summary>
        /// <param name="filePath">Путь</param>
        /// <returns></returns>
        public static Bitmap CreateUnblockedBitmap(string filePath)
        {
            byte[] numArray = File.ReadAllBytes(filePath);
            return new Bitmap(new MemoryStream(numArray, false));
        }
    }
}
