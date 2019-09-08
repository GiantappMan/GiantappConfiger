using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GiantappConfiger.Models
{
    public class DescriptorInfoDict : Dictionary<string, DescriptorInfo>
    {
    }
    /// <summary>
    /// 用于描述配置字段
    /// </summary>
    public class DescriptorInfo : _ObservableObject
    {
        #region Text

        /// <summary>
        /// The <see cref="Text" /> property's name.
        /// </summary>
        public const string TextPropertyName = "Text";

        private string _Text;

        /// <summary>
        /// 字段文本
        /// </summary>
        public string Text
        {
            get { return _Text; }

            set
            {
                if (_Text == value) return;

                _Text = value;
                NotifyOfPropertyChange(TextPropertyName);
            }
        }

        #endregion

        #region TextKey

        /// <summary>
        /// The <see cref="TextKey" /> property's name.
        /// </summary>
        public const string TextKeyPropertyName = "TextKey";

        private string _TextKey;

        /// <summary>
        /// 字段文本  多语言key
        /// </summary>
        public string TextKey
        {
            get { return _TextKey; }

            set
            {
                if (_TextKey == value) return;

                _TextKey = value;
                NotifyOfPropertyChange(TextKeyPropertyName);
            }
        }

        #endregion

        #region Desc

        /// <summary>
        /// The <see cref="Desc" /> property's name.
        /// </summary>
        public const string DescPropertyName = "Desc";

        private string _Desc;

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Desc
        {
            get { return _Desc; }

            set
            {
                if (_Desc == value) return;

                _Desc = value;
                NotifyOfPropertyChange(DescPropertyName);
            }
        }

        #endregion

        #region DescKey

        /// <summary>
        /// The <see cref="DescKey" /> property's name.
        /// </summary>
        public const string DescKeyPropertyName = "DescKey";

        private string _DescKey;

        /// <summary>
        /// 字段描述 多语言key
        /// </summary>
        public string DescKey
        {
            get { return _DescKey; }

            set
            {
                if (_DescKey == value) return;

                _DescKey = value;
                NotifyOfPropertyChange(DescKeyPropertyName);
            }
        }

        #endregion

        #region Type

        /// <summary>
        /// The <see cref="Type" /> property's name.
        /// </summary>
        public const string TypePropertyName = "Type";

        private PropertyType _Type;

        /// <summary>
        /// 字段类型
        /// </summary>
        public PropertyType Type
        {
            get { return _Type; }

            set
            {
                if (_Type == value) return;

                _Type = value;
                NotifyOfPropertyChange(TypePropertyName);
            }
        }

        #endregion

        #region DefaultValue

        /// <summary>
        /// The <see cref="DefaultValue" /> property's name.
        /// </summary>
        public const string DefaultValuePropertyName = "DefaultValue";

        private object _DefaultValue;

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue
        {
            get { return _DefaultValue; }

            set
            {
                if (_DefaultValue == value) return;

                _DefaultValue = value;
                NotifyOfPropertyChange(DefaultValuePropertyName);
            }
        }

        #endregion

        #region Options

        /// <summary>
        /// The <see cref="Options" /> property's name.
        /// </summary>
        public const string OptionsPropertyName = "Options";

        private ObservableCollection<DescriptorInfo> _Options;

        /// <summary>
        /// 
        /// 
        /// </summary>
        public ObservableCollection<DescriptorInfo> Options
        {
            get { return _Options; }

            set
            {
                if (_Options == value) return;

                _Options = value;
                NotifyOfPropertyChange(OptionsPropertyName);
            }
        }

        #endregion

        #region PropertyDescriptors

        /// <summary>
        /// The <see cref="PropertyDescriptors" /> property's name.
        /// </summary>
        public const string PropertyDescriptorsPropertyName = "PropertyDescriptors";

        private Dictionary<string, DescriptorInfo> _PropertyDescriptors;

        /// <summary>
        /// 子集描述对象
        /// </summary>
        public Dictionary<string, DescriptorInfo> PropertyDescriptors
        {
            get { return _PropertyDescriptors; }

            set
            {
                if (_PropertyDescriptors == value) return;

                _PropertyDescriptors = value;
                NotifyOfPropertyChange(PropertyDescriptorsPropertyName);
            }
        }

        #endregion
    }
}
