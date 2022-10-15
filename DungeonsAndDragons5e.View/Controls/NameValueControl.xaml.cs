using System.Windows;
using System.Windows.Controls;

namespace DungeonsAndDragons5e.View.Controls
{
    public partial class NameValueControl : UserControl
    {
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(NameValueControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty PropertyValueProperty = DependencyProperty.Register("PropertyValue", typeof(int), typeof(NameValueControl), new PropertyMetadata(default(int)));

        public string PropertyName
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public int PropertyValue
        {
            get => (int)GetValue(PropertyValueProperty);
            set => SetValue(PropertyValueProperty, value);
        }

        public NameValueControl()
        {
            InitializeComponent();
        }
    }
}