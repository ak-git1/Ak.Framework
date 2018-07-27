using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ak.Framework.Core.IO.Enums;
using Ak.Framework.Core.Helpers;

namespace Ak.Framework.Core.IO
{
    /// <summary>
    /// Класс для работы с путями в файловой системе
    /// </summary>
    public static class PathHelper
    {
        #region Публиные методы

        /// <summary>
        /// Проверка на то, что путьт в файловой системе существует
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return GetPathTargetType(path) != PathTargetTypes.Unknown;
        }

        /// <summary>
        /// Получение типа объета файловой системы по пути
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns></returns>
        public static PathTargetTypes GetPathTargetType(string path)
        {
            ThrowIfInvalid(path);
            bool exists = new FileInfo(path).Exists;
            PathTargetTypes pathTargetType = exists || new DirectoryInfo(path).Exists
                ? (exists ? PathTargetTypes.File : PathTargetTypes.Directory)
                : PathTargetTypes.Unknown;
            return pathTargetType;
        }

        /// <summary>
        /// Проверка на то, что указан внешний путь (например, http url)
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns></returns>
        public static bool IsExternal(string path)
        {
            ThrowIfInvalid(path);
            return path.StartsWith(@"\\");
        }

        /// <summary>
        /// Вызов исключения, если путь некорректен
        /// </summary>
        /// <param name="path">Путь</param>
        public static void ThrowIfInvalid(string path)
        {
            ThrowHelper.CheckTrue(IsPathValid(path), "Invalid path specified.");
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Проверка на то, что путь файловой системы указан корректно
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns></returns>
        private static bool IsPathValid(string path)
        {
            IEnumerable<char> chrs = Path.GetInvalidPathChars().Union(Path.GetInvalidFileNameChars());
            return string.IsNullOrWhiteSpace(path) || path.Any(chrs.Contains);
        }

        #endregion
    }
}
