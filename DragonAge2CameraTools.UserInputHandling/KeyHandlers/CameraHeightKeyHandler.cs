using System.Linq;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers
{
    /// <summary>
    /// Handler for camera height (zoom) movement. Should be enabled only
    /// when tactical camera is enabled, otherwise it will conflict with
    /// game built-in camera movement
    /// </summary>
    public class CameraHeightKeyHandler : IKeyHandler
    {
        private bool _isCameraMovingDown;
        private bool _isCameraMovingUp;
        private bool _isHandlerEnabled;
        
        public float CameraMovementOffset { get; set; }
        
        public bool IsHandlerEnabled
        {
            get => _isHandlerEnabled;
            set
            {
                if (!value)
                {
                    StopCurrentlyRunningKeyFunction();
                }

                _isHandlerEnabled = value;
            }
        }
        
        private readonly IGameValueService _gameValueService;
        private readonly IActionLoopService _actionLoopService;
        private readonly CameraHeightKeys _cameraHeightKeys;

        public CameraHeightKeyHandler(
            IGameValueService gameValueService, 
            IActionLoopServiceFactory actionLoopServiceFactory, 
            CameraHeightKeys cameraHeightKeys, 
            float cameraMovementOffset)
        {
            _gameValueService = gameValueService;
            _actionLoopService = actionLoopServiceFactory.CreateActionLoopService();
            _cameraHeightKeys = cameraHeightKeys;
            CameraMovementOffset = cameraMovementOffset;
        }
        
        public InputResult OnKeyDown(UserInputKey keyCode, ModifierKeys modifiers)
        {
            if (!IsHandlerEnabled || modifiers != ModifierKeys.None)
            {
                return InputResult.Continue;
            }
            
            InputResult inputResult;
            float cameraHeight;
            
            // Mouse scroll requires more effort to scroll as much as regular keys,
            // so we add a multiplier to keep it more consistent with other keys
            const int mouseScrollMultiplier = 3;

            if (_cameraHeightKeys.CameraUpKeys.Contains(keyCode))
            {
                if (keyCode == UserInputKey.MouseScrollDown || keyCode == UserInputKey.MouseScrollUp)
                {
                    cameraHeight = _gameValueService.GetZCameraPosition();
                    cameraHeight += CameraMovementOffset * mouseScrollMultiplier;
                    _gameValueService.SetZCameraPosition(cameraHeight);
                    inputResult = InputResult.StopProcessingEvents | InputResult.HideFromOtherApplications;
                }
                else
                {
                    _isCameraMovingUp = true;
                    inputResult = InputResult.StopProcessingEvents | InputResult.HideFromOtherApplications;
                }
            }
            else if (_cameraHeightKeys.CameraDownKeys.Contains(keyCode))
            {
                if (keyCode == UserInputKey.MouseScrollDown || keyCode == UserInputKey.MouseScrollUp)
                {
                    cameraHeight = _gameValueService.GetZCameraPosition();
                    cameraHeight -= CameraMovementOffset * mouseScrollMultiplier;
                    _gameValueService.SetZCameraPosition(cameraHeight);
                    inputResult = InputResult.StopProcessingEvents | InputResult.HideFromOtherApplications;
                }
                else
                {
                    _isCameraMovingDown = true;
                    inputResult = InputResult.StopProcessingEvents | InputResult.HideFromOtherApplications;
                }
            }
            else
            {
                inputResult = InputResult.Continue;
            }

            if (!_actionLoopService.IsLoopingAction && (_isCameraMovingDown || _isCameraMovingUp))
            {
                _actionLoopService.StartLoopingAction(MoveCamera);
            }

            return inputResult;
        }

        public InputResult OnKeyUp(UserInputKey keyCode)
        {
            if (!_isHandlerEnabled)
            {
                return InputResult.Continue;
            }
            
            InputResult inputResult;

            if (_cameraHeightKeys.CameraUpKeys.Contains(keyCode))
            {
                _isCameraMovingUp = false;
                inputResult = InputResult.StopProcessingEvents | InputResult.HideFromOtherApplications;
            }
            else if (_cameraHeightKeys.CameraDownKeys.Contains(keyCode))
            {
                _isCameraMovingDown = false;
                inputResult = InputResult.StopProcessingEvents | InputResult.HideFromOtherApplications;
            }
            else
            {
                inputResult = InputResult.Continue;
            }

            if (_actionLoopService.IsLoopingAction && !(_isCameraMovingDown || _isCameraMovingUp))
            {
                _actionLoopService.StopLoopingAction();
            }
            
            return inputResult;
        }

        public void StopCurrentlyRunningKeyFunction()
        {
            if (_actionLoopService.IsLoopingAction)
            {
                _isCameraMovingDown = false;
                _isCameraMovingUp = false;
                _actionLoopService.StopLoopingAction();
            }
        }

        private void MoveCamera()
        {
            if (_isCameraMovingUp)
            {
                if (!_isCameraMovingDown)
                {
                    float zPosition = _gameValueService.GetZCameraPosition();
                    _gameValueService.SetZCameraPosition(zPosition + CameraMovementOffset);
                }
            }
            else if (_isCameraMovingDown)
            {
                float zPosition = _gameValueService.GetZCameraPosition();
                _gameValueService.SetZCameraPosition(zPosition - CameraMovementOffset);
            }
        }
        
        public void Dispose()
        {
            if (_actionLoopService.IsLoopingAction)
            {
                _actionLoopService.StopLoopingAction();
            }
        }
    }
}