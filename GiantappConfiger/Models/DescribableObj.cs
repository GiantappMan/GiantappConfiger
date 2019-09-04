using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantappConfiger.Models
{
    public class DescribableObj
    {
        public string Lan { get; set; }
        public string LanKey { get; set; }
        public List<DescribableObj> SumbItems { get; set; }
    }
}
