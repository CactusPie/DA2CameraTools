using System;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.UserInputHandling.Data
{
    public class ProcessStatusChangeEventArgs : EventArgs
    {
        public GameProcessStatus NewStatus { get; }

        public ProcessStatusChangeEventArgs(GameProcessStatus newStatus)
        {
            NewStatus = newStatus;
        }
    }
}