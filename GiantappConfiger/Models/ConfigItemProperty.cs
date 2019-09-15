using GiantappMvvm.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GiantappConfiger.Models
{
    public enum PropertyType
    {
        None,
        Integer,
        Float,
        String,
        Boolean,
        TimeSpan,
        Combobox,
        List
    }

    /// <summary>
    /// 表示一个可填的字段，界面右边
    /// </summary>
    public class ConfigItemProperty : ObservableObj
    {
        #region Value

        /// <summary>
        /// The <see cref="Value" /> property's name.
        /// </summary>
        public const string ValuePropertyName = "Value";

        private object _Value;

        /// <summary>
        /// Value
        /// </summary>
        public object Value
        {
            get { return _Value; }

            set
            {
                if (_Value == value) return;

                if (_Value + "" == value + "")
                    return;

                _Value = value;
                NotifyOfPropertyChange(ValuePropertyName);
            }
        }

        #endregion

        #region Descriptor

        /// <summary>
        /// The <see cref="Descriptor" /> property's name.
        /// </summary>
        public const string DescriptorPropertyName = "Descriptor";

        private DescriptorInfo _Descriptor;

        /// <summary>
        /// 描述信息
        /// </summary>
        public DescriptorInfo Descriptor
        {
            get { return _Descriptor; }

            set
            {
                if (_Descriptor == value) return;

                _Descriptor = value;
                NotifyOfPropertyChange(DescriptorPropertyName);
            }
        }

        #endregion

        #region AddItemCommand

        private DelegateCommand _AddItemCommand;

        /// <summary>
        /// Gets the AddItemCommand.
        /// </summary>
        public DelegateCommand AddItemCommand
        {
            get
            {
                return _AddItemCommand ?? (_AddItemCommand = new DelegateCommand(ExecuteAddItemCommand));
            }
        }

        private void ExecuteAddItemCommand()
        {
            if (Value == null)
                return;

            var type = Value.GetType();
            bool isList = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableCollection<>);
            if (isList)
            {
                var itemType = Descriptor.SourceType.GetGenericArguments()[0];
                var list = Value as ObservableCollection<object>;
                var data = Activator.CreateInstance(itemType);
                //把对象转化为property对象，xaml自动绑定对应模板
                var vm = ConfigerService.GetNodes(new object[] { data }, null);
                list.Add(vm[0].Properties);
            }
        }

        #endregion

        #region DeleteItemCommand 

        private DelegateCommand<ObservableCollection<ConfigItemProperty>> _DeleteItemCommand;

        /// <summary>
        /// Gets the DeleteItemCommand.
        /// </summary>
        public DelegateCommand<ObservableCollection<ConfigItemProperty>> DeleteItemCommand
        {
            get
            {
                return _DeleteItemCommand ?? (_DeleteItemCommand = new DelegateCommand<ObservableCollection<ConfigItemProperty>>(ExecuteDeleteItemCommand));
            }
        }

        private void ExecuteDeleteItemCommand(ObservableCollection<ConfigItemProperty> parameter)
        {
            if (Value == null)
                return;

            var type = Value.GetType();
            bool isList = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableCollection<>);
            if (isList)
            {
                var list = Value as ObservableCollection<object>;
                list.Remove(parameter);
            }
        }

        #endregion
    }
}
