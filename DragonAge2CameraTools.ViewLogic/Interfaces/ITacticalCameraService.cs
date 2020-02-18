using System;
using System.Threading.Tasks;
using DragonAge2CameraTools.UserInputHandling.Data;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;

namespace DragonAge2CameraTools.ViewLogic.Interfaces
{
    public interface ITacticalCameraService : IDisposable
    {
        bool AreTacticalCameraTriggersEnabled { get; set; }
        GameProcessStatus ProcessStatus { get; set; }
        Task WaitAndAttachToProcess();
        void EnableTacticalCameraTriggers();
        void DisableTacticalCamera();
        void UpdateSettings(TacticalCameraSettings tacticalCameraSettings);
        event EventHandler<ProcessStatusChangeEventArgs> ProcessStatusChanged;
    }
}