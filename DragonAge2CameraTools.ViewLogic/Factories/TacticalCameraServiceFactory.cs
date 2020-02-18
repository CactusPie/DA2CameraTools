using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.ViewLogic.Factories.Interfaces;
using DragonAge2CameraTools.ViewLogic.Interfaces;

namespace DragonAge2CameraTools.ViewLogic.Factories
{
    public class TacticalCameraServiceFactory : ITacticalCameraServiceFactory
    {
        private readonly ICameraToolsFactory _cameraToolsFactory;
        private readonly IUserInputHandlerFactory _userInputHandlerFactory;
        private readonly ICodeInjectionReadinessChecker _codeInjectionReadinessChecker;
        private readonly ITacticalCameraKeyHandlerFactory _tacticalCameraKeyHandlerFactory;

        public TacticalCameraServiceFactory(
            ICameraToolsFactory cameraToolsFactory, 
            IUserInputHandlerFactory userInputHandlerFactory,
            ICodeInjectionReadinessChecker codeInjectionReadinessChecker,
            ITacticalCameraKeyHandlerFactory tacticalCameraKeyHandlerFactory)
        {
            _cameraToolsFactory = cameraToolsFactory;
            _userInputHandlerFactory = userInputHandlerFactory;
            _codeInjectionReadinessChecker = codeInjectionReadinessChecker;
            _tacticalCameraKeyHandlerFactory = tacticalCameraKeyHandlerFactory;
        }

        public ITacticalCameraService CreateTacticalCameraService(TacticalCameraSettings tacticalCameraSettings)
        {
            return new TacticalCameraService(
                _cameraToolsFactory, 
                _tacticalCameraKeyHandlerFactory,
                _userInputHandlerFactory, 
                _codeInjectionReadinessChecker, 
                tacticalCameraSettings);
        }
    }
}