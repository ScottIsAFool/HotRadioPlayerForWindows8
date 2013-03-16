using System;
using GalaSoft.MvvmLight.Messaging;
using Windows.Media;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace HotRadioPlayer.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : HotRadioPlayer.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            MediaControl.PlayPressed += MediaControlOnPlayPressed;
            MediaControl.PausePressed += MediaControlOnPausePressed;
            MediaControl.PlayPauseTogglePressed += MediaControlOnPlayPauseTogglePressed;
            MediaControl.StopPressed += MediaControlOnStopPressed;

            Loaded += (sender, args) => Messenger.Default.Send(new NotificationMessage("PageLoadedMsg"));
        }

        private async void MediaControlOnStopPressed(object sender, object o)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, PlayPause);
        }

        private async void MediaControlOnPlayPauseTogglePressed(object sender, object o)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, PlayPause);
        }

        private void PlayPause()
        {
            try
            {
                if (MediaPlayer.CurrentState == MediaElementState.Stopped || MediaPlayer.CurrentState == MediaElementState.Paused)
                {
                    MediaPlayer.Play();
                    backButton.Style = (Style)Application.Current.Resources["StopAppBarButtonStyle"];
                }
                else
                {
                    MediaPlayer.Pause();
                    backButton.Style = (Style)Application.Current.Resources["PlayAppBarButtonStyle"];
                }
            }
            catch{}
        }

        private async void MediaControlOnPausePressed(object sender, object o)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, PlayPause);
        }

        private async void MediaControlOnPlayPressed(object sender, object o)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, PlayPause);
        }

        private async void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            MediaControl.TrackName = "Hot Radio Player";

            PlayPause();
        }
    }
}
