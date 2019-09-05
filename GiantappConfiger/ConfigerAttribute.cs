using GiantappConfiger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantappConfiger
{
    [AttributeUsage(AttributeTargets.All)]
    public class ConfigerAttribute : Attribute
    {
        public ConfigerAttribute(Descriptor d)
        {
        }
    }
}
