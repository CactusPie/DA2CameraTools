using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories.Interfaces
{
    public interface ITacticalCameraKeyHandlerFactory
    {
        IKeyHandler CreateTacticalCameraKeyHandler(IGameValueService gameValueService, TacticalCameraSettings tacticalCameraSettings, Process gameProcess);
    }
}