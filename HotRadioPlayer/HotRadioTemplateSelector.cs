using HotRadioPlayer.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HotRadioPlayer
{
    public class HotRadioTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OnAirNowDataTemplate { get; set; }
        public DataTemplate OnNextDataTemplate { get; set; }
        public DataTemplate TrackDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var type = item.GetType();

            if (type == typeof (OnAirNext))
            {
                return OnNextDataTemplate;
            }
            if (type == typeof (OnAirNow))
            {
                return OnAirNowDataTemplate;
            }
            return type == typeof (TrackWrapper) ? TrackDataTemplate : base.SelectTemplateCore(item, container);
        }
    }
}
