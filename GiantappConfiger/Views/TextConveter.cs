using GiantappConfiger.Models;
using System;
using System.Globalization;
using MultiLanguageForXAML;
using System.Windows.Data;
namespace GiantappConfiger
{
    public class TextConveter : IValueConverter
    {
        public bool ReadDesc { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DescriptorInfo d = null;
            if (value is ConfigItemNode node)
                d = node.Descriptor;
            else if (value is ConfigItemProperty property)
                d = property.Descriptor;
            else if (value is DescriptorInfo)
                d = value as DescriptorInfo;

            if (d != null)
            {
                if (ReadDesc)
                {
                    if (!string.IsNullOrEmpty(d.Desc))
                        return d.Desc;

                    if (!string.IsNullOrEmpty(d.DescKey))
                    {
                        string lan = LanService.Get(d.DescKey).Result;
                        return lan;
                    }
                    return null;
                }
                else
                {
                    if (!string.IsNullOrEmpty(d.Text))
                        return d.Text;

                    if (!string.IsNullOrEmpty(d.TextKey))
                    {
                        string lan = LanService.Get(d.TextKey).Result;
                        return lan;
                    }
                }
                //return d.Name;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
