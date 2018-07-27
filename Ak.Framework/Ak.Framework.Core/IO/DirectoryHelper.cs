using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Ak.Framework.Core.IO.Enums;
using Ak.Framework.Core.IO.Extensions;
using Ak.Framework.Core.Helpers;

namespace Ak.Framework.Core.IO
{
    /// <summary>
    /// Класс для работы с директориями файловой системы
    /// </summary>
    public static class DirectoryHelper
    {
        #region Публичные методы

        /// <summary>
        /// Создание директории, если она не существует
        /// </summary>
        /// <param name="path">Путь к директории</param>
        public static void CreateDirectoryIfNotExists(string path)
        {
            PathHelper.ThrowIfInvalid(path);
            if (!PathHelper.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Рекурсивное создание поддиректорий
        /// </summary>
        /// <param name="path">Путь к директории</param>
        public static void CreateSubdirectoriesRecursively(string path)
        {
            PathHelper.ThrowIfInvalid(path);
            string[] strArray = path.Split('\\');
            if (strArray.Length != 0)
            {
                string str = strArray[0] + '\\';
                for (int i = 0; i < strArray.Length - 1; i++)
                {
                    string str2 = Path.Combine(str, strArray[i + 1]);
                    str = str2;
                    if (!Directory.Exists(str2))
                        Directory.CreateDirectory(str2);
                }
            }
        }

        /// <summary>
        /// Удаление директории, если он пустая
        /// </summary>
        /// <param name="path">Путь к директории</param>
        public static void DeleteDirectoryIfEmpty(string path)
        {
            PathHelper.ThrowIfInvalid(path);
            if (!(PathHelper.GetPathTargetType(path) != PathTargetTypes.Directory || Directory.GetFiles(path).Any()))
                Directory.Delete(path);
        }

        /// <summary>
        /// Проверка на то, что директория пуста
        /// </summary>
        /// <param name="directory">Путь к директории</param>
        /// <returns></returns>
        public static bool IsDirectoryEmpty(string directory)
        {
            PathHelper.ThrowIfInvalid(directory);
            ThrowHelper.CheckTrue(PathHelper.GetPathTargetType(directory) == PathTargetTypes.Directory, "Target path '" + directory + "' is not a directory.");
            return Directory.GetFileSystemEntries(directory).Any();
        }

        /// <summary>
        /// Перемещение содержимого директории
        /// </summary>
        /// <param name="sourceDirectory">начальная директория</param>
        /// <param name="targetDirectory">Конечная директория</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public static void MoveAllContents(string sourceDirectory, string targetDirectory, CancellationToken? cancellationToken = null)
        {
            ThrowHelper.CheckNotNullOrWhiteSpace(sourceDirectory, nameof(sourceDirectory));
            ThrowHelper.CheckNotNullOrWhiteSpace(targetDirectory, nameof(targetDirectory));
            ThrowHelper.CheckTrue(PathHelper.GetPathTargetType(sourceDirectory) == PathTargetTypes.Directory, "Path '" + sourceDirectory + "' is not a directory.");
            PathTargetTypes pathTargetType = PathHelper.GetPathTargetType(targetDirectory);
            ThrowHelper.CheckTrue(pathTargetType == PathTargetTypes.Directory || pathTargetType == PathTargetTypes.Unknown, "'" + targetDirectory + "' should be a directory or should not exist.");
            if (sourceDirectory != targetDirectory)
            {
                CreateDirectoryIfNotExists(targetDirectory);
                string[] directories = Directory.GetDirectories(sourceDirectory);
                foreach (string str in directories)
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    MoveAllContents(str, Path.Combine(targetDirectory, str));
                }

                string[] files = Directory.GetFiles(sourceDirectory);
                foreach (string str2 in files)
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    string fileName = Path.Combine(sourceDirectory, str2);
                    string targetPath = Path.Combine(targetDirectory, str2);
                    FileInfo file = new FileInfo(fileName);
                    StreamHelper.WriteToFileSafely(file.OpenReadLocked(), targetPath);
                    File.Delete(fileName);
                }
            }
        }

        /// <summary>
        /// Рекурсивное удаление директории и её содержимого
        /// </summary>
        /// <param name="directory">Директория</param>
        /// <param name="ignoreErrors">Игнорировать ошибки при удалении</param>
        public static void DeleteRecursively(string directory, bool ignoreErrors = false)
        {
            PathHelper.ThrowIfInvalid(directory);
            try
            {
                if (PathHelper.GetPathTargetType(directory) != PathTargetTypes.Unknown)
                {
                    ThrowHelper.CheckTrue(PathHelper.GetPathTargetType(directory) == PathTargetTypes.Directory, "Target is not a directory.");
                    string[] directories = Directory.GetDirectories(directory);
                    foreach (string dir in directories)
                        DeleteRecursively(Path.Combine(directory, dir));
                    
                    string[] files = Directory.GetFiles(directory);
                    foreach (string file in files)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {
                            if (!ignoreErrors)
                                throw;
                        }
                    }
                    try
                    {
                        Directory.Delete(directory);
                    }
                    catch
                    {
                        if (!ignoreErrors)
                            throw;
                    }
                }
            }
            catch
            {
                if (!ignoreErrors)
                    throw;
            }
        }

        /// <summary>
        /// Получение отфильтрованного по расширениям списка файлов в директории
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="extensionFilters">Фильтры расширений</param>
        /// <returns></returns>
        public static string[] GetFilteredFileList(string path, string extensionFilters)
        {
            return (from file in Directory.GetFiles(path)
                where DoesExtensionFilterMatches(file, extensionFilters)
                select Path.Combine(path, file)).ToArray();
        }

        /// <summary>
        /// Проверка на то, что путь удовлетворяет фильтру
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="extensionFilters">Фильтр </param>
        /// <returns></returns>
        public static bool DoesExtensionFilterMatches(string path, string extensionFilters)
        {
            if (string.IsNullOrEmpty(extensionFilters) || extensionFilters.Contains("*.*"))
                return true;
            
            string extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension))
                return false;
            
            string pattern = extensionFilters.Replace("*.", @"\.").Replace(" ", string.Empty);
            return Regex.IsMatch(extension, pattern, RegexOptions.IgnoreCase);
        }

        #endregion
    }
}
