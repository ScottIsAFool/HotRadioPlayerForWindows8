using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HotRadioPlayer
{
    public class Group<T> : ModelBase where T : class
    {
        public Group ()
        {
            Items = new ObservableCollection<T>();
        }
        public string Title { get; set; }
        public ObservableCollection<T> Items { get; set; }
    }

    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
