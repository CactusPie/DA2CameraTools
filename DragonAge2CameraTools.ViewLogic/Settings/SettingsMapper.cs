using System.Collections.Generic;
using System.Linq;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.ViewLogic.Data;
using DragonAge2CameraTools.ViewLogic.Settings.Data;
using DragonAge2CameraTools.ViewLogic.Settings.Interfaces;
using DragonAge2CameraTools.ViewLogic.ViewModels;

namespace DragonAge2CameraTools.ViewLogic.Settings
{
    public class SettingsMapper : ISettingsMapper
    {
        public KeyBindingSettings MapToKeyBindingSettings(ViewModelKeyBindings viewModelKeyBindings)
        {
            return new KeyBindingSettings
            {
                TacticalCameraToggleKeys = viewModelKeyBindings.TacticalCameraToggleKeys.Select(x => x.Key).ToList(),
                CameraForwardKeys = viewModelKeyBindings.CameraForwardKeys.Select(x => x.Key).ToList(),
                CameraBackKeys = viewModelKeyBindings.CameraBackKeys.Select(x => x.Key).ToList(),
                CameraLeftKeys = viewModelKeyBindings.CameraLeftKeys.Select(x => x.Key).ToList(),
                CameraRightKeys = viewModelKeyBindings.CameraRightKeys.Select(x => x.Key).ToList(),
                CameraDownKeys = viewModelKeyBindings.CameraDownKeys.Select(x => x.Key).ToList(),
                CameraUpKeys = viewModelKeyBindings.CameraUpKeys.Select(x => x.Key).ToList(),
                ZoomInKeys = viewModelKeyBindings.ZoomInKeys.Select(x => x.Key).ToList(),
                ZoomOutKeys = viewModelKeyBindings.ZoomOutKeys.Select(x => x.Key).ToList(),
            };
        }
        
        public ViewModelKeyBindings MapToViewModelKeyBindings(KeyBindingSettings keyBindings)
        {
            return new ViewModelKeyBindings
            {
                TacticalCameraToggleKeys = MapToObservableCollection(keyBindings.TacticalCameraToggleKeys),
                CameraForwardKeys = MapToObservableCollection(keyBindings.CameraForwardKeys), 
                CameraBackKeys = MapToObservableCollection(keyBindings.CameraBackKeys),
                CameraLeftKeys = MapToObservableCollection(keyBindings.CameraLeftKeys),
                CameraRightKeys = MapToObservableCollection(keyBindings.CameraRightKeys),
                CameraDownKeys = MapToObservableCollection(keyBindings.CameraDownKeys),
                CameraUpKeys = MapToObservableCollection(keyBindings.CameraUpKeys),
                ZoomInKeys = MapToObservableCollection(keyBindings.ZoomInKeys),
                ZoomOutKeys = MapToObservableCollection(keyBindings.ZoomOutKeys),
            };
        }
        
        public TacticalCameraSettings MapToTacticalCameraSettings(ApplicationSettings applicationSettings)
        {
            return new TacticalCameraSettings
            {
                TacticalCameraKeyBindings = MapToTacticalCameraKeyBindings(applicationSettings.KeyBindingSettings),
                UnlimitedZoomEnabled = applicationSettings.UnlimitedZoomEnabled,
                AutomaticTacticalCameraEnabled = applicationSettings.AutomaticTacticalCameraEnabled,
                ManualTacticalCameraEnabled = applicationSettings.ManualTacticalCameraEnabled,
                AutomaticTacticalCameraThreshold = applicationSettings.AutomaticCameraThresholdValue,
                HorizontalCameraSpeed = applicationSettings.HorizontalCameraSpeed,
                VerticalCameraSpeed = applicationSettings.VerticalCameraSpeed
            };
        }
        
        private static TacticalCameraKeyBindings MapToTacticalCameraKeyBindings(KeyBindingSettings keyBindings)
        {
            return new TacticalCameraKeyBindings
            {
                TacticalCameraToggleKeys = MapKeysToArray(keyBindings.TacticalCameraToggleKeys),
                CameraForwardKeys = MapKeysToArray(keyBindings.CameraForwardKeys),
                CameraBackKeys = MapKeysToArray(keyBindings.CameraBackKeys),
                CameraLeftKeys = MapKeysToArray(keyBindings.CameraLeftKeys),
                CameraRightKeys = MapKeysToArray(keyBindings.CameraRightKeys),
                CameraDownKeys = MapKeysToArray(keyBindings.CameraDownKeys),
                CameraUpKeys = MapKeysToArray(keyBindings.CameraUpKeys),
                ZoomInKeys = MapKeysToArray(keyBindings.ZoomInKeys),
                ZoomOutKeys = MapKeysToArray(keyBindings.ZoomOutKeys),
            };
        }

        private static FullyObservableCollection<KeyBindingViewmodel> MapToObservableCollection(IEnumerable<UserInputKey?> keys)
        {
            var viewModels = keys.Select(key => new KeyBindingViewmodel(key));
            return new FullyObservableCollection<KeyBindingViewmodel>(viewModels);
        }

        private static UserInputKey[] MapKeysToArray(IEnumerable<UserInputKey?> keys)
        {
            return keys.Where(x => x.HasValue).Select(x => x.Value).ToArray();
        }
    }
}