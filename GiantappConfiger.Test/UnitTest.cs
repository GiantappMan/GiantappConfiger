using GiantappConfiger.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace GiantappConfiger.Test
{
    class TestSetting
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
        public bool P3 { get; set; }
        public string P4 { get; set; }
        public SubSetting SubSetting { get; set; }
    }

    class SubSetting
    {
        public string SP1 { get; set; }
        public string SP2 { get; set; }
    }

    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestCheckDefault()
        {
            TestSetting setting = new TestSetting()
            {
                P1 = 1,
                P2 = null,
                P3 = true
            };

            var descriptor = new Descriptor()
            {
                SumbItems = new Dictionary<string, Descriptor>() {
                    {"P1", new Descriptor(){ Text="int property"} },
                    {"P2", new Descriptor(){ Text="string property",DefaultValue="xxx"} },
                    {"SubSetting", new Descriptor(){ Text="string property",DefaultValue=new SubSetting(),
                        SumbItems=new Dictionary<string, Descriptor>()
                        {
                            {"SP1", new Descriptor(){ Text="sub int property"} },
                            {"SP2", new Descriptor(){ Text="sub string property",DefaultValue="ooo"} }
                        }}
                    },
                }
            };
            //setting = (TestSetting)ConfigerService.CheckDefault(setting, descriptor);
            Assert.IsTrue(setting.P1 == 1);
            Assert.IsTrue(setting.P2 == "xxx");
            Assert.IsTrue(setting.P3 == true);
            Assert.IsTrue(setting.P4 == null);
            Assert.IsTrue(setting.SubSetting.SP1 == null);
            Assert.IsTrue(setting.SubSetting.SP2 == "ooo");
        }

        [TestMethod]
        public void TestGetVM()
        {
            ConfigerService service = new ConfigerService();
            TestSetting setting = new TestSetting()
            {
                P1 = 1,
                P2 = null,
                P3 = true
            };

            var descriptor = new Descriptor()
            {
                Text = "node 0",
                SumbItems = new Dictionary<string, Descriptor>() {
                    {"P1", new Descriptor(){ Text="int property"} },
                    {"P2", new Descriptor(){ Text="string property",DefaultValue="xxx"} },
                    {"SubSetting", new Descriptor(){ Text="string property",DefaultValue=new SubSetting(),
                        SumbItems=new Dictionary<string, Descriptor>()
                        {
                            {"SP1", new Descriptor(){ Text="sub int property"} },
                            {"SP2", new Descriptor(){ Text="sub string property",DefaultValue="ooo"} }
                        }}
                    },
                }
            };
            var vm = service.GetVM(setting, descriptor);
            Assert.IsTrue(vm.Nodes.Count == 1);
            Assert.IsTrue(vm.Nodes[0].Selected);
            Assert.IsTrue(vm.Nodes[0].Properties.Count == 4);
            Assert.IsTrue(vm.Nodes[0].Descriptor.Text == "node 0");
            var p1 = vm.Nodes[0].Properties[0];
            Assert.IsTrue(p1.Descriptor.Text == "int property");
            Assert.IsTrue((int)p1.Value == 1);
        }
    }
}
