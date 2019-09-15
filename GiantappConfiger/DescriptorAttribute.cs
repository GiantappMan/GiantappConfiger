using GiantappConfiger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantappConfiger
{
    [AttributeUsage(AttributeTargets.All | AttributeTargets.Property)]
    public class DescriptorAttribute : Attribute
    {
        public string Text { get; set; }
        public string TextKey { get; set; }
        public string Desc { get; set; }
        public string DescKey { get; set; }
        public PropertyType Type { get; set; }
        public object DefaultValue { get; set; }
    }
}
