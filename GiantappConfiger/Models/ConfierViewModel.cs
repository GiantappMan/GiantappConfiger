using System.Collections.ObjectModel;

namespace GiantappConfiger.Models
{
    public class ConfierViewModel : _ObservableObject
    {
        #region properties

        #region Nodes

        /// <summary>
        /// The <see cref="Nodes" /> property's name.
        /// </summary>
        public const string NodesPropertyName = "Nodes";

        private ObservableCollection<NodeInfo> _Nodes;

        /// <summary>
        /// Nodes
        /// </summary>
        public ObservableCollection<NodeInfo> Nodes
        {
            get { return _Nodes; }

            set
            {
                if (_Nodes == value) return;

                _Nodes = value;
                NotifyOfPropertyChange(NodesPropertyName);
            }
        }

        #endregion

        #endregion
    }
}
