using System.Windows;

namespace anowaku
{
    public class BooleanToHiddenConverter :
        BooleanConverter<Visibility>
    {
        public BooleanToHiddenConverter() :
            base(Visibility.Visible, Visibility.Hidden)
        {
        }
    }
}
