using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.ViewLogic.Interfaces;

namespace DragonAge2CameraTools.ViewLogic.Factories.Interfaces
{
    public interface ITacticalCameraServiceFactory
    {
        ITacticalCameraService CreateTacticalCameraService(TacticalCameraSettings tacticalCameraSettings);
    }
}