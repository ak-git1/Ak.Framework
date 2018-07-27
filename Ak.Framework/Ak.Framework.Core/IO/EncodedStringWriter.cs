using System.IO;
using System.Text;

namespace Ak.Framework.Core.IO
{
    /// <summary>
    /// Расширение для StringWriter содержащее кодировку
    /// </summary>
    /// <seealso cref="System.IO.StringWriter" />
    public class EncodedStringWriter : StringWriter
    {
        #region Свойства

        /// <summary>
        /// Кодировка
        /// </summary>
        public override Encoding Encoding { get; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="stringBuilder">StringBuilder</param>
        /// <param name="encoding">Кодировка</param>
        public EncodedStringWriter(StringBuilder stringBuilder, Encoding encoding) : base(stringBuilder)
        {
            Encoding = encoding;
        }

        #endregion
    }
}
