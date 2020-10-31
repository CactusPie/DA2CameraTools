using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement
{
    public class AddressFinderWithCache : IAddressFinder
    {
        private int _xCameraAddress;
        private int _yCameraAddress;
        private int _zCameraAddress;
        private int _freeCameraCodeAddress;
        private int _unlimitedZoomCodeAddress;
        private int _collisionZoomAdjustmentCodeAddress;
        private int _autoCameraAngleAdjustmentCodeAddress;
        private int _horizontalCameraAngleAddress;
        private int _verticalCameraAngleAddress;
        private int _cameraZoomDistanceAddress;
        private int _loadingScreenFlagAddress;
        private int _menuScreenFlagAddress;
        private int _dialogueFlagAddress;
        private int _menuOrDialogueFlagAddress;
        private int _saveGameLoadedFlagAddress;
        private int _zoomStateCodeAddress;
        private int _centeringCameraBehindCharacterCodeAddress;
        private int _menuOrDialogueEnteredCodeAddress;
        private int _menuOrDialogueExitedCodeAddress;
        private int _loadingScreenEnteredCodeAddress;
        private int _loadingScreenExitedCodeAddress;

        public AddressFinderWithCache(IAddressFinder addressFinder)
        {
            RetrieveAddresses(addressFinder);
        }
        
        public int GetXCameraAddress()
        {
            return _xCameraAddress;
        }
        
        public int GetYCameraAddress()
        {
            return _yCameraAddress;
        }
        
        public int GetZCameraAddress()
        {
            return _zCameraAddress;
        }
        
        public int GetHorizontalCameraAngleAddress()
        {
            return _horizontalCameraAngleAddress;
        }
        
        public int GetVerticalCameraAngleAddress()
        {
            return _verticalCameraAngleAddress;
        }

        public int GetCameraZoomDistanceAddress()
        {
            return _cameraZoomDistanceAddress;
        }

        public int GetLoadingScreenFlagAddress()
        {
            return _loadingScreenFlagAddress;
        }

        public int GetSaveGameLoadedFlagAddress()
        {
            return _saveGameLoadedFlagAddress;
        }

        public int GetMenuScreenFlagAddress()
        {
            return _menuScreenFlagAddress;
        }

        public int GetDialogueFlagAddress()
        {
            return _dialogueFlagAddress;
        }

        public int GetMenuOrDialogueFlagAddress()
        {
            return _menuOrDialogueFlagAddress;
        }

        public int GetUnlimitedZoomCodeAddress()
        {
            return _unlimitedZoomCodeAddress;
        }
        
        public int GetFreeCameraCodeAddress()
        {
            return _freeCameraCodeAddress;
        }

        public int GetCollisionZoomAdjustmentCodeAddress()
        {
            return _collisionZoomAdjustmentCodeAddress;
        }
        
        public int GetAutoCameraAngleAdjustmentCodeAddress()
        {
            return _autoCameraAngleAdjustmentCodeAddress;
        }

        public int GetZoomStateCodeAddress()
        {
            return _zoomStateCodeAddress;
        }

        public int GetCenteringCameraBehindCharacterCodeAddress()
        {
            return _centeringCameraBehindCharacterCodeAddress;
        }

        public int GetMenuOrDialogueEnteredCodeAddress()
        {
            return _menuOrDialogueEnteredCodeAddress;
        }

        public int GetMenuOrDialogueExitedCodeAddress()
        {
            return _menuOrDialogueExitedCodeAddress;
        }

        public int GetLoadingScreenEnteredCodeAddress()
        {
            return _loadingScreenEnteredCodeAddress;
        }

        public int GetLoadingScreenExitedCodeAddress()
        {
            return _loadingScreenExitedCodeAddress;
        }

        private void RetrieveAddresses(IAddressFinder addressFinder)
        {
            _xCameraAddress = addressFinder.GetXCameraAddress();
            _yCameraAddress = addressFinder.GetYCameraAddress();
            _zCameraAddress = addressFinder.GetZCameraAddress();
            _unlimitedZoomCodeAddress = addressFinder.GetUnlimitedZoomCodeAddress();
            _horizontalCameraAngleAddress = addressFinder.GetHorizontalCameraAngleAddress();
            _verticalCameraAngleAddress = addressFinder.GetVerticalCameraAngleAddress();
            _freeCameraCodeAddress = addressFinder.GetFreeCameraCodeAddress();
            _collisionZoomAdjustmentCodeAddress = addressFinder.GetCollisionZoomAdjustmentCodeAddress();
            _autoCameraAngleAdjustmentCodeAddress = addressFinder.GetAutoCameraAngleAdjustmentCodeAddress();
            _cameraZoomDistanceAddress = addressFinder.GetCameraZoomDistanceAddress();
            _loadingScreenFlagAddress = addressFinder.GetLoadingScreenFlagAddress();
            _menuScreenFlagAddress = addressFinder.GetMenuScreenFlagAddress();
            _saveGameLoadedFlagAddress = addressFinder.GetSaveGameLoadedFlagAddress();
            _dialogueFlagAddress = addressFinder.GetDialogueFlagAddress();
            _menuOrDialogueFlagAddress = addressFinder.GetMenuOrDialogueFlagAddress();
            _zoomStateCodeAddress = addressFinder.GetZoomStateCodeAddress();
            _centeringCameraBehindCharacterCodeAddress = addressFinder.GetCenteringCameraBehindCharacterCodeAddress();
            _menuOrDialogueEnteredCodeAddress = addressFinder.GetMenuOrDialogueEnteredCodeAddress();
            _menuOrDialogueExitedCodeAddress = addressFinder.GetMenuOrDialogueExitedCodeAddress();
            _loadingScreenEnteredCodeAddress = addressFinder.GetLoadingScreenEnteredCodeAddress();
            _loadingScreenExitedCodeAddress = addressFinder.GetLoadingScreenExitedCodeAddress();
        }
    }
}