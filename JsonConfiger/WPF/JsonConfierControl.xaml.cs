using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonConfiger.WPF
{
    /// <summary>
    /// Interaction logic for JsonConfierControl.xaml
    /// </summary>
    public partial class JsonConfierControl : UserControl
    {
        public JsonConfierControl()
        {
            InitializeComponent();
        }

        public string Column0Width
        {
            get { return (string)GetValue(Column0WidthProperty); }
            set { SetValue(Column0WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column0Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Column0WidthProperty =
            DependencyProperty.Register("Column0Width", typeof(string), typeof(JsonConfierControl), new PropertyMetadata("1*"));

        public string Column1Width
        {
            get { return (string)GetValue(Column1WidthProperty); }
            set { SetValue(Column1WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Column1Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Column1WidthProperty =
            DependencyProperty.Register("Column1Width", typeof(string), typeof(JsonConfierControl), new PropertyMetadata("4*"));



    }
}
