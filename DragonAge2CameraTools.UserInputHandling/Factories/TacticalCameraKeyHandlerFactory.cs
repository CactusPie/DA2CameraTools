using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories
{
    public class TacticalCameraKeyHandlerFactory : ITacticalCameraKeyHandlerFactory
    {
        private readonly IKeyAndMouseEventHandlerFactory _keyAndMouseEventHandlerFactory;
        private readonly IGameEventServiceFactory _gameEventServiceFactory;

        public TacticalCameraKeyHandlerFactory(IKeyAndMouseEventHandlerFactory keyAndMouseEventHandlerFactory, IGameEventServiceFactory gameEventServiceFactory)
        {
            _keyAndMouseEventHandlerFactory = keyAndMouseEventHandlerFactory;
            _gameEventServiceFactory = gameEventServiceFactory;
        }
        
        public IKeyHandler CreateTacticalCameraKeyHandler(IGameValueService gameValueService, TacticalCameraSettings tacticalCameraSettings, Process gameProcess)
        {
            return new TacticalCameraKeyHandler(_keyAndMouseEventHandlerFactory, _gameEventServiceFactory, gameValueService, gameProcess, tacticalCameraSettings);
        }
    }
}