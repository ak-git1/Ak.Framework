using System;

namespace Ak.Framework.Wpf.ViewModels
{
    /// <summary>
    /// ViewNodel с деструктором
    /// </summary>
    public abstract class DisposableViewModel : ViewModelBase, IDisposable
    {
        #region Переменные

        /// <summary>
        /// Декструктор запущен
        /// </summary>
        protected bool IsDisposed;

        #endregion

        #region Публичные методы

        /// <summary>
        /// Деструктор
        /// </summary>
        public virtual void Dispose()
        {
            if (!IsDisposed)
                IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
