using System.Collections.ObjectModel;

namespace JsonConfiger.Models
{
    public class JsonConfierViewModel : ObservableObject
    {
        #region properties

        #region Nodes

        /// <summary>
        /// The <see cref="Nodes" /> property's name.
        /// </summary>
        public const string NodesPropertyName = "Nodes";

        private ObservableCollection<CNode> _Nodes;

        /// <summary>
        /// Nodes
        /// </summary>
        public ObservableCollection<CNode> Nodes
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
