using System.IO;
using System.IO.Compression;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы с zip-архивами
    /// </summary>
    public sealed class ZipHelper
    {
        #region Публичные методы

        /// <summary>
        /// Извлекает файлы в указанную папку
        /// </summary>
        /// <param name="sourceStream">Поток с архивом</param>
        /// <param name="path">Путь к папке</param>
        public static void Extract(Stream sourceStream, string path)
        {
            using (ZipArchive arch = new ZipArchive(sourceStream, ZipArchiveMode.Read, false))
            {
                arch.ExtractToDirectory(path);
            }
        }

        #endregion
    }
}
