namespace Ak.Framework.Core.IO.FileSystemWatchers.Enums
{
    /// <summary>
    /// Состояния соединения 
    /// </summary>
    public enum FileSystemWatcherConnectionStates
    {
        /// <summary>
        /// Соединение с файловой системой установлено
        /// </summary>
        Connected = 0,
        
        /// <summary>
        /// Соединение с файловой системой нарушено
        /// </summary>
        Disconnected = 1,

        /// <summary>
        /// Псевдосостостояние для индикации того, что наблюдатель файловой системы был пересоединен.
        /// (Наблюдатель никогда не будет в данном состоянии)
        /// </summary>
        Reconnected = 2
    }
}
