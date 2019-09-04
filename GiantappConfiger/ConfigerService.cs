using GiantappConfiger.Models;
using System.Windows.Controls;
using GiantappConfiger;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;

namespace GiantappConfiger
{
    public class ConfigerService
    {
        private readonly Dictionary<string, List<dynamic>> _injectedDescOjbs = new Dictionary<string, List<dynamic>>();

        public void InjectDescObjs(string key, List<dynamic> descObjs)
        {
            _injectedDescOjbs[key] = descObjs;
        }

        /// <summary>
        /// 和默认配置对比，如果缺失则补上
        /// </summary>
        /// <param name="data"></param>
        /// <param name="defaultData"></param>
        /// <returns></returns>
        public static JToken CheckDefault(JObject data, JObject defaultData)
        {
            var result = JToken.FromObject(new object());
            if (data != null)
                result = data.DeepClone();
            foreach (var x in defaultData)
            {
                var tmpValue = result[x.Key];
                var defaultValue = defaultData[x.Key];
                if (tmpValue == null)
                {
                    result[x.Key] = defaultValue;
                }
                else if (!(tmpValue is JValue))
                {
                    result[x.Key] = CheckDefault(tmpValue as JObject, defaultValue as JObject);
                }
            }
            return result;
        }

        public ConfierViewModel GetVM(object config, Base descConfig)
        {
            var vm = new ConfierViewModel
            {
                Nodes = ResolveJson(config, descConfig).Nodes
            };
            vm.Nodes[0].Selected = true;
            return vm;
        }

        public UserControl GetView(object config, Base desc)
        {
            var control = new ConfigControl
            {
                DataContext = GetVM(config, desc)
            };
            return control;
        }

        public object GetData(ObservableCollection<NodeInfo> nodes)
        {
            var result = new ExpandoObject() as IDictionary<string, object>;
            foreach (var nodeItem in nodes)
            {
                var tempNodeObj = GetDataFromNode(nodeItem);

                foreach (var subNode in nodeItem.Children)
                {
                    var subNodeObj = GetDataFromNode(subNode);
                    tempNodeObj.Add(subNode.Name, subNodeObj);
                }
                result.Add(nodeItem.Name, tempNodeObj);
            }
            return result;
        }

        #region private

        private IDictionary<string, object> GetDataFromNode(NodeInfo nodeItem)
        {
            var tempNodeObj = new ExpandoObject() as IDictionary<string, object>;
            foreach (var propertyItem in nodeItem.Properties)
            {
                tempNodeObj.Add(propertyItem.Name, propertyItem.Value);
            }
            return tempNodeObj;
        }

        private void FillObj(Base property, Base descInfo)
        {
            property.Lan = descInfo.Lan;
            property.LanKey = descInfo.LanKey;
            property.Desc = descInfo.Desc;
            property.DescLanKey = descInfo.DescLanKey;
        }

        private PropertyInfo ConverterToNodeProperty(JValue value)
        {
            if (value == null)
                return null;

            PropertyInfo result = new PropertyInfo
            {
                Value = value.Value
            };
            bool ok = Enum.TryParse(value.Type.ToString(), out PropertyType Type);
            if (ok)
                result.Type = Type;
            return result;
        }

        private (ObservableCollection<NodeInfo> Nodes, ObservableCollection<PropertyInfo> Properties) ResolveJson(object data, Base descObj)
        {
            var childNodes = new ObservableCollection<NodeInfo>();
            var properties = new ObservableCollection<PropertyInfo>();
            if (data == null)
                return (childNodes, properties);

            dynamic descInfo = null;
            foreach (var x in data)
            {
                if (descObj != null)
                    descInfo = descObj[x.Key];

                descInfo = ConveterJnjectData(descInfo);

                if (x.Value is JValue)
                {
                    var value = x.Value as JValue;
                    PropertyInfo property = ConverterToNodeProperty(value);
                    if (property == null)
                        continue;

                    if (descInfo != null)
                    {
                        bool ok = Enum.TryParse(descInfo.type.ToString(), true, out PropertyType cType);
                        if (ok)
                            property.Type = cType;

                        FillObj(property, descInfo);

                        if (descInfo.cbItems != null)
                        {
                            var tempList = new List<PropertyInfo>();
                            foreach (var item in descInfo.cbItems)
                            {
                                var tmp = new PropertyInfo();
                                FillObj(tmp, item);
                                tmp.Value = item.value.ToString();
                                tempList.Add(tmp);
                            }
                            property.Options = tempList;
                            property.Selected = tempList.FirstOrDefault
                                (m => m.Value != null && property.Value != null
                                && m.Value.ToString() == property.Value.ToString());
                        }
                    }

                    property.Name = x.Key;

                    if (property != null)
                        properties.Add(property);
                }
                else
                {
                    var node = new NodeInfo();
                    if (descInfo != null)
                    {
                        FillObj(node, descInfo);
                    }
                    node.Name = x.Key;

                    var (Nodes, Properties) = ResolveJson(x.Value as JObject, descInfo as JObject);
                    node.Children = Nodes;
                    node.Properties = Properties;
                    childNodes.Add(node);
                }
            }

            return (childNodes, properties);
        }

        private dynamic ConveterJnjectData(JToken descInfo)
        {
            if (descInfo == null) return null;

            foreach (JProperty item in descInfo)
            {
                string tmpValue = item?.Value.ToString();
                if (tmpValue.StartsWith("$"))
                {
                    item.Value = JToken.FromObject(_injectedDescOjbs[tmpValue]);
                }
            }

            return descInfo;
        }

        #endregion

        #region Obsolete

        [Obsolete]
        public ConfierViewModel GetVM(object config, object descConfig)
        {
            if (!(config is JObject json))
                return null;

            var vm = new ConfierViewModel
            {
                Nodes = ResolveJson(config as JObject, descConfig as JObject).Nodes
            };
            vm.Nodes[0].Selected = true;
            return vm;
        }

        [Obsolete]
        public UserControl GetView(object config, object desc)
        {
            var control = new ConfigControl
            {
                DataContext = GetVM(config, desc)
            };
            return control;
        }
        #endregion
    }
}
