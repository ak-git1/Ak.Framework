using System;

namespace Ak.Framework.Core.Utils
{
    /// <summary>
    /// Класс для работы со сборщиком мусора
    /// </summary>
    public class GarbageCollector
    {
        /// <summary>
        /// Сбор мусора
        /// </summary>
        /// <param name="x86Only">Применить только для x86</param>
        public static void Collect(bool x86Only = true)
        {
            try
            {
                if (!x86Only || !Environment.Is64BitProcess)
                {
                    GC.Collect();
                    GC.Collect();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.WaitForFullGCComplete();
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}
