using System.Windows;
using System.Windows.Media;

namespace Ak.Framework.Wpf.Extensions
{
    /// <summary>
    /// Extensions for dependency object
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Gets the parent object
        /// </summary>
        /// <param name="child">Child element</param>
        /// <returns></returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            if (child is ContentElement contentElement)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce?.Parent;
            }

            if (child is FrameworkElement frameworkElement)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null)
                    return parent;
            }

            return VisualTreeHelper.GetParent(child);
        }
    }
}
