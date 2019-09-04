using JsonConfiger.Models;
using System;
using Windows.ApplicationModel.Resources;
using MultiLanguageForXAML;
using Windows.UI.Xaml.Data;
namespace JsonConfiger
{
    public class NameConveter : IValueConverter
    {
        public bool ReadDesc { get; set; }

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

                if (!string.IsNullOrEmpty(cp.UID))
                {
                    string lan = GetString(cp.UID);
                    return lan;
                }

                return cp.Name;
            }

            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
