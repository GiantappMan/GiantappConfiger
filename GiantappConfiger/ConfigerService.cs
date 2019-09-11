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
        public ConfigerViewModel GetVM(object config, DescriptorInfoDict descriptor)
        {
            return GetVM(new object[] { config }, descriptor);
        }
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

        public List<T> GetAllData<T>(IList<ConfigItemNode> nodes)
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

        public T GetData<T>(IList<ConfigItemNode> nodes)
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
                DescriptorInfo descInfo = GetOrCreateDescriptorInfo(type.Name, type, descriptor);
                var node = GetNode(configItem, descInfo);
                result.Add(node);
            }
            return result;
        }

        private DescriptorInfo GetOrCreateDescriptorInfo(string key, Type sourceType, DescriptorInfoDict descriptor)
        {
            DescriptorInfo descInfo;
            if (descriptor != null && descriptor.ContainsKey(key))
                descInfo = descriptor[key];
            else
            {
                //生成默认描述信息
                descInfo = new DescriptorInfo() { Text = key };

                if (sourceType == typeof(TimeSpan))
                    descInfo.Type = PropertyType.TimeSpan;

                if (IsList(sourceType))
                {
                    descInfo.Type = PropertyType.List;
                }
            }
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
                var valueDescriptor = GetOrCreateDescriptorInfo(pType.Name, pType.PropertyType, descriptor.PropertyDescriptors);
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

        private bool IsValue(Type type)
        {
            return type.IsPrimitive
                || type.Equals(typeof(string))
                || type.Equals(typeof(TimeSpan))
                || IsList(type);
        }

        private bool IsList(Type type)
        {
            bool isList = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
            return isList;
        }

        #endregion
    }
}
