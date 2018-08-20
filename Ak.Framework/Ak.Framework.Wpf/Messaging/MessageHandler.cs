namespace Ak.Framework.Wpf.Messaging
{
    /// <summary>
    /// Делегат обработчика сообщения
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="args">Параметры</param>
    public delegate void MessageHandler(Message message, params object[] args);
}
