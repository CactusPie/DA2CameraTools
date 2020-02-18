using System.Collections.Generic;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data
{
    public class AutoTacticalCameraKeys
    {
        public IEnumerable<UserInputKey> ZoomOutKeys { get; set; }
        public IEnumerable<UserInputKey> ZoomInKeys { get; set; }
        public IEnumerable<UserInputKey> ToggleKeys { get; set; }
    }
}