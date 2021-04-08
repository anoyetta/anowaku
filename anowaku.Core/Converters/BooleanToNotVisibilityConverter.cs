using System.Windows;

namespace anowaku
{
    public class BooleanToNotVisibilityConverter :
        BooleanConverter<Visibility>
    {
        public BooleanToNotVisibilityConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        {
        }
    }
}
