using System.Web;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы с Url
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        /// Получение Url веб-приложения
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationPath()
        {
            string appPath = null;
            HttpContext context = HttpContext.Current;

            if (context != null)
                appPath = $"{context.Request.Url.Scheme}://{context.Request.Url.Host}{(context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port)}{context.Request.ApplicationPath}";
            if (appPath != null && !appPath.EndsWith("/"))
                appPath += "/";
            return appPath;
        }
    }
}
