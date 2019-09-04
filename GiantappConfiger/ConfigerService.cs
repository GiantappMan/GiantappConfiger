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
        private readonly Dictionary<string, List<dynamic>> _injectedDescOjbs = new Dictionary<string, List<dynamic>>();

        public void InjectDescObjs(string key, List<dynamic> descObjs)
        {
            _injectedDescOjbs[key] = descObjs;
        }
        public static T DeepClone<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var type = obj.GetType();
            return (T)JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// 和默认配置对比，如果缺失则补上
        /// </summary>
        /// <param name="data"></param>
        /// <param name="defaultData"></param>
        /// <returns></returns>
        public static object CheckDefault(object data, Descriptor defaultData)
        {
            if (data == null)
                return null;
            var result = DeepClone(data);
            var properties = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            if (properties != null)
                foreach (var pItem in properties)
                {
                    var currentPropertyValue = pItem.GetValue(result);
                    if (currentPropertyValue == null && defaultData.SumbItems.ContainsKey(pItem.Name))
                    {
                        var descriptor = defaultData.SumbItems[pItem.Name];
                        if (descriptor != null)
                        {
                            pItem.SetValue(result, descriptor.DefaultValue);
                            currentPropertyValue = descriptor.DefaultValue;
                        }

                        if (descriptor.SumbItems != null)
                        {
                            currentPropertyValue = CheckDefault(currentPropertyValue, descriptor);
                            pItem.SetValue(result, currentPropertyValue);
                        }
                    }
                }
            return result;
        }

        public ConfigerViewModel GetVM(object config, Descriptor descConfig)
        {
            config = CheckDefault(config, descConfig);
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
            var configNodes = new ObservableCollection<ConfigItemNode>();
            if (data == null)
                return null;

            bool isValue = IsValue(data.GetType());
            if (!isValue)
            {
                var tmpProperties = new ObservableCollection<ConfigItemProperty>();
                var node = new ConfigItemNode();
                if (descriptor != null)
                {
                    var tmp = DeepClone(descriptor);
                    //只需要保留当前级的信息，节约内存
                    tmp.SumbItems = null;
                    node.Descriptor = tmp;
                }
                else
                    descriptor = new Descriptor() { Text = data.GetType().Name };

                configNodes.Add(node);

                var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                foreach (var pType in properties)
                {
                    Descriptor subDescriptor = null;
                    if (descriptor.SumbItems.ContainsKey(pType.Name))
                    {
                        subDescriptor = descriptor.SumbItems[pType.Name];
                    }
                    else
                        subDescriptor = new Descriptor() { Text = pType.Name };
                    var property = new ConfigItemProperty
                    {
                        Descriptor = subDescriptor
                    };

                    isValue = IsValue(pType.PropertyType);
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
            }

            return configNodes;

            //Descriptor descriptor = null;
            //properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            //foreach (var x in properties)
            //{
            //    if (descObj != null && descObj.SumbItems.ContainsKey(x.Name))
            //        descriptor = descObj.SumbItems[x.Name];

            //    var currentValue = x.GetValue(data);
            //    //isValue = IsValue(x);
            //    if (currentValue != null && isValue)
            //    {
            //        var property = new ConfigItemProperty();
            //        property.Descriptor = descriptor;
            //        property.Value = currentValue;
            //        if (property != null)
            //            configProperties.Add(property);
            //    }
            //    else
            //    {
            //        var node = new ConfigItemNode();
            //        if (descriptor != null)
            //        {
            //            var tmp = DeepClone(descriptor);
            //            //只需要保留当前级的信息，节约内存
            //            tmp.SumbItems = null;
            //            node.Descriptor = tmp;
            //        }

            //        var Nodes = Resolve(currentValue, descriptor);
            //        node.Children = Nodes;
            //        configNodes.Add(node);
            //    }
            //}

            //return configNodes;
        }

        private bool IsValue(Type type)
        {
            //var properties = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            //return properties.Length > 0;
            return type.IsPrimitive || type.Equals(typeof(string));
        }

        #endregion
    }
}
