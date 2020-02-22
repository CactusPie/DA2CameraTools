using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
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
        private Process _process;
        
        private readonly ICameraToolsFactory _cameraToolsFactory;
        private readonly ITacticalCameraKeyHandlerFactory _tacticalCameraKeyHandlerFactory;
        private readonly IUserInputHandlerFactory _userInputHandlerFactory;
        private readonly ICodeInjectionReadinessChecker _codeInjectionReadinessChecker;
        private TacticalCameraSettings _tacticalCameraSettings;

        public bool AreTacticalCameraTriggersEnabled { get; set; }
        public GameProcessStatus ProcessStatus { get; set; }
        public event EventHandler<ProcessStatusChangeEventArgs> ProcessStatusChanged;

        public TacticalCameraService(
            ICameraToolsFactory cameraToolsFactory, 
            ITacticalCameraKeyHandlerFactory tacticalCameraKeyHandlerFactory,
            IUserInputHandlerFactory userInputHandlerFactory,
            ICodeInjectionReadinessChecker codeInjectionReadinessChecker,
            TacticalCameraSettings tacticalCameraSettings)
        {
            _cameraToolsFactory = cameraToolsFactory;
            _tacticalCameraKeyHandlerFactory = tacticalCameraKeyHandlerFactory;
            _userInputHandlerFactory = userInputHandlerFactory;
            _codeInjectionReadinessChecker = codeInjectionReadinessChecker;
            _tacticalCameraSettings = tacticalCameraSettings;
        }
        
        public async Task WaitAndAttachToProcess()
        {
            ProcessStatus = GameProcessStatus.ProcessNotAvailable;
            while (_process == null)
            {
                _process = Process.GetProcessesByName("DragonAge2").FirstOrDefault();

                if (_process != null)
                {
                    _process.EnableRaisingEvents = true;
                    _process.Exited += OnGameProcessExited;
                    break;
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }

            OnGameProcessStatusChange(GameProcessStatus.WaitingUntilReadyForInjection);
            
            await _codeInjectionReadinessChecker.WaitUntilCodeIsReadyToBeInjected(_process);
            _gameValueService = _cameraToolsFactory.CreateGameValueService(_process.Handle, _process);
            
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

            _keyHandler = _tacticalCameraKeyHandlerFactory.CreateTacticalCameraKeyHandler(_gameValueService, _tacticalCameraSettings);
            
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
            _keyHandler = _tacticalCameraKeyHandlerFactory.CreateTacticalCameraKeyHandler(_gameValueService, _tacticalCameraSettings);
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
            _process.Dispose();
            _process = null;

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
            
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }
        }
    }
}