using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace JsonConfiger.Models
{
    public class CNode : CBaseObj
    {
        #region properties

        #region Children

        /// <summary>
        /// The <see cref="Children" /> property's name.
        /// </summary>
        public const string ChildrenPropertyName = "Children";

        private ObservableCollection<CNode> _Children;

        /// <summary>
        /// Children
        /// </summary>
        public ObservableCollection<CNode> Children
        {
            get { return _Children; }

            set
            {
                if (_Children == value) return;

                _Children = value;
                NotifyOfPropertyChange(ChildrenPropertyName);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="Properties" /> property's name.
        /// </summary>
        public const string PropertiesPropertyName = "Properties";

        private ObservableCollection<CProperty> _Properties;

        /// <summary>
        /// Properties
        /// </summary>
        public ObservableCollection<CProperty> Properties
        {
            get { return _Properties; }

            set
            {
                if (_Properties == value) return;

                _Properties = value;
                NotifyOfPropertyChange(PropertiesPropertyName);
            }
        }

        #endregion

        #region Selected

        /// <summary>
        /// The <see cref="Selected" /> property's name.
        /// </summary>
        public const string SelectedPropertyName = "Selected";

        private bool _Selected;

        /// <summary>
        /// Selected
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }

            set
            {
                if (_Selected == value) return;

                _Selected = value;
                NotifyOfPropertyChange(SelectedPropertyName);
            }
        }

        #endregion

        #endregion
    }
}
