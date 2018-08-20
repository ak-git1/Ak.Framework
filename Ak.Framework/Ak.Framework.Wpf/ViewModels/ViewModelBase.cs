using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Ak.Framework.Wpf.ViewModels.Interfaces;

namespace Ak.Framework.Wpf.ViewModels
{
    /// <summary>
    /// Базовый класс для ViewModel
    /// </summary>
    public abstract class ViewModelBase : IViewModel
    {
        #region Переменные

        /// <summary>
        /// Режим редактирования
        /// </summary>
        private static bool? _isInDesignMode;

        /// <summary>
        /// Отложенные изменния свойств
        /// </summary>
        private readonly Stack<int> _propertyChangedDelays = new Stack<int>();

        #endregion

        #region Свойства

        /// <summary>
        /// Режим редактирования
        /// </summary>
        public bool IsInDesignMode => IsInDesignModeStatic;

        /// <summary>
        /// Режим редактирования
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
                
                return _isInDesignMode.Value;
            }
        }

        /// <summary>
        /// Проверка названия свойств
        /// </summary>
        protected bool VerifyPropertyNames { get; set; } = true;

        #endregion

        #region События

        /// <summary>
        /// Событие изменение свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Публичные методы        

        /// <summary>
        /// Установка свойства
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="storage">Хранилище</param>
        /// <param name="value">Значение</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns></returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Проверка названия свойства
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        [DebuggerStepThrough, Conditional("DEBUG")]
        public void VerifyPropertyName(string propertyName)
        {
            if (VerifyPropertyNames && !string.IsNullOrEmpty(propertyName) && (TypeDescriptor.GetProperties(this)[propertyName] == null))
                throw new ArgumentException("Invalid property name: " + propertyName, propertyName);
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Обработка события изменения свойства
        /// </summary>
        /// <param name="propertyName">Название свойства</param>
        private void HandlerPropertyChanged(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
                VerifyPropertyName(propertyName);

            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                propertyChanged(this, e);
            }
        }

        #endregion

        #region Обработчики событий

        protected IDisposable DelayPropertyChanged(int milliseconds)
        {
            return new PropertyChangedDelayer(this, milliseconds);
        }

        protected virtual void OnPropertyChanged()
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(string.Empty);
                propertyChanged(this, e);
            }
        }

        public virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            if (PropertyChanged != null)
            {
                MemberExpression body = propertyExpression.Body as MemberExpression;
                if (body == null)
                    throw new ArgumentNullException("propertyExpression");

                OnPropertyChanged(body.Member.Name);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_propertyChangedDelays.Any())
            {
                int timeout = _propertyChangedDelays.Peek();
                Task.Factory.StartNew(delegate
                {
                    Thread.Sleep(timeout);
                    HandlerPropertyChanged(propertyName);
                });
            }
            else
            {
                HandlerPropertyChanged(propertyName);
            }
        }

        #endregion

        #region Вспомогательные классы

        /// <summary>
        /// Класс для отложенного изменения 
        /// </summary>
        private class PropertyChangedDelayer : IDisposable
        {
            #region Переменные

            /// <summary>
            /// ViewModel
            /// </summary>
            private readonly ViewModelBase _viewModel;

            #endregion

            #region Конструктор

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="viewModel">ViewModel</param>
            /// <param name="milliseconds">Миллисекунжы</param>
            public PropertyChangedDelayer(ViewModelBase viewModel, int milliseconds)
            {
                if (viewModel == null)
                    throw new ArgumentNullException("viewModel");

                _viewModel = viewModel;
                _viewModel._propertyChangedDelays.Push(milliseconds);
            }

            #endregion

            #region Публичные методы

            /// <summary>
            /// Деструктор
            /// </summary>
            public void Dispose()
            {
                _viewModel._propertyChangedDelays.Pop();
            }

            #endregion
        }

        #endregion
    }
}
