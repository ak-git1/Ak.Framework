using System.IO;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Core.IO.Extensions
{
    /// <summary>
    /// Расширения для работы с путями
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// Замена расширения
        /// </summary>
        /// <param name="sourcePath">Исходный путь</param>
        /// <param name="targetExtension">Расширение конеченое</param>
        /// <returns></returns>
        public static string ReplaceExtension(this string sourcePath, string targetExtension)
        {
            if (sourcePath.IsNullOrEmpty())
                return sourcePath;

            try
            {
                char[] chrArray = { ' ', '.' };
                return Path.Combine(Path.GetDirectoryName(sourcePath), string.Concat(Path.GetFileNameWithoutExtension(sourcePath), ".", targetExtension.Trim(chrArray)));
            }
            catch
            {
                return sourcePath;
            }
        }
    }
}
