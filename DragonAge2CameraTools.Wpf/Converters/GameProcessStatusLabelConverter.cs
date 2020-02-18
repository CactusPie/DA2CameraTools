using System;
using System.Globalization;
using System.Windows.Data;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.Wpf.Converters
{
    public class GameProcessStatusLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gameProcessStatus = value as GameProcessStatus?;

            if (gameProcessStatus == null)
            {
                return "Invalid binding";
            }

            switch (gameProcessStatus)
            {
                case GameProcessStatus.ProcessNotAvailable:
                    return "Waiting for Dragon Age 2 process";
                case GameProcessStatus.WaitingUntilReadyForInjection:
                    return "Waiting until the game is loaded";
                case GameProcessStatus.Attached:
                    return "Attached to process";
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