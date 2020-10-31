using System;
using System.Linq;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers
{
    /// <inheritdoc/>>
    public class AutoTacticalCameraKeyHandler : IAutoTacticalCameraKeyHandler
    {
        private readonly float _turnOnTacticalCameraThreshold;
        private float _turnOffTacticalCameraThreshold = 0f;

        public event TacticalCameraStateChangedHandler TacticalCameraStateChanged; 
        
        public bool IsTacticalCameraEnabled { get; private set; }
        public bool IsHandlerEnabled { get; set; } = true;

        private readonly IGameValueService _gameValueService;
        private readonly AutoTacticalCameraKeys _keys;

        public AutoTacticalCameraKeyHandler(IGameValueService gameValueService, AutoTacticalCameraKeys keys, float turnOnTacticalCameraZoomThreshold)
        {
            _gameValueService = gameValueService;
            _keys = keys;

            if (turnOnTacticalCameraZoomThreshold < 0)
            {
                throw new ArgumentException("Automatic camera threshold cannot be lower than 0", nameof(turnOnTacticalCameraZoomThreshold));
            }
            
            // The game uses slightly different values, where 1 is the closest zoom and -1 is the furthest zoom
            // If unlimited zoom is enabled we can go below -1
            _turnOnTacticalCameraThreshold = -turnOnTacticalCameraZoomThreshold;
        }

        public InputResult OnKeyDown(UserInputKey keyCode, ModifierKeys modifiers)
        {
            if (!IsHandlerEnabled)
            {
                return InputResult.Continue;
            }
            
            // zoom out
            if (_keys.ZoomOutKeys.Contains(keyCode))
            {
                float zoomValue = _gameValueService.GetCameraZoomDistance();
                float cameraHeight = _gameValueService.GetZCameraPosition();
                
                if (!IsTacticalCameraEnabled && zoomValue <= _turnOnTacticalCameraThreshold)
                {
                    _turnOffTacticalCameraThreshold = cameraHeight;
                    EnableTacticalCamera();
                }
                else
                {
                    return InputResult.Continue;
                }
            }
            else if (_keys.ZoomInKeys.Contains(keyCode))
            {
                float cameraHeight = _gameValueService.GetZCameraPosition();
                
                if (IsTacticalCameraEnabled)
                {
                    if (cameraHeight < _turnOffTacticalCameraThreshold)
                    {
                        DisableTacticalCamera();
                    }
                }
                else
                {
                    return InputResult.Continue;
                }
            }
            else if (_keys.ToggleKeys.Contains(keyCode))
            {
                if (IsTacticalCameraEnabled)
                {
                    DisableTacticalCamera();
                    return InputResult.HideFromOtherApplications;
                }
            }

            return InputResult.Continue;
        }
        
        public InputResult OnKeyUp(UserInputKey keyCode)
        {
            return InputResult.Continue;
        }
        
        public void StopCurrentlyRunningKeyFunction()
        {
        }
        
        public void EnableTacticalCamera()
        {
            if (IsTacticalCameraEnabled)
            {
                return;
            }
            
            _gameValueService.EnableFreeCamera();
            _gameValueService.DisableAutoCameraAngleAdjustment();
            _gameValueService.DisableCollisionZoomAdjustment();
            _gameValueService.DisableZoom();
            _gameValueService.DisableCenteringCameraBehindCharacter();
            IsTacticalCameraEnabled = true;
            OnTacticalCameraStateChanged(true);
        }

        public void DisableTacticalCamera()
        {
            if (!IsTacticalCameraEnabled)
            {
                return;
            }
            
            float currentZoom = _gameValueService.GetCameraZoomDistance();
            if (currentZoom <= _turnOnTacticalCameraThreshold)
            {
                _gameValueService.SetCameraZoomDistance(_turnOnTacticalCameraThreshold);
            }
            
            _gameValueService.SetCameraZoomDistance(0);
            _gameValueService.DisableFreeCamera();
            _gameValueService.EnableAutoCameraAngleAdjustment();
            _gameValueService.EnableCollisionZoomAdjustment();
            _gameValueService.EnableCenteringCameraBehindCharacter();
            _gameValueService.EnableZoom();
            IsTacticalCameraEnabled = false;
            OnTacticalCameraStateChanged(false);
        }

        private void OnTacticalCameraStateChanged(bool isTacticalCameraEnabled)
        {
            TacticalCameraStateChangedHandler eventHandler = TacticalCameraStateChanged;
            eventHandler?.Invoke(isTacticalCameraEnabled);
        }

        public void Dispose()
        {
        }
    }
}