using System;
using System.Globalization;
using System.Windows.Data;
using DragonAge2CameraTools.ViewLogic.ViewModels;

namespace DragonAge2CameraTools.Wpf.Converters
{
    public class KeyBindingViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var binding = value as KeyBindingViewmodel;

            if (binding == null)
            {
                return "Invalid binding";
            }

            if (binding.IsInBinding)
            {
                return "Press any key...";
            }

            if (binding.Key == null)
            {
                return "Not bound";
            }

            return binding.Key.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}