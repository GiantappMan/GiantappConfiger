using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GiantappConfiger.ValidationRules
{
    public enum LimitType
    {
        Interget,
        Float,
        TimeSpan
    }

    public class TextBoxValidationRule : ValidationRule
    {
        public LimitType LimitType { get; set; }
        public string Error { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = null;
            if (value != null)
                text = value.ToString();

            bool result = false;

            switch (LimitType)
            {
                case LimitType.Interget:
                    string pattern = @"^(\d+)$";
                    Regex regex = new Regex(pattern);
                    result = regex.IsMatch(text);
                    break;

                case LimitType.Float:
                    pattern = @"^([0-9]+(.[0-9]+)?)$";
                    regex = new Regex(pattern);
                    result = regex.IsMatch(text);
                    break;
                case LimitType.TimeSpan:
                    pattern = @"^(\d\d):(60|([0-5][0-9])):(60|([0-5][0-9]))$";
                    regex = new Regex(pattern);
                    result = regex.IsMatch(text);
                    break;
            }

            if (!result)
                return new ValidationResult(false, Error);

            return new ValidationResult(true, null);
        }
    }
}
