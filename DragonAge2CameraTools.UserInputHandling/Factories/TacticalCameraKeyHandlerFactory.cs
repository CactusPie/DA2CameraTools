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
        private readonly ILoadingScreenMonitorFactory _loadingScreenMonitorFactory;

        public TacticalCameraKeyHandlerFactory(IKeyAndMouseEventHandlerFactory keyAndMouseEventHandlerFactory, ILoadingScreenMonitorFactory loadingScreenMonitorFactory)
        {
            _keyAndMouseEventHandlerFactory = keyAndMouseEventHandlerFactory;
            _loadingScreenMonitorFactory = loadingScreenMonitorFactory;
        }
        
        public IKeyHandler CreateTacticalCameraKeyHandler(IGameValueService gameValueService, TacticalCameraSettings tacticalCameraSettings)
        {
            return new TacticalCameraKeyHandler(_keyAndMouseEventHandlerFactory, _loadingScreenMonitorFactory, gameValueService, tacticalCameraSettings);
        }
    }
}