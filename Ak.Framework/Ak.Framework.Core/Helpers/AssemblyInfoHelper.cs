using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для получения информации о сборке
    /// </summary>
    public class AssemblyInfoHelper
    {
        #region Публичные методы

        /// <summary>
        /// Получение списка используемых программных библиотек
        /// </summary>
        public static List<string> GetAssembliesList()
        {
            List<string> result = new List<string>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().OrderBy(a => a.FullName).ToList())
                result.Add(assembly.FullName);
            return result;
        }

        /// <summary>
        /// Получение даты создания основной сборки
        /// </summary>
        public static DateTime GetMainAssemblyCreationDate()
        {
            return File.GetCreationTime(Assembly.GetEntryAssembly().Location);
        }

        /// <summary>
        /// Получение даты последенего изменения основной сборки
        /// </summary>
        public static DateTime GetMainAssemblyModifyDate()
        {
            return File.GetLastWriteTime(Assembly.GetEntryAssembly().Location);
        }

        /// <summary>
        /// Получение версии основной сборки
        /// </summary>
        public static string GetMainAssemblyVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Получение даты сборки
        /// </summary>
        /// <returns></returns>
        public static DateTime GetBuildDate()
        {
            string filePath = Assembly.GetCallingAssembly().Location;
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            using (Stream s = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                s.Read(b, 0, 2048);
            }

            int secondsSince1970 = BitConverter.ToInt32(b, BitConverter.ToInt32(b, peHeaderOffset) + linkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

        #endregion
    }
}
