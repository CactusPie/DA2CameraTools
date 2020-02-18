using System.Collections.Generic;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories
{
    public class KeyAndMouseEventHandlerFactory : IKeyAndMouseEventHandlerFactory
    {
        private readonly IActionLoopServiceFactory _actionLoopServiceFactory;

        public KeyAndMouseEventHandlerFactory(IActionLoopServiceFactory actionLoopServiceFactory)
        {
            _actionLoopServiceFactory = actionLoopServiceFactory;
        }

        public ICameraMovementKeyHandler CreateCameraMovementHandler(
            IGameValueService gameValueService, 
            CameraMovementKeys cameraMovementKeys, 
            float cameraMovementSpeed)
        {
            return new CameraMovementKeyHandler(gameValueService, _actionLoopServiceFactory, cameraMovementKeys, cameraMovementSpeed);
        }

        public IKeyHandler CreateCameraHeightHandler(IGameValueService gameValueService, CameraHeightKeys cameraHeightKeys, float cameraMovementOffset)
        {
            return new CameraHeightKeyHandler(gameValueService, _actionLoopServiceFactory, cameraHeightKeys, cameraMovementOffset);
        }

        public IAutoTacticalCameraKeyHandler CreateAutoTacticalCameraHandler(
            IGameValueService gameValueService, 
            AutoTacticalCameraKeys autoTacticalCameraKeys, 
            float turnOnTacticalCameraThreshold)
        {
            return new AutoTacticalCameraKeyHandler(gameValueService, autoTacticalCameraKeys, turnOnTacticalCameraThreshold);
        }

        public IManualTacticalCameraKeyHandler CreateManualTacticalCameraHandler(IGameValueService gameValueService, IEnumerable<UserInputKey> tacticalCameraToggleKeys)
        {
            return new ManualTacticalCameraKeyHandler(gameValueService, tacticalCameraToggleKeys);
        }
    }
}