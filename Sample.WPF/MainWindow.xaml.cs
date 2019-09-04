using DZY.Util.Common.Helpers;
using JsonConfiger;
using JsonConfiger.Models;
using MultiLanguageForXAML;
using Newtonsoft.Json.Linq;
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
    public partial class MainWindow : Window
    {
        JCrService service = new JCrService();
        UserControl control;
        readonly string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.json");
        readonly string defaultPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.default.json");
        readonly string descPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.desc.json");
        public MainWindow()
        {
            InitializeComponent();

            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Data\\Languages");
            LanService.Init(new JsonDB(path), true);

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await JsonHelper.JsonDeserializeFromFileAsync<object>(path);
            var defaultData = await JsonHelper.JsonDeserializeFromFileAsync<object>(defaultPath);
            var dataDesc = await JsonHelper.JsonDeserializeFromFileAsync<object>(descPath);

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

            service.InjectDescObjs("$screen", extraDescObjs);

            data = JCrService.CheckDefault(data as JObject, defaultData as JObject);

            control = service.GetView(data as JObject, dataDesc as JObject);
            control.BorderBrush = new SolidColorBrush(Colors.Red);

            grid.Children.Insert(0, control);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = control.DataContext as JsonConfierViewModel;
            var data = service.GetData(vm.Nodes);
            _ = await JsonHelper.JsonSerializeAsync(data, path);
        }
    }
}
