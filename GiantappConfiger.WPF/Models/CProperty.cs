using System;
using System.Collections.Generic;

namespace GiantappConfiger.Models
{
    public enum CPropertyType
    {
        Integer,
        Float,
        String,
        Boolean,
        TimeSpan,
        Combobox
    }

    public class CProperty : CBaseObj
    {
        #region CType

        /// <summary>
        /// The <see cref="CType" /> property's name.
        /// </summary>
        public const string CTypePropertyName = "CType";

        private CPropertyType _CType;

        /// <summary>
        /// CType
        /// </summary>
        public CPropertyType CType
        {
            get { return _CType; }

            set
            {
                if (_CType == value) return;

                _CType = value;
                NotifyOfPropertyChange(CTypePropertyName);
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

        #region ItemsSource

        /// <summary>
        /// The <see cref="ItemsSource" /> property's name.
        /// </summary>
        public const string ItemsSourcePropertyName = "ItemsSource";

        private List<CProperty> _ItemsSource;

        /// <summary>
        /// ItemsSource
        /// </summary>
        public List<CProperty> ItemsSource
        {
            get { return _ItemsSource; }

            set
            {
                if (_ItemsSource == value) return;

                _ItemsSource = value;
                NotifyOfPropertyChange(ItemsSourcePropertyName);
            }
        }

        #endregion

        #region Selected

        /// <summary>
        /// The <see cref="Selected" /> property's name.
        /// </summary>
        public const string SelectedPropertyName = "Selected";

        private CProperty _Selected;

        /// <summary>
        /// Selected
        /// </summary>
        public CProperty Selected
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
