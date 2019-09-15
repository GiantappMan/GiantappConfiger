# GiantappConfiger
懒人专用配置界面生成器，根据传入对象自动生成UI界面
只支持WPF。UWP等微软Net5出了再看

# 支持功能
- [x] 给任意对象，自动生成配置UI
- [x] 多语言
- [x] 外部文件描述

# 用法
//.xaml
```
        <giantappconfiger:ConfigControl x:Name="configer" />
```

//描述文件格式，可不填
```
            var descriptor = new DescriptorInfoDict()
            {
                {"TestSetting",
                    new DescriptorInfo(){
                        Text = "node 0",
                        PropertyDescriptors = new DescriptorInfoDict() {
                            {"P1", new DescriptorInfo(){ Text="int property"} },
                            {"P2", new DescriptorInfo(){ Text="string property",DefaultValue="xxx"} },
                            {"SubSetting", new DescriptorInfo(){ Text="string property",DefaultValue=new SubSetting(),
                                PropertyDescriptors=new DescriptorInfoDict()
                                {
                                    {"SP1", new DescriptorInfo(){ Text="sub int property"} },
                                    {"SP2", new DescriptorInfo(){ Text="sub string property",DefaultValue="ooo"} }
                                }}
                            },
                        }
                    }}
            };
```

//.cs 获取vm， descriptor可传null
```
            var tmp = new TestSetting()
            {
                Str1 = "6",
            };
            var vm = ConfigerService.GetVM(tmp, descriptor);
            configer.DataContext = vm;

			//UI操作后，获取数据
            tmp = ConfigerService.GetData<TestSetting>(vm.Nodes);

```
