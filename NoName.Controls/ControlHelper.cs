using System.Windows;
using System.Windows.Media;

namespace NoName.Controls
{
    public class ControlHelper
    {
        public static T GetParent<T>(FrameworkElement element) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(element);
            if (parent == null)
            {
                return null;
            }
            var fe = parent as T;
            return fe ?? GetParent<T>(parent as FrameworkElement);
        }
    }
}
