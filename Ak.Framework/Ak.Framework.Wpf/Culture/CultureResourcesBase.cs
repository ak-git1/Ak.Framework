using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using Ak.Framework.Core.Extensions;

namespace Ak.Framework.Wpf.Culture
{
    /// <summary>
    /// Базовый класс для классов используемых для получения локализованных текстов в виде привязанных данных.
    /// Используется для обеспечения возможности смены культуры без перезапуска приложения.
    /// Наследующие классы должны добавить публичный метод, возвращающий ресурсы и конструктор, вызывающий конструктор этого класса с правильными параметрами.
    /// </summary>
    public abstract class CultureResourcesBase
    {
        #region Переменные

        /// <summary>
        /// Список наименований зарегистрированных ресурсов
        /// </summary>
        private static readonly ConcurrentBag<string> ResourceNames = new ConcurrentBag<string>();

        /// <summary>
        /// Список типов зарегистрированных ресурсов (классы типа Resources.Designer)
        /// </summary>
        private static readonly ConcurrentBag<Type> ResourceTypes = new ConcurrentBag<Type>();

        #endregion

        #region Методы

        /// <summary>
        /// Изменение текущей культуры
        /// </summary>
        /// <param name="culture">Новая культура</param>
        public static void ChangeCulture(CultureInfo culture)
        {
            ResourceTypes.ForEach(t =>
            {
                t.GetProperty("Culture", BindingFlags.Public | BindingFlags.Static)?.SetValue(null, culture);
            });

            Thread.CurrentThread.CurrentCulture = culture;
            Application.Current?.Dispatcher?.Invoke(() => Thread.CurrentThread.CurrentCulture = culture);

            ResourceNames.ForEach(r =>
            {
                (Application.Current.FindResource(r) as ObjectDataProvider)?.Refresh();
            });
        }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя ресурса</param>
        /// <param name="resourceType">Тип ресурсов (класс типа Resources.Designer)</param>
        protected CultureResourcesBase(string name, Type resourceType)
        {
            ResourceNames.Add(name);
            ResourceTypes.Add(resourceType);
        }

        #endregion
    }
}
