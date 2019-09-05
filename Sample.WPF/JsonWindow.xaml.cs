using DZY.Util.Common.Helpers;
using GiantappConfiger;
using GiantappConfiger.Models;
using MultiLanguageForXAML;
using Newtonsoft.Json.Linq;
using Sample.WPF.Data;
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

namespace Sample.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class JsonWindow : Window
    {
        readonly ConfigerService service = new ConfigerService();
        UserControl control;
        readonly string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.json");
        readonly string defaultPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.default.json");
        readonly string descPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.desc.json");
        public JsonWindow()
        {
            InitializeComponent();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Data\\Languages");
            LanService.Init(new JsonDB(path), true);

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await JsonHelper.JsonDeserializeFromFileAsync<Setting>(path);
            var dataDesc = await JsonHelper.JsonDeserializeFromFileAsync<DescriptorInfo>(descPath);
            List<dynamic> extraDescObjs = new List<dynamic>();

            extraDescObjs.Add(new
            {
                lan = string.Format($"禁用"),
                value = -1
            });
            for (int i = 0; i < System.Windows.Forms.Screen.AllScreens.Length; i++)
            {
                extraDescObjs.Add(new
                {
                    lan = string.Format($"屏幕{i}"),
                    value = i
                });
            }
            control = service.GetView(data, dataDesc);

            grid.Children.Insert(0, control);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = control.DataContext as ConfigerViewModel;
            var data = service.GetData(vm.Nodes);
            _ = await JsonHelper.JsonSerializeAsync(data, path);
        }
    }
}
