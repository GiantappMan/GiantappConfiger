using GiantappMvvm.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GiantappConfiger.Models
{
    public enum PropertyType
    {
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
                list.Add(Activator.CreateInstance(itemType));
            }
        }

        #endregion
    }
}
