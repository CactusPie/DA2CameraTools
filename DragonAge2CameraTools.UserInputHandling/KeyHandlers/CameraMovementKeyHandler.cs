using System;
using System.Linq;
using DragonAge2CameraTools.GameManagement.Data;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers
{
    /// <inheritdoc/>>
    public class CameraMovementKeyHandler : ICameraMovementKeyHandler
    {
        private bool _isCameraMovingBack;
        private bool _isCameraMovingUp;
        private bool _isCameraMovingLeft;
        private bool _isCameraMovingRight;
        private bool _isCameraMovingInAnyDirection;
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
        private readonly CameraMovementKeys _cameraMovementKeys;

        public CameraMovementKeyHandler(
            IGameValueService gameValueService, 
            IActionLoopServiceFactory actionLoopServiceFactory, 
            CameraMovementKeys cameraMovementKeys, 
            float cameraMovementOffset)
        {
            _gameValueService = gameValueService;
            _actionLoopService = actionLoopServiceFactory.CreateActionLoopService();
            _cameraMovementKeys = cameraMovementKeys;
            CameraMovementOffset = cameraMovementOffset;
        }
        
        public InputResult OnKeyDown(UserInputKey keyCode, ModifierKeys modifiers)
        {
            if (!IsHandlerEnabled || modifiers != ModifierKeys.None)
            {
                return InputResult.Continue;
            }
            
            InputResult inputResult;

            if (_cameraMovementKeys.CameraForwardKeys.Contains(keyCode))
            {
                _isCameraMovingUp = true;
                inputResult = InputResult.HideFromOtherApplications | InputResult.StopProcessingEvents;
            }
            else if (_cameraMovementKeys.CameraBackKeys.Contains(keyCode))
            {
                _isCameraMovingBack = true;
                inputResult = InputResult.HideFromOtherApplications | InputResult.StopProcessingEvents;
            }
            else if (_cameraMovementKeys.CameraLeftKeys.Contains(keyCode))
            {
                _isCameraMovingLeft = true;
                inputResult = InputResult.HideFromOtherApplications | InputResult.StopProcessingEvents;
            }
            else if (_cameraMovementKeys.CameraRightKeys.Contains(keyCode))
            {
                _isCameraMovingRight = true;
                inputResult = InputResult.HideFromOtherApplications | InputResult.StopProcessingEvents;
            }
            else
            {
                inputResult = InputResult.Continue;
            }

            if (!_actionLoopService.IsLoopingAction && 
                (_isCameraMovingUp || _isCameraMovingBack || _isCameraMovingLeft || _isCameraMovingRight))
            {
                _isCameraMovingInAnyDirection = true;
                _actionLoopService.StartLoopingAction(MoveCamera);
            }

            return inputResult;
        }
        
        public InputResult OnKeyUp(UserInputKey keyCode)
        {
            if (!IsHandlerEnabled || !_isCameraMovingInAnyDirection)
            {
                return InputResult.Continue;
            }

            if (_cameraMovementKeys.CameraForwardKeys.Contains(keyCode))
            {
                _isCameraMovingUp = false;
            }
            else if (_cameraMovementKeys.CameraBackKeys.Contains(keyCode))
            {
                _isCameraMovingBack = false;
            }
            else if (_cameraMovementKeys.CameraLeftKeys.Contains(keyCode))
            {
                _isCameraMovingLeft = false;
            }
            else if (_cameraMovementKeys.CameraRightKeys.Contains(keyCode))
            {
                _isCameraMovingRight = false;
            }

            if (!(_isCameraMovingUp || _isCameraMovingBack || _isCameraMovingLeft || _isCameraMovingRight))
            {
                _isCameraMovingInAnyDirection = false;
                _actionLoopService.StopLoopingAction();
            }

            return InputResult.Continue;
        }

        public void StopCurrentlyRunningKeyFunction()
        {
            if (_actionLoopService.IsLoopingAction)
            {
                _isCameraMovingInAnyDirection = false;
                _isCameraMovingBack = false;
                _isCameraMovingLeft = false;
                _isCameraMovingRight = false;
                _isCameraMovingUp = false;
                _actionLoopService.StopLoopingAction();
            }
        }

        private void MoveCamera()
        {
            var angleRetrieved = false;
            float horizontalCameraAngle = 0;
            float horizontalCameraAngleUpDown = 0;
            float horizontalCameraAngleLeftRight = 0;

            if (_isCameraMovingUp)
            {
                if (!_isCameraMovingBack)
                {
                    horizontalCameraAngle = _gameValueService.GetHorizontalCameraAngle();
                    angleRetrieved = true;
                    horizontalCameraAngleUpDown = (float) (horizontalCameraAngle + Math.PI);
                }
            }
            else if (_isCameraMovingBack)
            {
                horizontalCameraAngle = _gameValueService.GetHorizontalCameraAngle();
                angleRetrieved = true;
                horizontalCameraAngleUpDown = horizontalCameraAngle;
            }
            
            if (_isCameraMovingRight)
            {
                if (!_isCameraMovingLeft)
                {
                    if (!angleRetrieved)
                    {
                        horizontalCameraAngle = _gameValueService.GetHorizontalCameraAngle();
                    }
                    
                    horizontalCameraAngleLeftRight = (float) (horizontalCameraAngle + 0.5 * Math.PI);
                }
            }
            else if(_isCameraMovingLeft)
            {
                if (!angleRetrieved)
                {
                    horizontalCameraAngle = _gameValueService.GetHorizontalCameraAngle();
                }
                
                horizontalCameraAngleLeftRight = (float) (horizontalCameraAngle + 1.5 * Math.PI);
            }

            float xOffset = 0, yOffset = 0;
            if (horizontalCameraAngleUpDown != 0)
            {
                xOffset += (float)Math.Cos(horizontalCameraAngleUpDown);
                yOffset += (float)Math.Sin(horizontalCameraAngleUpDown);
            }
            
            if (horizontalCameraAngleLeftRight != 0)
            {
                xOffset += (float)Math.Cos(horizontalCameraAngleLeftRight);
                yOffset += (float)Math.Sin(horizontalCameraAngleLeftRight);
            }
            
            ChangeCameraPosition(xOffset * CameraMovementOffset, yOffset* CameraMovementOffset);
        }

        private void ChangeCameraPosition(float xOffset, float yOffset)
        {
            if (xOffset != 0)
            {
                if (yOffset != 0)
                {
                    XYCameraPosition cameraPosition = _gameValueService.GetXYCameraPosition();
                
                    float newXPosition = cameraPosition.XPosition + xOffset;
                    float newYPosition = cameraPosition.YPosition + yOffset;
                    
                    _gameValueService.SetCameraPosition(newXPosition, newYPosition);
                }
                else
                {
                    float xCameraPosition = _gameValueService.GetXCameraPosition();
                    _gameValueService.SetXCameraPosition(xCameraPosition + xOffset);
                }
            }
            else if (yOffset != 0)
            {
                float yCameraPosition = _gameValueService.GetYCameraPosition();
                _gameValueService.SetYCameraPosition(yCameraPosition + yOffset);
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