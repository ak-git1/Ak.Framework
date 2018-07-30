using System.IO;
using System.Windows.Media.Imaging;

namespace Ak.Framework.Imaging.Helpers
{
    public class BitmapSourceHelper
    {
        /// <summary>
        /// Сохраняет битмап в файл формата JPEG
        /// </summary>
        /// <param name="bitmapSource">Битмап</param>
        /// <param name="pathName">Имя файла</param>
        public static void SaveToJpeg(BitmapSource bitmapSource, string pathName)
        {
            JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
            jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            using (FileStream fileStream = new FileStream(pathName, FileMode.Create))
                jpegBitmapEncoder.Save(fileStream);
        }
    }
}
