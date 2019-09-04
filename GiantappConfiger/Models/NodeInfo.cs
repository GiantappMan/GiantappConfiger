using System.Collections.ObjectModel;

namespace GiantappConfiger.Models
{
    /// <summary>
    /// 表示一个数据节点，UI左侧
    /// </summary>
    public class NodeInfo : Base
    {
        #region properties
        
        #region Children

        /// <summary>
        /// The <see cref="Children" /> property's name.
        /// </summary>
        public const string ChildrenPropertyName = "Children";

        private ObservableCollection<NodeInfo> _Children;

        /// <summary>
        /// 子节点
        /// </summary>
        public ObservableCollection<NodeInfo> Children
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

        private ObservableCollection<PropertyInfo> _Properties;

        /// <summary>
        /// 该节点包含的属性
        /// </summary>
        public ObservableCollection<PropertyInfo> Properties
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

        #endregion
    }
}
