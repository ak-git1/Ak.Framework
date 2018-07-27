using System.IO;

namespace Ak.Framework.Core.IO.Extensions
{
    /// <summary>
    /// Расширения для FileInfo
    /// </summary>
    public static class FileInfoExtensions
    {
        #region Публичные методы

        /// <summary>
        /// Открытие файла на чтение
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        public static FileStream OpenReadLocked(this FileInfo file)
        {
            return file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
        }

        /// <summary>
        /// Открытие файла на запись
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        public static FileStream OpenWriteLocked(this FileInfo file)
        {
            return file.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        #endregion
    }
}
