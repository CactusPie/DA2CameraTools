using System.Collections.Generic;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories.Interfaces
{
    public interface IKeyAndMouseEventHandlerFactory
    {
        ICameraMovementKeyHandler CreateCameraMovementHandler(IGameValueService gameValueService,
            CameraMovementKeys cameraMovementKeys, float cameraMovementSpeed);
        IKeyHandler CreateCameraHeightHandler(IGameValueService gameValueService, CameraHeightKeys cameraHeightKeys,
            float cameraMovementOffset);
        IAutoTacticalCameraKeyHandler CreateAutoTacticalCameraHandler(IGameValueService gameValueService, AutoTacticalCameraKeys autoTacticalCameraKeys, float turnOnTacticalCameraThreshold);
        IManualTacticalCameraKeyHandler CreateManualTacticalCameraHandler(IGameValueService gameValueService, IEnumerable<UserInputKey> tacticalCameraToggleKeys);
    }
}