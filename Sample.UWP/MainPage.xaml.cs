using DZY.Util.Common.Helpers;
using JsonConfiger;
using JsonConfiger.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sample.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        JCrService service = new JCrService();
        UserControl control;
        string path = System.IO.Path.Combine("Data", "test.json");
        string descPath = System.IO.Path.Combine("Data", "test.desc.json");
        public MainPage()
        {
            Loaded += MainWindow_Loaded;
            this.InitializeComponent();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //path = @"C:\Users\zy\AppData\Roaming\EyeNurse\Configs\setting.json";
            //descPath = @"E:\mscoder\github\EyeNurse\EyeNurse.Client\bin\Debug\Configs\setting.desc.json";
            var data = await JsonHelper.JsonDeserializeFromFileAsync<object>(path);
            var dataDesc = await JsonHelper.JsonDeserializeFromFileAsync<object>(descPath);
            control = service.GetView(data as JObject, dataDesc as JObject);
            //control = service.GetView(data, null);

            grid.Children.Insert(0, control);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = control.DataContext as JsonConfierViewModel;
            var data = service.GetData(vm.Nodes);
            //to uwp write file
            //path = "D:\test.json";
            //var test = await JsonHelper.JsonSerializeAsync(data, path);
            //grid.Children.RemoveAt(0);
        }
    }
}
