using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace anowaku
{
    public class DesignModeProperties : DependencyObject
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
            nameof(Control.BackgroundProperty),
            typeof(Brush),
            typeof(DesignModeProperties),
            new FrameworkPropertyMetadata(Brushes.Transparent,
            OnBackgroundChanged));

        public static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d) &&
                d is Control control &&
                e.NewValue is Brush brush)
            {
                control.Background = brush;
            }
        }

        public static Brush GetBackground(DependencyObject dependencyObject)
        {
            return (Brush)dependencyObject.GetValue(BackgroundProperty);
        }

        public static void SetBackground(DependencyObject dependencyObject, Brush value)
        {
            dependencyObject.SetValue(BackgroundProperty, value);
        }
    }
}
