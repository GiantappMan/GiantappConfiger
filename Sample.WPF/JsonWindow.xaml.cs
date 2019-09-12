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
using System.Threading;
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
        static JsonWindow()
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Data\\Languages");
            LanService.Init(new JsonDB(path), true);
        }
        readonly string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.json");
        readonly string descPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Data", "test.desc.json");
        public JsonWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await JsonHelper.JsonDeserializeFromFileAsync<Setting>(path);
            var dataDesc = await JsonHelper.JsonDeserializeFromFileAsync<DescriptorInfoDict>(descPath);
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
            configer.DataContext = ConfigerService.GetVM(new object[] { data }, dataDesc);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = configer.DataContext as ConfigerViewModel;
            var data = ConfigerService.GetData<Setting>(vm.Nodes);
            var setting = await JsonHelper.JsonSerializeAsync(data, path);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(data.S3);
            await LanService.UpdateLanguage();
        }
    }
}
