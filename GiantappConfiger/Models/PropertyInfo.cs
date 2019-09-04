using System;
using System.Collections.Generic;

namespace GiantappConfiger.Models
{
    public enum PropertyType
    {
        Integer,
        Float,
        String,
        Boolean,
        TimeSpan,
        Combobox
    }

    /// <summary>
    /// 表示一个可填的字段，界面右边
    /// </summary>
    public class PropertyInfo : Base
    {
        #region Type

        /// <summary>
        /// The <see cref="Type" /> property's name.
        /// </summary>
        public const string TypePropertyName = "Type";

        private PropertyType _Type;

        /// <summary>
        /// CType
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

        #region Value

        /// <summary>
        /// The <see cref="Value" /> property's name.
        /// </summary>
        public const string ValuePropertyName = "Value";

        private Object _Value;

        /// <summary>
        /// Value
        /// </summary>
        public Object Value
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

        #region Options

        /// <summary>
        /// The <see cref="Options" /> property's name.
        /// </summary>
        public const string OptionsPropertyName = "Options";

        private List<PropertyInfo> _Options;

        /// <summary>
        /// 表示当前字段的可选择项
        /// </summary>
        public List<PropertyInfo> Options
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

        #region Selected

        /// <summary>
        /// The <see cref="Selected" /> property's name.
        /// </summary>
        public const string SelectedPropertyName = "Selected";

        private PropertyInfo _Selected;

        /// <summary>
        /// Selected
        /// </summary>
        public PropertyInfo Selected
        {
            get { return _Selected; }

            set
            {
                if (_Selected == value) return;

                _Selected = value;
                if (value == null)
                    Value = null;
                else
                    Value = _Selected.Value;
                NotifyOfPropertyChange(SelectedPropertyName);
            }
        }

        #endregion
    }
}
