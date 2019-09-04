using GiantappConfiger.Models;
using System.Windows;
using System.Windows.Controls;
namespace GiantappConfiger
{
    public class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is CProperty cp))
            {
                return null;
            }

            string key = $"{cp.CType.ToString()}Editor";
            var template = ((FrameworkElement)container).FindResource(key) as DataTemplate;
            return template;
        }
    }
}
