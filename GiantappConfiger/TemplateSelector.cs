using GiantappConfiger.Models;
using System.Windows;
using System.Windows.Controls;
namespace GiantappConfiger
{
    public class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is PropertyInfo cp))
            {
                return null;
            }

            string key = $"{cp.Type.ToString()}Editor";
            var template = ((FrameworkElement)container).FindResource(key) as DataTemplate;
            return template;
        }
    }
}
