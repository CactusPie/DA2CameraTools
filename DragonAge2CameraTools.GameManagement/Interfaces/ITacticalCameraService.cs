using System;

namespace DragonAge2CameraTools.GameManagement.Interfaces
{
    public interface ITacticalCameraService : IDisposable
    {
        bool IsTacticalCameraEnabled { get; set; }
        bool TryAttachToProcess();
        void EnableTacticalCamera();
        void DisableTacticalCamera();
    }
}