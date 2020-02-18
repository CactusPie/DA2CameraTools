using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.Wpf.Converters
{
    public class GameProcessStatusLabelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gameProcessStatus = value as GameProcessStatus?;

            if (gameProcessStatus == null)
            {
                return new SolidColorBrush(Colors.Red);
            }
            
            switch (gameProcessStatus)
            {
                case GameProcessStatus.ProcessNotAvailable:
                    return new SolidColorBrush(Colors.Red);
                case GameProcessStatus.WaitingUntilReadyForInjection:
                    return new SolidColorBrush(Colors.DarkBlue);
                case GameProcessStatus.Attached:
                    return new SolidColorBrush(Colors.DarkGreen);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}