using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Ak.Framework.Imaging.Helpers;

namespace Ak.Framework.Imaging.Converters
{
    /// <summary>
    /// Класс для преобразования изображения
    /// </summary>
    public static class ImageConverter
    {
        #region Публичные методы

        /// <summary>
        /// Преобразование изображение в массив байт
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Преобразование изображение в массив байт
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="imageFormat">Формат изображения</param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(Image image, ImageFormat imageFormat)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Преобразование байтового массива в изображение
        /// </summary>
        /// <param name="data">Байтовый массив</param>
        /// <returns></returns>
        public static Image ConvertToImage(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Получение BitmapSource из изображения
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <returns></returns>
        public static BitmapSource ConvertToBitmapSource(Image image)
        {
            byte[] bytes = ConvertToBytes(image);
            using (MemoryStream memoryStream = new MemoryStream(bytes))
                return BitmapImageHelper.GetBitmapImageFromStream(memoryStream);
        }

        #endregion
    }
}