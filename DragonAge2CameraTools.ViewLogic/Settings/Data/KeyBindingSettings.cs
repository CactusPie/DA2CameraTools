using System.Collections.Generic;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.ViewLogic.Settings.Data
{
    public class KeyBindingSettings
    {
        public List<UserInputKey?> TacticalCameraToggleKeys { get; set; }
        public List<UserInputKey?> CameraForwardKeys { get; set; }
        public List<UserInputKey?> CameraBackKeys { get; set; }
        public List<UserInputKey?> CameraLeftKeys { get; set; }
        public List<UserInputKey?> CameraRightKeys { get; set; }
        public List<UserInputKey?> CameraDownKeys { get; set; }
        public List<UserInputKey?> CameraUpKeys { get; set; }
        public List<UserInputKey?> ZoomInKeys { get; set; }
        public List<UserInputKey?> ZoomOutKeys { get; set; }
    }
}