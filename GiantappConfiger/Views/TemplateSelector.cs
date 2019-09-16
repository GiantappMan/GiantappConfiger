using GiantappConfiger.Models;
using System.Windows;
using System.Windows.Controls;
namespace GiantappConfiger
{
    public class TemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is ConfigItemProperty cp))
            {
                return null;
            }

            string templateName = null;
            if (cp.Descriptor.Type == PropertyType.None)
                //默认模板
                templateName = PropertyType.String.ToString();
            else
                templateName = cp.Descriptor.Type.ToString();

            string key = $"{templateName}Editor";
            var template = ((FrameworkElement)container).FindResource(key) as DataTemplate;
            return template;
        }
    }
}
