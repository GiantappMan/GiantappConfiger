using GiantappConfiger;
using GiantappConfiger.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.WPF.Data
{
    public class Custom
    {
        public int C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
    }

    public class Setting
    {
        public string S1 { get; set; }
        public int S2 { get; set; }
        public TimeSpan TS { get; set; }
        public string S3 { get; set; }
        public string S4 { get; set; }
        public Custom Custom { get; set; }
    }
}
