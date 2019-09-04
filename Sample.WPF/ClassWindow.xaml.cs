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
    public class TestSetting
    {
        public string Str1 { get; set; }
        public bool B2 { get; set; }
    }

    /// <summary>
    /// ClassWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClassWindow : Window
    {
        public ClassWindow()
        {
            InitializeComponent();
            configer.Init(new TestSetting()
            {
                Str1 = "test",
            });
        }
    }
}
