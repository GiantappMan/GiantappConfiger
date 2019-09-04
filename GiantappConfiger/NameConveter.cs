using GiantappConfiger.Models;
using System;
using System.Globalization;
using MultiLanguageForXAML;
using System.Windows.Data;
namespace GiantappConfiger
{
    public class NameConveter : IValueConverter
    {
        public bool ReadDesc { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Descriptor d)
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

                    if (!string.IsNullOrEmpty(d.Text))
                    {
                        string lan = LanService.Get(d.TextKey).Result;
                        return lan;
                    }
                }
                return d.Name;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
