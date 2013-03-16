using System.ComponentModel;
using WinRtUtility;

namespace HotRadioPlayer.Model
{
    public class Settings : INotifyPropertyChanged
    {
        public bool AutoRefreshDetails { get; set; }
        private async void OnAutoRefreshDetailsChanged()
        {
            await new ObjectStorageHelper<bool>(StorageType.Roaming).SaveAsync(AutoRefreshDetails, "AutoRefreshDetails");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
