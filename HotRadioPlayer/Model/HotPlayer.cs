using System.Runtime.Serialization;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace HotRadioPlayer.Model
{
    [DataContract]
    public class Track
    {
        [DataMember]
        public string TrackID { get; set; }
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string Artist { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string ArtistSlug { get; set; }
        [DataMember]
        public string DownloadUrl { get; set; }
        [DataMember]
        public string CDUrl { get; set; }
        [DataMember]
        public string Image { get; set; }

        public string BigImage
        {
            get
            {
                if (Image.Contains("-75.jpg"))
                {
                    var retValue = Image.Replace("100x100", "225x225");
                    retValue = retValue.Replace("170x170", "225x225");
                    return retValue;
                }
                return Image;
            }
        }
    }

    public class TrackWrapper : Track
    {
        public TrackWrapper(Track item)
        {
            TrackID = item.TrackID;
            Time = item.Time;
            Artist = item.Artist;
            ArtistSlug = item.ArtistSlug;
            Title = item.Title;
            DownloadUrl = item.DownloadUrl;
            CDUrl = item.CDUrl;
            Image = item.Image;
        }
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
        public Visibility NowPlayingVisibility { get; set; }
    }

    [DataContract]
    public class OnAirBase
    {
        [DataMember]
        public string StaffID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ImageOnAir { get; set; }
        [DataMember]
        public string ImagePlayer { get; set; }
        [DataMember]
        public string Times { get; set; }
        [DataMember]
        public string Caption { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PageUrl { get; set; }
        [DataMember]
        public string EmailUrl { get; set; }
    }

    public class OnAirNow : OnAirBase
    {
        
    }

    public class OnAirNext : OnAirBase
    {
        
    }

    [DataContract]
    public class HotPlayer
    {
        [DataMember]
        public List<Track> Last10Tracks { get; set; }
        [DataMember]
        public OnAirNow OnAirNow { get; set; }
        [DataMember]
        public OnAirNext OnAirNext { get; set; }
    }
}
