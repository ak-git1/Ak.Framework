using System.Runtime.InteropServices;
using Ak.Framework.Core.Enums;

namespace Ak.Framework.Core
{
    /// <summary>
    /// Человекочитабельный размер файла
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct UserFriendlyFileSize
    {
        #region Свойства

        /// <summary>
        /// Размер
        /// </summary>
        public double SizeValue { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public MemoryUnits SizeUnit { get; set; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sizeValue">Размер</param>
        /// <param name="sizeUnit">Единица измерения</param>
        public UserFriendlyFileSize(double sizeValue, MemoryUnits sizeUnit)
        {
            this = new UserFriendlyFileSize();
            SizeValue = sizeValue;
            SizeUnit = sizeUnit;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Приведение к строковому пердставлению
        /// </summary>
        public override string ToString()
        {
            return $"{SizeValue} {SizeUnit}";
        }

        #endregion
    }
}
