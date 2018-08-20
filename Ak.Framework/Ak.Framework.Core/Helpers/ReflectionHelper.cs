using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ak.Framework.Core.Helpers
{
    /// <summary>
    /// Класс для работы с рефлексией
    /// </summary>
    public static class ReflectionHelper
    {
        #region Публичные методы

        /// <summary>
        /// Получение массива наследуемых классов
        /// </summary>
        /// <param name="baseType">Базовый тип</param>
        /// <returns></returns>
        public static Type[] GetTypesDerivedFrom(Type baseType)
        {
            Func<Type, bool> predicate = null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (baseType.IsGenericType)
                throw new NotSupportedException("Operation is not implemented for generic types.");
            
            List<Type> list = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                if (predicate == null)
                    predicate = t => !t.IsGenericType && t.IsSubclassOf(baseType);
                
                list.AddRange(types.Where(predicate));
            }
            return list.ToArray();
        }

        #endregion
    }
}
