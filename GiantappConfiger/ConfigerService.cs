using GiantappConfiger.Models;
using System.Windows.Controls;
using GiantappConfiger;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;

namespace GiantappConfiger
{
    public static class ConfigerService
    {
        public static ConfigerViewModel GetVM(object config, DescriptorInfoDict descriptor = null)
        {
            return GetVM(new object[] { config }, descriptor);
        }

        public static ConfigerViewModel GetVM(object[] configs, DescriptorInfoDict descriptor)
        {
            var nodes = GetNodes(configs, descriptor);
            var vm = new ConfigerViewModel
            {
                Nodes = new ObservableCollection<ConfigItemNode>(nodes)
            };
            vm.Nodes[0].Selected = true;
            return vm;
        }

        public static List<T> GetAllData<T>(IList<ConfigItemNode> nodes)
        {
            var tmpList = new List<ExpandoObject>();
            foreach (var nodeItem in nodes)
            {
                ExpandoObject tempNodeObj = GetDataFromNode(nodeItem) as ExpandoObject;
                tmpList.Add(tempNodeObj);
            }

            var json = JsonConvert.SerializeObject(tmpList);
            JArray array = JsonConvert.DeserializeObject<JArray>(json);
            var result = array.ToObject<List<T>>();
            return result;
        }

        public static T GetData<T>(IList<ConfigItemNode> nodes)
        {
            return GetAllData<T>(nodes)[0];
        }

        #region private

        private static T DeepClone<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var type = obj.GetType();
            return (T)JsonConvert.DeserializeObject(json, type);
        }

        private static IDictionary<string, object> GetDataFromNode(ConfigItemNode nodeItem)
        {
            var result = new ExpandoObject() as IDictionary<string, object>;
            foreach (var propertyItem in nodeItem.Properties)
            {
                object tmpValue = propertyItem.Value;
                //是列表对象
                if (propertyItem.Value is ObservableCollection<object> listData)
                    tmpValue = GetListData(listData);

                result.Add(propertyItem.Descriptor.PropertyName, tmpValue);
            }

            if (nodeItem.SubNodes != null)
                foreach (var subNode in nodeItem.SubNodes)
                {
                    var subNodeObj = GetDataFromNode(subNode);
                    result.Add(subNode.Descriptor.PropertyName, subNodeObj);
                }
            return result;
        }

        private static object GetListData(ObservableCollection<object> listData)
        {
            List<object> list = new List<object>();
            //一个对象的所有属性值
            foreach (ObservableCollection<ConfigItemProperty> properties in listData)
            {
                var data = new ExpandoObject() as IDictionary<string, object>;
                foreach (ConfigItemProperty p in properties)
                    data.Add(p.Descriptor.PropertyName, p.Value);

                list.Add(data);
            }
            return list;
        }

        internal static List<ConfigItemNode> GetNodes(object[] configs, DescriptorInfoDict descriptor)
        {
            List<ConfigItemNode> result = new List<ConfigItemNode>();
            foreach (var configItem in configs)
            {
                var type = configItem.GetType();
                var attr = type.GetCustomAttribute(typeof(DescriptorAttribute)) as DescriptorAttribute;
                DescriptorInfo descInfo = GetOrCreateDescriptorInfo(type.Name, type, descriptor, attr);
                var node = GetNode(configItem, descInfo);
                result.Add(node);
            }
            return result;
        }

        private static DescriptorInfo GetOrCreateDescriptorInfo(string key, Type sourceType, DescriptorInfoDict descriptor, DescriptorAttribute attr = null)
        {
            DescriptorInfo descInfo;
            if (descriptor != null && descriptor.ContainsKey(key))
                descInfo = descriptor[key];
            else
                //生成默认描述信息
                descInfo = new DescriptorInfo();

            //定义了attribute
            if (attr != null)
            {
                descInfo.Text = attr.Text;
                descInfo.Type = attr.Type;
            }

            //补充必填信息
            if (string.IsNullOrEmpty(descInfo.Text))
                descInfo.Text = key;

            if (descInfo.Type == PropertyType.None)
                descInfo.Type = GetDefaultType(sourceType);

            if (descInfo.SourceType == null)
                descInfo.SourceType = sourceType;

            if (sourceType.IsEnum && descInfo.Options == null)
            {
                var items = sourceType.GetEnumNames();
                var options = items.Select(m => new DescriptorInfo() { Text = m, DefaultValue = Enum.Parse(sourceType, m) });
                descInfo.Options = new ObservableCollection<DescriptorInfo>(options);
            }

            DescriptorInfo.SetPropertyName(key, descInfo);
            return descInfo;
        }

        private static ConfigItemNode GetNode(object configItem, DescriptorInfo descriptor)
        {
            ConfigItemNode result = new ConfigItemNode
            {
                Properties = new ObservableCollection<ConfigItemProperty>(),
                SubNodes = new ObservableCollection<ConfigItemNode>()
            };

            var propertyInfos = configItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            foreach (var pInfo in propertyInfos)
            {
                var value = pInfo.GetValue(configItem);
                var valueDescriptor = GetOrCreateDescriptorInfo(pInfo.Name, pInfo.PropertyType, descriptor.PropertyDescriptors,
                    pInfo.GetCustomAttribute(typeof(DescriptorAttribute)) as DescriptorAttribute);
                if (IsValue(pInfo.PropertyType))
                {
                    //property
                    bool isList = IsList(pInfo.PropertyType);
                    if (value == null && isList)
                        //构造默认list
                        value = valueDescriptor.DefaultValue ?? new ObservableCollection<object>();

                    if (isList)
                        value = ConvertToListValue(value, valueDescriptor);

                    ConfigItemProperty tmpP = null;
                    if (isList)
                        tmpP = new ListItemProperty();
                    else
                        tmpP = new ConfigItemProperty();

                    tmpP.Descriptor = valueDescriptor;
                    tmpP.Value = value;

                    if (value == null && valueDescriptor.DefaultValue != null)
                        //没有值，就使用默认值
                        tmpP.Value = valueDescriptor.DefaultValue;
                    result.Properties.Add(tmpP);
                }
                else
                {
                    //node
                    if (value == null)
                        value = Activator.CreateInstance(pInfo.PropertyType);
                    var subPropertyTypes = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                    var tmpNode = GetNode(value, valueDescriptor);
                    result.SubNodes.Add(tmpNode);
                }
            }
            //拷贝，不破坏元对象。只需要保留当前级的信息，节约内存
            string oldPropertyName = descriptor.PropertyName;
            descriptor = DeepClone(descriptor);
            //因为propertyName没有 setter，所以需要重新复制
            DescriptorInfo.SetPropertyName(oldPropertyName, descriptor);
            result.Descriptor = descriptor;
            return result;
        }

        private static object ConvertToListValue(object value, DescriptorInfo d)
        {
            //list不为空，把原始数据转化为property对象
            var tmpList = value as IEnumerable;
            var listValue = new ObservableCollection<object>();
            foreach (var tmpData in tmpList)
            {
                var tmpVM = GetNode(tmpData, d);
                listValue.Add(tmpVM.Properties);
            }
            return listValue;
        }

        private static bool IsValue(Type type)
        {
            var result = type.IsPrimitive
                || type.Equals(typeof(string))
                || type.Equals(typeof(TimeSpan))
                || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                || IsList(type)
                || type.IsEnum;
            return result;
        }
        private static PropertyType GetDefaultType(Type type)
        {
            if (type == typeof(TimeSpan))
                return PropertyType.TimeSpan;
            if (type == typeof(string))
                return PropertyType.String;
            if (IsList(type))
                return PropertyType.List;
            if (type.IsEnum)
                return PropertyType.Combobox;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GenericTypeArguments[0].IsEnum)
                return PropertyType.Combobox;

            return PropertyType.String;
        }
        private static bool IsList(Type type)
        {
            bool isList = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
            return isList;
        }

        #endregion
    }
}
