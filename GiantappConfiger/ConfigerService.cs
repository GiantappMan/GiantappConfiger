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
        public static T DeepClone<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var type = obj.GetType();
            return (T)JsonConvert.DeserializeObject(json, type);
        }

        public ConfigerViewModel GetVM(object config, DescriptorInfo descConfig = null)
        {
            var vm = new ConfigerViewModel
            {
                Nodes = GetNodes(config, descConfig)
            };
            vm.Nodes[0].Selected = true;
            return vm;
        }

        public UserControl GetView(object config, DescriptorInfo desc)
        {
            var control = new ConfigControl
            {
                DataContext = GetVM(config, desc)
            };
            return control;
        }

        public object GetData(ObservableCollection<ConfigItemNode> nodes)
        {
            var result = new ExpandoObject() as IDictionary<string, object>;
            foreach (var nodeItem in nodes)
            {
                var tempNodeObj = GetDataFromNode(nodeItem);

                foreach (var subNode in nodeItem.SubNodes)
                {
                    var subNodeObj = GetDataFromNode(subNode);
                    tempNodeObj.Add(subNode.Descriptor.Name, subNodeObj);
                }
                result.Add(nodeItem.Descriptor.Name, tempNodeObj);
            }
            return result;
        }

        #region private

        private IDictionary<string, object> GetDataFromNode(ConfigItemNode nodeItem)
        {
            var tempNodeObj = new ExpandoObject() as IDictionary<string, object>;
            foreach (var propertyItem in nodeItem.Properties)
            {
                tempNodeObj.Add(propertyItem.Descriptor.Name, propertyItem.Value);
            }
            return tempNodeObj;
        }

        private ObservableCollection<ConfigItemNode> GetNodes(object data, DescriptorInfo descriptor)
        {
            if (data == null || IsValue(data.GetType()))
                return null;

            var result = new ObservableCollection<ConfigItemNode>();
            var node = new ConfigItemNode();
            if (descriptor == null)
                descriptor = new DescriptorInfo() { Text = data.GetType().Name };

            //拷贝，不破坏元对象。只需要保留当前级的信息，节约内存
            descriptor = DeepClone(descriptor);
            descriptor.SumbDescriptorInfo = null;

            node.Descriptor = descriptor;
            node.Properties = GetProperties(data, descriptor.SumbDescriptorInfo);

            result.Add(node);
            return result;
        }

        private ObservableCollection<ConfigItemProperty> GetProperties(object data, Dictionary<string, DescriptorInfo> descriptor)
        {
            var subProperties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            var tmpProperties = new ObservableCollection<ConfigItemProperty>();
            foreach (var pType in subProperties)
            {
                DescriptorInfo subDescriptor = null;
                if (descriptor != null && descriptor.ContainsKey(pType.Name))
                {
                    subDescriptor = descriptor[pType.Name];
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
                    if (property.Value == null)
                    {
                        //子对象
                        property.Value = Activator.CreateInstance(pType.PropertyType);
                        if (node.SubNodes == null)
                            node.SubNodes = new ObservableCollection<ConfigItemNode>();

                        var subNodes = GetNodes(property.Value, subDescriptor);
                        if (subNodes != null)
                            node.SubNodes = new ObservableCollection<ConfigItemNode>(subNodes);
                    }
                }
            }
            return tmpProperties
        }

        private bool IsValue(Type type)
        {
            return type.IsPrimitive || type.Equals(typeof(string));
        }

        #endregion
    }
}
