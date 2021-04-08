using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace anowaku
{
    /// <summary>
    /// Dialog.xaml の相互作用ロジック
    /// </summary>
    public partial class Dialog : MetroWindow
    {
        public Dialog()
        {
            this.InitializeComponent();
            this.Loaded += this.Dialog_Loaded;
        }

        public Button OkButton => this.InnerOkButton;
        public Button CancelButton => this.InnerCancelButton;

        public new UIElement Content
        {
            get => this.ContentPanel.Children.Count > 0 ? this.ContentPanel.Children[0] : null;
            set
            {
                this.ContentPanel.Children.Clear();
                this.ContentPanel.Children.Add(value);
            }
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.InnerOkButton.Click += (x, y) => this.DialogResult = true;
        }
    }
}
