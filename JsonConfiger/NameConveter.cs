using JsonConfiger.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Windows.ApplicationModel.Resources;
using MultiLanguageForXAML;

#if WINDOWS_UWP
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
#endif
namespace JsonConfiger
{
    public class NameConveter : IValueConverter
    {
        public bool ReadDesc { get; set; }
#if WINDOWS_UWP

        private static ResourceLoader _loader;
        private static ResourceLoader CurrentResourceLoader
        {
            get { return _loader ?? (_loader = ResourceLoader.GetForCurrentView("Resources")); }
        }
        public static string GetString(string key)
        {
            string s = CurrentResourceLoader.GetString(key);
            return s;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
#else
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
#pragma warning disable CS0436 // Type conflicts with imported type
            if (value is CBaseObj)
            {
                CBaseObj cp = value as CBaseObj;
#pragma warning restore CS0436 // Type conflicts with imported type

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

#if WINDOWS_UWP
                if (!string.IsNullOrEmpty(cp.UID))
                {
                    string lan = GetString(cp.UID);
                    return lan;
                }
#endif

                return cp.Name;
            }

            return value;
        }
#if WINDOWS_UWP
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#else
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
        {
            throw new NotImplementedException();
        }
    }
}
