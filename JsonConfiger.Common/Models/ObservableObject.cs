using System.ComponentModel;

namespace JsonConfiger.Models
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyOfPropertyChange(string propertyName)
        {
            var handle = PropertyChanged;
            if (handle == null)
                return;
            handle(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
