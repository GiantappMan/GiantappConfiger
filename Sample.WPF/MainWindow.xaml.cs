using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sample.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnClass_Click(object sender, RoutedEventArgs e)
        {
            var window = new ClassWindow();
            window.ShowDialog();
        }

        private void btnJson_Click(object sender, RoutedEventArgs e)
        {
            var window = new JsonWindow();
            window.ShowDialog();
        }
    }
}
