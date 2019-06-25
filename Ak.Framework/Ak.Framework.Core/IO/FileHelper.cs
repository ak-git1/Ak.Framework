using System.IO;

namespace Ak.Framework.Core.IO
{
    /// <summary>
    /// Класс для работы с файлами
    /// </summary>
    public static class FileHelper
    {
        #region Публичные методы

        /// <summary>
        /// Проверка возможности создания файла
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns></returns>
        public static bool CheckFileCanBeCreatedOrRewritten(string filePath)
        {
            if (File.Exists(filePath) && IsFileLocked(new FileInfo(filePath)))
                return false;

            return true;
        }

        /// <summary>
        /// Проверка на то, что файл заблокирован
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }

        #endregion
    }
}
