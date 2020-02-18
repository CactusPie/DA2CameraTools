using DragonAge2CameraTools.ViewLogic.Settings.Data;

namespace DragonAge2CameraTools.ViewLogic.Settings.Interfaces
{
    public interface IApplicationSettingsRepository
    {
        ApplicationSettings GetApplicationSettings();
        void SaveApplicationSettings(ApplicationSettings applicationSettings);
        ApplicationSettings GetDefaultSettings();
    }
}