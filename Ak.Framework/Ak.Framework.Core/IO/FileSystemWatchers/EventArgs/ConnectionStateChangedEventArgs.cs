using System.Data;
using Ak.Framework.Core.IO.FileSystemWatchers.Enums;

namespace Ak.Framework.Core.IO.FileSystemWatchers.EventArgs
{
    /// <summary>
    /// Аргументы события изменения состояния наблюдателя файловой системы
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ConnectionStateChangedEventArgs : System.EventArgs
    {
        #region Свойства

        /// <summary>
        /// Состояние соединения
        /// </summary>
        public FileSystemWatcherConnectionStates ConnectionState { get; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionState">Состояние соединения</param>
        public ConnectionStateChangedEventArgs(FileSystemWatcherConnectionStates connectionState)
        {
            ConnectionState = connectionState;
        }

        #endregion
    }
}
