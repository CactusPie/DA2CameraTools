using System.Collections.Generic;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data
{
    public class CameraMovementKeys
    {
        public IEnumerable<UserInputKey> CameraForwardKeys { get; set; }
        public IEnumerable<UserInputKey> CameraBackKeys { get; set; }
        public IEnumerable<UserInputKey> CameraLeftKeys { get; set; }
        public IEnumerable<UserInputKey> CameraRightKeys { get; set; }
    }
}