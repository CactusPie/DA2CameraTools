using System.Collections.Generic;
using System.IO;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.ViewLogic.Settings.Data;
using DragonAge2CameraTools.ViewLogic.Settings.Interfaces;

namespace DragonAge2CameraTools.ViewLogic.Settings
{
    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        private readonly IStringSerializer _stringSerializer;
        private readonly string _filePath;

        public ApplicationSettingsRepository(IStringSerializer stringSerializer, string filePath)
        {
            _stringSerializer = stringSerializer;
            _filePath = filePath;
        }
        
        public ApplicationSettings GetApplicationSettings()
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }
            
            string serializedSettings = File.ReadAllText(_filePath);
            return _stringSerializer.Deserialize<ApplicationSettings>(serializedSettings);
        }
        
        public ApplicationSettings GetDefaultSettings()
        {
            return new ApplicationSettings
            {
                KeyBindingSettings = new KeyBindingSettings
                {
                    TacticalCameraToggleKeys = new List<UserInputKey?> {UserInputKey.MiddleMouseButton, UserInputKey.F12},
                    CameraForwardKeys = new List<UserInputKey?> {UserInputKey.W, UserInputKey.Up},
                    CameraBackKeys = new List<UserInputKey?> {UserInputKey.S, UserInputKey.Down},
                    CameraLeftKeys = new List<UserInputKey?> {UserInputKey.A, UserInputKey.Left},
                    CameraRightKeys = new List<UserInputKey?> {UserInputKey.D, UserInputKey.Right},
                    CameraDownKeys = new List<UserInputKey?> {UserInputKey.MouseScrollUp, UserInputKey.F},
                    CameraUpKeys = new List<UserInputKey?> {UserInputKey.MouseScrollDown, UserInputKey.R},
                    ZoomInKeys = new List<UserInputKey?> {UserInputKey.MouseScrollUp, UserInputKey.PageDown},
                    ZoomOutKeys = new List<UserInputKey?> {UserInputKey.MouseScrollDown, UserInputKey.PageUp},
                },
                UnlimitedZoomEnabled = true,
                ManualTacticalCameraEnabled = true,
                AutomaticTacticalCameraEnabled = true,
                AutomaticCameraThresholdValue = 3,
                HorizontalCameraSpeed = 0.3f,
                VerticalCameraSpeed = 0.3f,
            };
        }

        public void SaveApplicationSettings(ApplicationSettings applicationSettings)
        {
            string serializedSettings = _stringSerializer.Serialize(applicationSettings);
            File.WriteAllText(_filePath, serializedSettings);
        }
    }
}