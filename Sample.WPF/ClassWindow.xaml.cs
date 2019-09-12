using GiantappConfiger;
using GiantappConfiger.Models;
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
    public class SubT
    {
        public int MyProperty { get; set; } = 5;
    }
    public class SubT2
    {
        public string MyProperty2 { get; set; } = "s";
    }

    public class TestSetting
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public bool B2 { get; set; }
        public SubT T1 { get; set; }
        public List<SubT2> T2 { get; set; }
    }

    /// <summary>
    /// ClassWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClassWindow : Window
    {
        TestSetting tmp = new TestSetting()
        {
            Str1 = "6",
        };
        ConfigerViewModel _vm;
        public ClassWindow()
        {
            InitializeComponent();

            _vm = ConfigerService.GetVM(new object[] { tmp }, null);
            configer.DataContext = _vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tmp = ConfigerService.GetData<TestSetting>(_vm.Nodes);
        }
    }
}
