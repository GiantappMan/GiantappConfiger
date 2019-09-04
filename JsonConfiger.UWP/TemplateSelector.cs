using JsonConfiger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows.Controls;
#endif
namespace JsonConfiger
{
#if WINDOWS_UWP
    public class TempateItem
    {
        public string Key { get; set; }
        public DataTemplate Template { get; set; }
    }
#endif
    public class TemplateSelector : DataTemplateSelector
    {
#if WINDOWS_UWP
        public List<TempateItem> DataTemplates { get; set; } = new List<TempateItem>();

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
#else 
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
#endif
        {
#pragma warning disable CS0436 // Type conflicts with imported type
            if (!(item is CProperty cp))
#pragma warning restore CS0436 // Type conflicts with imported type
            {
                return null;
            }

            string key = $"{cp.CType.ToString()}Editor";
#if WINDOWS_UWP
            var template = (DataTemplates.FirstOrDefault(m => m.Key == key)?.Template);

#else
            var template = ((FrameworkElement)container).FindResource(key) as DataTemplate;
#endif
            return template;
        }
    }
}
