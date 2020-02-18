using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.ViewLogic.Data;
using DragonAge2CameraTools.ViewLogic.Settings.Data;

namespace DragonAge2CameraTools.ViewLogic.Settings.Interfaces
{
    public interface ISettingsMapper
    {
        KeyBindingSettings MapToKeyBindingSettings(ViewModelKeyBindings viewModelKeyBindings);
        ViewModelKeyBindings MapToViewModelKeyBindings(KeyBindingSettings keyBindings);
        TacticalCameraSettings MapToTacticalCameraSettings(ApplicationSettings applicationSettings);
    }
}