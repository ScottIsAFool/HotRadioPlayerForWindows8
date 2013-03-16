using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HotRadioPlayer.Model;
using Newtonsoft.Json;
using Windows.Media;
using Windows.System;
using Windows.UI.Xaml;

namespace HotRadioPlayer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly HttpClient _httpClient;
        private readonly DispatcherTimer _refreshTimer;

        private const string HotPlayerUrl = "http://www.hot1028.com/now.crossdomain.json?callback=nowJson&_={0}";

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigation)
        {
            if (IsInDesignMode)
            {
                var items = new List<Track>
                                {
                                    new Track {Artist = "Aerosmith", Time = "20:56", Image = "http://a1.mzstatic.com/us/r2000/001/Music/2d/14/3a/mzi.gmwaxycw.100x100-75.jpg", Title="Dude, Looks Like A Lady"},
                                    new Track {Artist = "Bon Jovi", Time = "20:52", Image = "http://a4.mzstatic.com/us/r1000/012/Music/b4/3c/12/mzi.ntemtkqd.100x100-75.jpg", Title="Wanted (Dead Or Alive)"}
                                };
                RecentlyPlayed = new ObservableCollection<TrackWrapper>(AddColSpans(items));
                OnAirNow = new OnAirNow { Name = "Vicky Bright", ImageOnAir = "http://mm.gmstatic.net/83/779353.png", Times = "7:00PM - 9:00PM", Caption = "Hot Radio Evenings", Email = "glen.mitchell@hot1028.com" };
                OnAirNext = new OnAirNext { Name = "Vicky Bright", ImageOnAir = "http://mm.gmstatic.net/83/779353.png", Times = "7:00PM - 9:00PM", Caption = "Hot Radio Evenings" };
            }
            else
            {
                _navigationService = navigation;
                _httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip });
                ProgressVisibility = Visibility.Collapsed;

                _refreshTimer = new DispatcherTimer
                                    {
                                        Interval = new TimeSpan(0, 2, 0),
                                    };
                _refreshTimer.Tick += RefreshTimerOnTick;
                _refreshTimer.Start();
                WireMessages();
            }
            
        }

        private async void RefreshTimerOnTick(object sender, object o)
        {
            var settings = (Settings) Application.Current.Resources["Settings"];
            if (settings.AutoRefreshDetails)
            {
                await GetNowPlayingInfo();
            }
        }

        private void WireMessages()
        {
            Messenger.Default.Register<NotificationMessage>(this, async m =>
                                                                      {
                                                                          if (m.Notification.Equals("PageLoadedMsg"))
                                                                          {
                                                                              await GetNowPlayingInfo();
                                                                          }
                                                                      });
        }

        public string ProgressText { get; set; }
        public Visibility ProgressVisibility { get; set; }

        public ObservableCollection<TrackWrapper> RecentlyPlayed { get; set; }
        public OnAirNow OnAirNow { get; set; }
        public OnAirNext OnAirNext { get; set; }

        public RelayCommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                                                  {
                                                      await GetNowPlayingInfo();
                                                  });
            }
        }

        public RelayCommand<string> LaunchEmailCommand
        {
            get
            {
                return new RelayCommand<string>(email => Launcher.LaunchUriAsync(new Uri(string.Format("mailto:{0}", email))));
            }
        } 

        private async Task GetNowPlayingInfo()
        {
            if (_navigationService.IsNetworkAvailable)
            {
                ProgressText = "Getting now playing details...";
                ProgressVisibility = Visibility.Visible;
                var hotUrl = string.Format(HotPlayerUrl, GetTime());

                try
                {
                    var json = await _httpClient.GetStringAsync(hotUrl);
                    if(json.Contains("nowJson("))
                        json = json.Replace("nowJson(", "").Replace(");", "");

                    var playerDetails = JsonConvert.DeserializeObject<HotPlayer>(json);

                    foreach (var item in playerDetails.Last10Tracks)
                    {
                        item.Artist = Uri.UnescapeDataString(item.Artist);
                        item.Title = Uri.UnescapeDataString(item.Title);
                    }

                    OnAirNow = playerDetails.OnAirNow;
                    OnAirNext = playerDetails.OnAirNext;
                    RecentlyPlayed = new ObservableCollection<TrackWrapper>(AddColSpans(playerDetails.Last10Tracks));
                }
                catch
                {
                    
                }

                ProgressText = string.Empty;
                ProgressVisibility = Visibility.Collapsed;
            }
        }

        private IEnumerable<TrackWrapper> AddColSpans(IEnumerable<Track> list)
        {
            var result = new List<TrackWrapper>();
            var i = 0;
            foreach (var item in list)
            {
                var nItem = new TrackWrapper(item)
                                {
                                    ColSpan = i == 0 ? 2 : 1,
                                    RowSpan = i == 0 ? 2 : 1,
                                    NowPlayingVisibility = i == 0 ? Visibility.Visible : Visibility.Collapsed
                                };
                result.Add(nItem);
                i++;
            }

            return result;
        }

        private static Int64 GetTime()
        {
            Int64 retval;
            var st = new DateTime(1970, 1, 1);
            var t = (DateTime.Now.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }
    }
}