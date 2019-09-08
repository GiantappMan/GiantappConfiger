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

namespace GiantappConfiger
{
    public class ConfigerService
    {
        public ConfigerViewModel GetVM(object[] configs, DescriptorInfoDict descriptor)
        {
            var nodes = GetNodes(configs, descriptor);
            var vm = new ConfigerViewModel
            {
                Nodes = new ObservableCollection<ConfigItemNode>(nodes)
            };
            vm.Nodes[0].Selected = true;
            return vm;
        }

        public List<T> GetData<T>(IList<ConfigItemNode> nodes)
        {
            //var tmp = new ExpandoObject() as IDictionary<string, object>;
            var tmpList = new List<ExpandoObject>();
            foreach (var nodeItem in nodes)
            {
                ExpandoObject tempNodeObj = GetDataFromNode(nodeItem) as ExpandoObject;
                //var tmp = new ExpandoObject() as IDictionary<string, object>;

                //tmp.Add(nodeItem.Descriptor.PropertyName, tempNodeObj);
                tmpList.Add(tempNodeObj);
            }

            var json = JsonConvert.SerializeObject(tmpList);
            JArray array = JsonConvert.DeserializeObject<JArray>(json);
            var result = array.ToObject<List<T>>();
            return result;
        }

        #region private

        private static T DeepClone<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var type = obj.GetType();
            return (T)JsonConvert.DeserializeObject(json, type);
        }

        private IDictionary<string, object> GetDataFromNode(ConfigItemNode nodeItem)
        {
            var result = new ExpandoObject() as IDictionary<string, object>;
            foreach (var propertyItem in nodeItem.Properties)
            {
                result.Add(propertyItem.Descriptor.PropertyName, propertyItem.Value);
            }

            if (nodeItem.SubNodes != null)
                foreach (var subNode in nodeItem.SubNodes)
                {
                    var subNodeObj = GetDataFromNode(subNode);
                    result.Add(subNode.Descriptor.PropertyName, subNodeObj);
                }
            return result;
        }

        private List<ConfigItemNode> GetNodes(object[] configs, DescriptorInfoDict descriptor)
        {
            List<ConfigItemNode> result = new List<ConfigItemNode>();
            foreach (var configItem in configs)
            {
                var type = configItem.GetType();

                DescriptorInfo descInfo = GetOrCreateDescriptorInfo(type.Name, descriptor);
                var node = GetNode(configItem, descInfo);
                result.Add(node);
            }
            return result;
        }

        private DescriptorInfo GetOrCreateDescriptorInfo(string key, DescriptorInfoDict descriptor)
        {
            DescriptorInfo descInfo;
            if (descriptor.ContainsKey(key))
                descInfo = descriptor[key];
            else
                descInfo = new DescriptorInfo() { Text = key };//生成默认描述信息
            DescriptorInfo.SetPropertyName(key, descInfo);
            return descInfo;
        }

        private string GetDataKey(object obj)
        {
            var type = obj.GetType();
            string key = type.Name;
            return key;
        }

        private ConfigItemNode GetNode(object configItem, DescriptorInfo descriptor)
        {
            ConfigItemNode result = new ConfigItemNode();
            result.Properties = new ObservableCollection<ConfigItemProperty>();
            result.SubNodes = new ObservableCollection<ConfigItemNode>();

            var propertyTypes = configItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            foreach (var pType in propertyTypes)
            {
                //property
                var isValue = IsValue(pType.PropertyType);
                var value = pType.GetValue(configItem);
                var valueDescriptor = GetOrCreateDescriptorInfo(pType.Name, descriptor.PropertyDescriptors);
                if (isValue)
                {
                    var tmpP = new ConfigItemProperty()
                    {
                        Descriptor = valueDescriptor,
                        Value = value
                    };
                    if (valueDescriptor.DefaultValue != null && value == null)
                        tmpP.Value = valueDescriptor.DefaultValue;
                    result.Properties.Add(tmpP);
                }
                else
                {
                    //node
                    if (value == null)
                        value = Activator.CreateInstance(pType.PropertyType);
                    var subPropertyTypes = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                    var tmpNode = GetNode(value, valueDescriptor);
                    result.SubNodes.Add(tmpNode);
                    //foreach (var subType in subPropertyTypes)
                    //{
                    //    var subValue = subType.GetValue(value);
                    //    if (subValue == null && !IsValue(subType.PropertyType))
                    //        subValue = Activator.CreateInstance(subType.PropertyType);
                    //    var subDescInfo = GetOrCreateDescriptorInfo(subType.Name, descriptor.PropertyDescriptors);
                    //    var tmpNode = GetNode(subValue, subDescInfo);
                    //    result.SubNodes.Add(tmpNode);
                    //}
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

        //private ConfigItemNode GetNodeDepth0(object data, DescriptorInfo descriptorInfo)
        //{
        //    var node = new ConfigItemNode();
        //    if (descriptorInfo == null)
        //        descriptorInfo = new DescriptorInfo() { Text = data.GetType().Name };

        //    var propertyTypes = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
        //    node.Properties = GetProperties(data, propertyTypes, descriptorInfo.PropertyDescriptors);
        //    var subNodes = GetSubNodes(data, propertyTypes, descriptorInfo.PropertyDescriptors);
        //    node.SubNodes = new ObservableCollection<ConfigItemNode>(subNodes);

        //    //拷贝，不破坏元对象。只需要保留当前级的信息，节约内存
        //    descriptorInfo = DeepClone(descriptorInfo);
        //    descriptorInfo.PropertyDescriptors = null;
        //    node.Descriptor = descriptorInfo;

        //    return node;
        //}

        //private IList<ConfigItemNode> GetSubNodes(object data, PropertyInfo[] propertyTypes, Dictionary<string, DescriptorInfo> propertyDescriptors)
        //{
        //    var result = new List<ConfigItemNode>();
        //    foreach (var pType in propertyTypes)
        //    {
        //        ConfigItemNode node = new ConfigItemNode();

        //        DescriptorInfo subDescriptor = null;
        //        if (propertyDescriptors != null && propertyDescriptors.ContainsKey(pType.Name))
        //            subDescriptor = propertyDescriptors[pType.Name];
        //        else
        //            subDescriptor = new DescriptorInfo() { Text = pType.Name };
        //        var property = new ConfigItemProperty
        //        {
        //            Descriptor = subDescriptor
        //        };

        //        var isValue = IsValue(pType.PropertyType);
        //        if (isValue)
        //            continue;

        //        if (property.Value == null)
        //            //子对象
        //            property.Value = Activator.CreateInstance(pType.PropertyType);

        //        propertyTypes = property.Value.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
        //        var subNodes = GetSubNodes(property.Value, propertyTypes, subDescriptor.PropertyDescriptors);
        //        if (subNodes != null)
        //            node.SubNodes = new ObservableCollection<ConfigItemNode>(subNodes);
        //        node.Properties = GetProperties(property.Value, propertyTypes, subDescriptor.PropertyDescriptors);
        //        result.Add(node);
        //    }
        //    return result;
        //}

        //private ObservableCollection<ConfigItemProperty> GetProperties(object data, PropertyInfo[] instanceProperties, Dictionary<string, DescriptorInfo> descriptors)
        //{
        //    var tmpProperties = new ObservableCollection<ConfigItemProperty>();
        //    foreach (var pType in instanceProperties)
        //    {
        //        DescriptorInfo subDescriptor = null;
        //        if (descriptors != null && descriptors.ContainsKey(pType.Name))
        //        {
        //            subDescriptor = descriptors[pType.Name];
        //        }
        //        else
        //            subDescriptor = new DescriptorInfo() { Text = pType.Name };
        //        var property = new ConfigItemProperty
        //        {
        //            Descriptor = subDescriptor
        //        };

        //        var isValue = IsValue(pType.PropertyType);
        //        if (isValue)
        //        {
        //            var currentValue = pType.GetValue(data);
        //            property.Value = currentValue;
        //            if (property != null)
        //                tmpProperties.Add(property);
        //        }
        //        else
        //        {
        //            //if (property.Value == null)
        //            //{
        //            //    //子对象
        //            //    property.Value = Activator.CreateInstance(pType.PropertyType);
        //            //    if (node.SubNodes == null)
        //            //        node.SubNodes = new ObservableCollection<ConfigItemNode>();

        //            //    var subNodes = GetNode(property.Value, subDescriptor);
        //            //    if (subNodes != null)
        //            //        node.SubNodes = new ObservableCollection<ConfigItemNode>(subNodes);
        //            //}
        //        }
        //    }
        //    return tmpProperties;
        //}

        private bool IsValue(Type type)
        {
            return type.IsPrimitive || type.Equals(typeof(string));
        }

        #endregion
    }
}
