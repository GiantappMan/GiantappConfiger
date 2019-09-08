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
        public ConfigerViewModel GetVM(DescriptorInfoDict descriptor, params object[] config)
        {
            var nodes = GetNodes(descriptor, config);
            var vm = new ConfigerViewModel
            {
                Nodes = new ObservableCollection<ConfigItemNode>(nodes)
            };
            vm.Nodes[0].Selected = true;
            return vm;
        }

        public T GetData<T>(IList<ConfigItemNode> nodes)
        {
            return default;
            //var result = new ExpandoObject() as IDictionary<string, object>;
            //foreach (var nodeItem in nodes)
            //{
            //    var tempNodeObj = GetDataFromNode(nodeItem);

            //    foreach (var subNode in nodeItem.SubNodes)
            //    {
            //        var subNodeObj = GetDataFromNode(subNode);
            //        tempNodeObj.Add(subNode.Descriptor.Name, subNodeObj);
            //    }
            //    result.Add(nodeItem.Descriptor.Name, tempNodeObj);
            //}
            //return result;
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
            return null;
            //var tempNodeObj = new ExpandoObject() as IDictionary<string, object>;
            //foreach (var propertyItem in nodeItem.Properties)
            //{
            //    tempNodeObj.Add(propertyItem.Descriptor.Name, propertyItem.Value);
            //}
            //return tempNodeObj;
        }

        private List<ConfigItemNode> GetNodes(DescriptorInfoDict descriptor, params object[] configs)
        {
            List<ConfigItemNode> result = new List<ConfigItemNode>();
            foreach (var configItem in configs)
            {
                if (configItem == null || IsValue(configItem.GetType()))
                    return null;

                var type = configItem.GetType();
                string key = type.Name;
                ConfigItemNode tmp = GetNode(configItem, descriptor[key]);
                result.Add(tmp);
            }
            return result;
        }

        private ConfigItemNode GetNode(object data, DescriptorInfo descriptorInfo)
        {
            var node = new ConfigItemNode();
            if (descriptorInfo == null)
                descriptorInfo = new DescriptorInfo() { Text = data.GetType().Name };

            var propertyTypes = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            node.Properties = GetProperties(data, propertyTypes, descriptorInfo.PropertyDescriptors);
            var subNodes = GetSubNodes(data, propertyTypes, descriptorInfo.PropertyDescriptors);
            node.SubNodes = new ObservableCollection<ConfigItemNode>(subNodes);
            //node.SubNodes=GetNodes(descriptorInfo.PropertyDescriptors,)

            //拷贝，不破坏元对象。只需要保留当前级的信息，节约内存
            descriptorInfo = DeepClone(descriptorInfo);
            descriptorInfo.PropertyDescriptors = null;
            node.Descriptor = descriptorInfo;

            return node;
        }

        private IList<ConfigItemNode> GetSubNodes(object data, PropertyInfo[] propertyTypes, Dictionary<string, DescriptorInfo> propertyDescriptors)
        {
            var result = new List<ConfigItemNode>();
            foreach (var pType in propertyTypes)
            {
                ConfigItemNode node = new ConfigItemNode();

                DescriptorInfo subDescriptor = null;
                if (propertyDescriptors != null && propertyDescriptors.ContainsKey(pType.Name))
                    subDescriptor = propertyDescriptors[pType.Name];
                else
                    subDescriptor = new DescriptorInfo() { Text = pType.Name };
                var property = new ConfigItemProperty
                {
                    Descriptor = subDescriptor
                };

                var isValue = IsValue(pType.PropertyType);
                if (isValue)
                    continue;

                if (property.Value == null)
                    //子对象
                    property.Value = Activator.CreateInstance(pType.PropertyType);

                propertyTypes = property.Value.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                var subNodes = GetSubNodes(property.Value, propertyTypes, subDescriptor.PropertyDescriptors);
                if (subNodes != null)
                    node.SubNodes = new ObservableCollection<ConfigItemNode>(subNodes);
                node.Properties = GetProperties(property.Value, propertyTypes, subDescriptor.PropertyDescriptors);
                result.Add(node);
            }
            return result;
        }

        private ObservableCollection<ConfigItemProperty> GetProperties(object data, PropertyInfo[] instanceProperties, Dictionary<string, DescriptorInfo> descriptors)
        {
            var tmpProperties = new ObservableCollection<ConfigItemProperty>();
            foreach (var pType in instanceProperties)
            {
                DescriptorInfo subDescriptor = null;
                if (descriptors != null && descriptors.ContainsKey(pType.Name))
                {
                    subDescriptor = descriptors[pType.Name];
                }
                else
                    subDescriptor = new DescriptorInfo() { Text = pType.Name };
                var property = new ConfigItemProperty
                {
                    Descriptor = subDescriptor
                };

                var isValue = IsValue(pType.PropertyType);
                if (isValue)
                {
                    var currentValue = pType.GetValue(data);
                    property.Value = currentValue;
                    if (property != null)
                        tmpProperties.Add(property);
                }
                else
                {
                    //if (property.Value == null)
                    //{
                    //    //子对象
                    //    property.Value = Activator.CreateInstance(pType.PropertyType);
                    //    if (node.SubNodes == null)
                    //        node.SubNodes = new ObservableCollection<ConfigItemNode>();

                    //    var subNodes = GetNode(property.Value, subDescriptor);
                    //    if (subNodes != null)
                    //        node.SubNodes = new ObservableCollection<ConfigItemNode>(subNodes);
                    //}
                }
            }
            return tmpProperties;
        }

        private bool IsValue(Type type)
        {
            return type.IsPrimitive || type.Equals(typeof(string));
        }

        #endregion
    }
}
