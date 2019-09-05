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

        ///// <summary>
        ///// 和默认配置对比，如果缺失则补上
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="defaultData"></param>
        ///// <returns></returns>
        //public static object CheckDefault(object data, Descriptor defaultData)
        //{
        //    if (data == null || defaultData == null)
        //        return data;
        //    var result = DeepClone(data);
        //    var properties = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
        //    if (properties != null)
        //        foreach (var pItem in properties)
        //        {
        //            var currentPropertyValue = pItem.GetValue(result);
        //            if (currentPropertyValue == null && defaultData.SumbItems.ContainsKey(pItem.Name))
        //            {
        //                var descriptor = defaultData.SumbItems[pItem.Name];
        //                if (descriptor != null)
        //                {
        //                    pItem.SetValue(result, descriptor.DefaultValue);
        //                    currentPropertyValue = descriptor.DefaultValue;
        //                }

        //                if (descriptor.SumbItems != null)
        //                {
        //                    currentPropertyValue = CheckDefault(currentPropertyValue, descriptor);
        //                    pItem.SetValue(result, currentPropertyValue);
        //                }
        //            }
        //        }
        //    return result;
        //}

        public ConfigerViewModel GetVM(object config, Descriptor descConfig)
        {
            //config = CheckDefault(config, descConfig);
            var vm = new ConfigerViewModel
            {
                Nodes = GetNodes(config, descConfig)
            };
            vm.Nodes[0].Selected = true;
            return vm;
        }

        public UserControl GetView(object config, Descriptor desc)
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

        private ObservableCollection<ConfigItemNode> GetNodes(object data, Descriptor descriptor)
        {
            if (data == null || IsValue(data.GetType()))
                return null;

            var result = new ObservableCollection<ConfigItemNode>();
            var node = new ConfigItemNode();
            if (descriptor == null)
                descriptor = new Descriptor() { Text = data.GetType().Name };

            //拷贝，不破坏元对象。只需要保留当前级的信息，节约内存
            descriptor = DeepClone(descriptor);
            descriptor.SumbItems = null;

            node.Descriptor = descriptor;
            result.Add(node);

            var subProperties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

            var tmpProperties = new ObservableCollection<ConfigItemProperty>();
            foreach (var pType in subProperties)
            {
                Descriptor subDescriptor = null;
                if (descriptor.SumbItems != null && descriptor.SumbItems.ContainsKey(pType.Name))
                {
                    subDescriptor = descriptor.SumbItems[pType.Name];
                }
                else
                    subDescriptor = new Descriptor() { Text = pType.Name };
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
            node.Properties = tmpProperties;

            return result;
        }

        private bool IsValue(Type type)
        {
            return type.IsPrimitive || type.Equals(typeof(string));
        }

        #endregion
    }
}
