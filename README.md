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
//.cs
```
            var tmp = new TestSetting()
            {
                Str1 = "6",
            };
            var vm = ConfigerService.GetVM(new object[] { tmp }, null);
            configer.DataContext = vm;
```
