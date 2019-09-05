using System.Collections.ObjectModel;

namespace GiantappConfiger.Models
{
    /// <summary>
    /// 表示一个数据节点，UI左侧
    /// </summary>
    public class ConfigItemNode : _ObservableObject
    {
        #region properties
        
        #region Children

        /// <summary>
        /// The <see cref="SubNodes" /> property's name.
        /// </summary>
        public const string ChildrenPropertyName = "Children";

        private ObservableCollection<ConfigItemNode> _Children;

        /// <summary>
        /// 子节点
        /// </summary>
        public ObservableCollection<ConfigItemNode> SubNodes
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

        private ObservableCollection<ConfigItemProperty> _Properties;

        /// <summary>
        /// 该节点包含的属性
        /// </summary>
        public ObservableCollection<ConfigItemProperty> Properties
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
        /// 节点是否选中
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

        #endregion
    }
}
