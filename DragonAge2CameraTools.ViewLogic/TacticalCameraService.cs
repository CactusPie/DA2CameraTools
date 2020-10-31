using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Data;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;
using DragonAge2CameraTools.ViewLogic.Interfaces;

namespace DragonAge2CameraTools.ViewLogic
{
    public class TacticalCameraService : ITacticalCameraService
    {
        private IUserInputHandler _userInputHandler;
        private IGameValueService _gameValueService;
        private IKeyHandler _keyHandler;
        private Process _gameProcess;
        
        private readonly ICameraToolsFactory _cameraToolsFactory;
        private readonly ITacticalCameraKeyHandlerFactory _tacticalCameraKeyHandlerFactory;
        private readonly IUserInputHandlerFactory _userInputHandlerFactory;
        private readonly ICodeInjectionReadinessChecker _codeInjectionReadinessChecker;
        private readonly IGameFunctionHookServiceFactory _gameFunctionHookServiceFactory;
        private TacticalCameraSettings _tacticalCameraSettings;

        public bool AreTacticalCameraTriggersEnabled { get; set; }
        public GameProcessStatus ProcessStatus { get; set; }
        public event EventHandler<ProcessStatusChangeEventArgs> ProcessStatusChanged;

        public TacticalCameraService
        (
            ICameraToolsFactory cameraToolsFactory, 
            ITacticalCameraKeyHandlerFactory tacticalCameraKeyHandlerFactory,
            IUserInputHandlerFactory userInputHandlerFactory,
            ICodeInjectionReadinessChecker codeInjectionReadinessChecker,
            IGameFunctionHookServiceFactory gameFunctionHookServiceFactory,
            TacticalCameraSettings tacticalCameraSettings
        )
        {
            _cameraToolsFactory = cameraToolsFactory;
            _tacticalCameraKeyHandlerFactory = tacticalCameraKeyHandlerFactory;
            _userInputHandlerFactory = userInputHandlerFactory;
            _codeInjectionReadinessChecker = codeInjectionReadinessChecker;
            _gameFunctionHookServiceFactory = gameFunctionHookServiceFactory;
            _tacticalCameraSettings = tacticalCameraSettings;
        }
        
        public async Task WaitAndAttachToProcess()
        {
            ProcessStatus = GameProcessStatus.ProcessNotAvailable;
            while (_gameProcess == null)
            {
                _gameProcess = Process.GetProcessesByName("DragonAge2").FirstOrDefault();

                if (_gameProcess != null)
                {
                    _gameProcess.EnableRaisingEvents = true;
                    _gameProcess.Exited += OnGameProcessExited;
                    break;
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }

            OnGameProcessStatusChange(GameProcessStatus.WaitingUntilReadyForInjection);
            
            await _codeInjectionReadinessChecker.WaitUntilCodeIsReadyToBeInjected(_gameProcess);
            _gameValueService = _cameraToolsFactory.CreateGameValueService(_gameProcess);
            
            OnGameProcessStatusChange(GameProcessStatus.Attached);
        }

        public void EnableTacticalCameraTriggers()
        {
            if (AreTacticalCameraTriggersEnabled)
            {
                throw new InvalidOperationException("Tactical camera is already running.");    
            }

            AreTacticalCameraTriggersEnabled = true;
            if (_tacticalCameraSettings.UnlimitedZoomEnabled)
            {
                _gameValueService.EnableUnlimitedZoom();
            }
            _gameValueService.DisableFreeCamera();
            _gameValueService.EnableZoom();
            _gameValueService.EnableCollisionZoomAdjustment();
            _gameValueService.EnableAutoCameraAngleAdjustment();
            _gameValueService.EnableCenteringCameraBehindCharacter();
            _gameValueService.SetCameraZoomDistance(0);

            _keyHandler = _tacticalCameraKeyHandlerFactory.CreateTacticalCameraKeyHandler(_gameValueService, _tacticalCameraSettings, _gameProcess);
            
            _userInputHandler = _userInputHandlerFactory.CreateUserInputHandler
            (
                _keyHandler,
                _gameValueService
            );

            _userInputHandler.StartProcessingInputEvents();
        }

        public void DisableTacticalCamera()
        {
            if (_gameValueService != null)
            {
                _gameValueService.DisableUnlimitedZoom();
                _gameValueService.DisableFreeCamera();
                _gameValueService.EnableZoom();
                _gameValueService.EnableCollisionZoomAdjustment();
                _gameValueService.EnableAutoCameraAngleAdjustment();
                _gameValueService.EnableCenteringCameraBehindCharacter();
                _gameValueService.SetCameraZoomDistance(0);
            }

            DisposeHandlers();
            AreTacticalCameraTriggersEnabled = false;
        }

        public void UpdateSettings(TacticalCameraSettings tacticalCameraSettings)
        {
            DisableTacticalCamera();
            DisposeHandlers();
            _tacticalCameraSettings = tacticalCameraSettings;
            _keyHandler = _tacticalCameraKeyHandlerFactory.CreateTacticalCameraKeyHandler(_gameValueService, _tacticalCameraSettings, _gameProcess);
            EnableTacticalCameraTriggers();
            if (tacticalCameraSettings.UnlimitedZoomEnabled)
            {
                _gameValueService.EnableUnlimitedZoom();
            }
            else
            {
                _gameValueService.DisableUnlimitedZoom();
            }
        }
        
        private void OnGameProcessExited(object sender, EventArgs e)
        {
            DisposeHandlers();
            _gameProcess.Dispose();
            _gameProcess = null;

            OnGameProcessStatusChange(GameProcessStatus.ProcessNotAvailable);
        }

        private void OnGameProcessStatusChange(GameProcessStatus newStatus)
        {
            ProcessStatus = newStatus;

            if (newStatus != GameProcessStatus.Attached)
            {
                if (AreTacticalCameraTriggersEnabled)
                {
                    DisableTacticalCamera();
                }
            }
            else
            {
                EnableTacticalCameraTriggers();
            }
            
            var eventHandler = ProcessStatusChanged;
            eventHandler?.Invoke(this, new ProcessStatusChangeEventArgs(newStatus));
        }

        private void DisposeHandlers()
        {
            if (_keyHandler != null)
            {
                _keyHandler.Dispose();
                _keyHandler = null;
            }

            if (_userInputHandler != null)
            {
                _userInputHandler.Dispose();
                _userInputHandler = null;
            }
        }
        
        public void Dispose()
        {
            DisableTacticalCamera();
            
            if (_gameProcess != null)
            {
                _gameProcess.Dispose();
                _gameProcess = null;
            }
        }
    }
}