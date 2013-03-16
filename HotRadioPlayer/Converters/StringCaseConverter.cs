using System;
using Windows.UI.Xaml.Data;

namespace HotRadioPlayer.Converters
{
    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var textToConvert = (value == null) ? "" : value.ToString();
            var isToLower = true;
            if (parameter != null) isToLower = System.Convert.ToBoolean(parameter.ToString());

            return isToLower ? textToConvert.ToLower() : textToConvert.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
