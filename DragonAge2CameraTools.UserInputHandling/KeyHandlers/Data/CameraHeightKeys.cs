using System.Collections.Generic;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data
{
    public class CameraHeightKeys
    {
        public IEnumerable<UserInputKey> CameraUpKeys { get; set; }
        public IEnumerable<UserInputKey> CameraDownKeys { get; set; }

    }
}