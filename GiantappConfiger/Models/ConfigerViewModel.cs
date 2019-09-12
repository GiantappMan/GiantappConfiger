using GiantappMvvm.Base;
using System.Collections.ObjectModel;

namespace GiantappConfiger.Models
{
    public class ConfigerViewModel : ObservableObj
    {
        #region properties

        #region Nodes

        /// <summary>
        /// The <see cref="Nodes" /> property's name.
        /// </summary>
        public const string NodesPropertyName = "Nodes";

        private ObservableCollection<ConfigItemNode> _Nodes;

        /// <summary>
        /// Nodes
        /// </summary>
        public ObservableCollection<ConfigItemNode> Nodes
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
