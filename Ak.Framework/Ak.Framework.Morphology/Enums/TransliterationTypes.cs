using System.ComponentModel;

namespace Ak.Framework.Morphology.Enums
{
    /// <summary>
    /// Стандарты транслитерации
    /// </summary>
    public enum TransliterationTypes
    {
        /// <summary>
        /// ГОСТ 16876-71
        /// </summary>
        [Description("ГОСТ 16876-71")]
        Gost,

        /// <summary>
        /// ISO 9-95
        /// </summary>
        [Description("ISO 9-95")]
        Iso
    }
}
