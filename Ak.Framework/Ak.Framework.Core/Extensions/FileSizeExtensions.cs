using System;
using Ak.Framework.Core.Enums;

namespace Ak.Framework.Core.Extensions
{
    /// <summary>
    /// Расширения для работы с размерами файлов
    /// </summary>
    public static class FileSizeExtensions
    {
        /// <summary>
        /// Приведение к человекочитабельному размеру файла
        /// </summary>
        /// <param name="sizeInBytes">Рамер файла </param>
        /// <returns></returns>
        public static UserFriendlyFileSize SizeInBytesToUserFriendlyFileSize(this long sizeInBytes)
        {
            sizeInBytes.SizeInBytesToUserFriendlyFileSize(out double num, out MemoryUnits unit);
            return new UserFriendlyFileSize(num, unit);
        }

        /// <summary>
        /// Приведение к человекочитабельному размеру файла
        /// </summary>
        /// <param name="sizeInBytes">Размер файла</param>
        /// <param name="sizeValue">Значение</param>
        /// <param name="sizeUnit">Единица измерения</param>
        public static void SizeInBytesToUserFriendlyFileSize(this long sizeInBytes, out double sizeValue, out MemoryUnits sizeUnit)
        {
            if (sizeInBytes == 0L)
            {
                sizeValue = 0.0;
                sizeUnit = MemoryUnits.Bytes;
            }
            else
            {
                sizeInBytes = Math.Abs(sizeInBytes);
                int num = Convert.ToInt32(Math.Floor(Math.Log(sizeInBytes, 1024.0)));
                sizeValue = Math.Round(sizeInBytes / Math.Pow(1024.0, num), 1);
                sizeValue = Math.Sign(sizeInBytes) * sizeValue;
                sizeUnit = (MemoryUnits)num;
            }
        }
    }
}
