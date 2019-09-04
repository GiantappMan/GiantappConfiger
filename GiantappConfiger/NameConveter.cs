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
            if (value is CBaseObj)
            {
                CBaseObj cp = value as CBaseObj;

                if (ReadDesc)
                {
                    if (!string.IsNullOrEmpty(cp.Desc))
                        return cp.Desc;

                    if (!string.IsNullOrEmpty(cp.DescLanKey))
                    {
                        string lan = LanService.Get(cp.DescLanKey).Result;
                        return lan;
                    }
                    return null;
                }
                else
                {
                    if (!string.IsNullOrEmpty(cp.Lan))
                        return cp.Lan;

                    if (!string.IsNullOrEmpty(cp.LanKey))
                    {
                        string lan = LanService.Get(cp.LanKey).Result;
                        return lan;
                    }
                }
                return cp.Name;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
