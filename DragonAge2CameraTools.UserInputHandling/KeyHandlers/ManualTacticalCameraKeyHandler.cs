using System.Collections.Generic;
using System.Linq;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers
{
    /// <inheritdoc/>>
    public class ManualTacticalCameraKeyHandler : IManualTacticalCameraKeyHandler
    {
        private bool _isTacticalCameraEnabled = false;
        private bool _isHandlerEnabled = true;
        
        public event TacticalCameraStateChangedHandler TacticalCameraStateChanged; 
        
        public bool IsHandlerEnabled
        {
            get => _isHandlerEnabled;
            set
            {
                if (!value && _isTacticalCameraEnabled)
                {
                    DisableTacticalCamera();
                }
                _isHandlerEnabled = value;
            }
        }

        private readonly IGameValueService _gameValueService;
        private readonly IEnumerable<UserInputKey> _tacticalCameraToggleKeys;

        public ManualTacticalCameraKeyHandler(IGameValueService gameValueService, IEnumerable<UserInputKey> tacticalCameraToggleKeys)
        {
            _gameValueService = gameValueService;
            _tacticalCameraToggleKeys = tacticalCameraToggleKeys;
        }
        
        public InputResult OnKeyDown(UserInputKey keyCode, ModifierKeys modifiers)
        {
            if (!_isHandlerEnabled)
            {
                return InputResult.Continue;
            }
            
            if (_tacticalCameraToggleKeys.Contains(keyCode))
            {
                if (!_isTacticalCameraEnabled && IsHandlerEnabled)
                {
                    EnableTacticalCamera();
                }
                else
                {
                    DisableTacticalCamera();
                }
            }

            return InputResult.Continue;
        }
        
        public InputResult OnKeyUp(UserInputKey keyCode)
        {
            return InputResult.Continue;
        }

        public void EnableTacticalCamera()
        {
            if (_isTacticalCameraEnabled)
            {
                return;
            }
            
            _gameValueService.EnableFreeCamera();
            _gameValueService.DisableAutoCameraAngleAdjustment();
            _gameValueService.DisableCollisionZoomAdjustment();
            _gameValueService.DisableZoom();
            _gameValueService.SetCameraZoomDistance(1.66f);
            _gameValueService.SetZCameraPosition(_gameValueService.GetZCameraPosition() + 2);
            _isTacticalCameraEnabled = true;
            OnTacticalCameraStateChanged(true);
        }

        public void DisableTacticalCamera()
        {
            if (!_isTacticalCameraEnabled)
            {
                return;
            }
            
            _gameValueService.SetCameraZoomDistance(0);
            _gameValueService.DisableFreeCamera();
            _gameValueService.EnableAutoCameraAngleAdjustment();
            _gameValueService.EnableCollisionZoomAdjustment();
            _gameValueService.EnableZoom();
            _isTacticalCameraEnabled = false;
            OnTacticalCameraStateChanged(false);
        }
        
        public void StopCurrentlyRunningKeyFunction()
        {
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