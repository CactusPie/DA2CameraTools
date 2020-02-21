using System;
using DragonAge2CameraTools.GameManagement.Data;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.GameManagement
{
    public class GameValueService : IGameValueService
    {
        private readonly IAddressFinder _addressFinder;
        private readonly IProcessFunctionsService _processFunctionsService;
        private readonly IntPtr _processHandle;

        public GameValueService(IAddressFinder addressFinder, IProcessFunctionsService processFunctionsService, IntPtr processHandle)
        {
            _addressFinder = addressFinder;
            _processFunctionsService = processFunctionsService;
            _processHandle = processHandle;
        }

        // The X, Y and Z of camera position is a bit tricky. It's not the position of the camera itself
        // but rather the point around which the camera rotates
        /// <inheritdoc/>>
        public float GetXCameraPosition()
        {
            int xCameraAddress = _addressFinder.GetXCameraAddress();
            float xCameraPosition = _processFunctionsService.ReadMemoryFloat(_processHandle, xCameraAddress);
            return xCameraPosition;
        }
        
        /// <inheritdoc/>>
        public float GetYCameraPosition()
        {
            int yCameraAddress = _addressFinder.GetYCameraAddress();
            float yCameraPosition = _processFunctionsService.ReadMemoryFloat(_processHandle, yCameraAddress);
            return yCameraPosition;
        }
        
        /// <inheritdoc/>>
        public float GetZCameraPosition()
        {
            int zCameraAddress = _addressFinder.GetZCameraAddress();
            float zCameraPosition = _processFunctionsService.ReadMemoryFloat(_processHandle, zCameraAddress);
            return zCameraPosition;
        }
        
        /// <inheritdoc/>>
        public XYCameraPosition GetXYCameraPosition()
        {
            const int bytesToRead = sizeof(float) * 2;
            int xCameraAddress = _addressFinder.GetXCameraAddress();
            var bytes = _processFunctionsService.ReadMemoryBytes(_processHandle, xCameraAddress, bytesToRead);
            
            var result = new XYCameraPosition(
                BitConverter.ToSingle(bytes, 0), 
                BitConverter.ToSingle(bytes, sizeof(float))
            );

            return result;
        }
        
        /// <inheritdoc/>>
        public XYZCameraPosition GetXYZCameraPosition()
        {
            const int bytesToRead = sizeof(float) * 3;
            int xCameraAddress = _addressFinder.GetXCameraAddress();
            var bytes = _processFunctionsService.ReadMemoryBytes(_processHandle, xCameraAddress, bytesToRead);
            
            var result = new XYZCameraPosition(
                BitConverter.ToSingle(bytes, 0), 
                BitConverter.ToSingle(bytes, sizeof(float)),
                BitConverter.ToSingle(bytes, sizeof(float) * 2)
            );

            return result;
        }
        
        /// <inheritdoc/>>
        public float GetHorizontalCameraAngle()
        {
            int horizontalCameraAngleAddress = _addressFinder.GetHorizontalCameraAngleAddress();
            float horizontalCameraAngle = _processFunctionsService.ReadMemoryFloat(_processHandle, horizontalCameraAngleAddress);
            return horizontalCameraAngle;
        }
        
        /// <inheritdoc/>>
        public float GetVerticalCameraAngle()
        {
            int verticalCameraAddress = _addressFinder.GetVerticalCameraAngleAddress();
            float verticalCameraAngle = _processFunctionsService.ReadMemoryFloat(_processHandle, verticalCameraAddress);
            return verticalCameraAngle;
        }
        
        /// <inheritdoc/>>
        public float GetCameraZoomDistance()
        {
            int cameraZoomDistanceAddress = _addressFinder.GetCameraZoomDistanceAddress();
            float cameraZoomDistance = _processFunctionsService.ReadMemoryFloat(_processHandle, cameraZoomDistanceAddress);
            return cameraZoomDistance;
        }

        /// <inheritdoc/>>
        public bool IsGameInLoadingScreen()
        {
            int loadingScreenFlagAddress = _addressFinder.GetLoadingScreenFlagAddress();
            int isInLoadingScreenFlagValue = _processFunctionsService.ReadMemoryInt(_processHandle, loadingScreenFlagAddress);
            bool isLoadingScreen = (isInLoadingScreenFlagValue & 1) == 1;
            return isLoadingScreen;
        }
        
        /// <inheritdoc/>>
        public bool IsGameInMenuScreen()
        {
            int menuScreenFlagAddress = _addressFinder.GetMenuScreenFlagAddress();
            int isMenuScreenFlagValue = _processFunctionsService.ReadMemoryInt(_processHandle, menuScreenFlagAddress);
            bool isInMenuScreen = (isMenuScreenFlagValue & 1) == 1;
            return isInMenuScreen;
        }
        
        /// <inheritdoc/>>
        public bool IsGameInDialogue()
        {
            int dialogueFlagAddress = _addressFinder.GetDialogueFlagAddress();
            int dialogueFlagValue = _processFunctionsService.ReadMemoryInt(_processHandle, dialogueFlagAddress);
            bool isInDialogue = (dialogueFlagValue & 1) == 1;
            return isInDialogue;
        }
        
        /// <inheritdoc/>>
        public bool IsGameInMenuOrDialogue()
        {
            int menuOrDialogueFlagAddress = _addressFinder.GetMenuOrDialogueFlagAddress();
            int menuOrDialogueFlagValue = _processFunctionsService.ReadMemoryByte(_processHandle, menuOrDialogueFlagAddress);
            // The flag here is inverted - it's 0 when user is in menu or dialogue and 1 otherwise
            bool isInMenuOrDialogue = (menuOrDialogueFlagValue & 1) == 0;
            return isInMenuOrDialogue;
        }
        
        /// <inheritdoc/>>
        public void SetCameraZoomDistance(float newZoomDistance)
        {
            int cameraZoomDistanceAddress = _addressFinder.GetCameraZoomDistanceAddress();
            _processFunctionsService.WriteMemoryFloat(_processHandle, cameraZoomDistanceAddress, newZoomDistance);
        }

        /// <inheritdoc/>>
        public void SetCameraPosition(float x, float y)
        {
            var xBytes = BitConverter.GetBytes(x);
            var yBytes = BitConverter.GetBytes(y);
            var combinedBytes = new byte[sizeof(float) * 2];

            xBytes.CopyTo(combinedBytes, 0);
            yBytes.CopyTo(combinedBytes, sizeof(float));

            int xCameraAddress = _addressFinder.GetXCameraAddress();
            _processFunctionsService.WriteMemoryBytes(_processHandle, xCameraAddress, combinedBytes);
        }
        
        /// <inheritdoc/>>
        public void SetCameraPosition(float x, float y, float z)
        {
            var xBytes = BitConverter.GetBytes(x);
            var yBytes = BitConverter.GetBytes(y);
            var zBytes = BitConverter.GetBytes(z);
            var combinedBytes = new byte[sizeof(float) * 3];

            xBytes.CopyTo(combinedBytes, 0);
            yBytes.CopyTo(combinedBytes, sizeof(float));
            zBytes.CopyTo(combinedBytes, sizeof(float) * 2);

            int xCameraAddress = _addressFinder.GetXCameraAddress();
            _processFunctionsService.WriteMemoryBytes(_processHandle, xCameraAddress, combinedBytes);
        }
        
        /// <inheritdoc/>>
        public void SetXCameraPosition(float xCameraPosition)
        {
            int xCameraAddress = _addressFinder.GetXCameraAddress();
            _processFunctionsService.WriteMemoryFloat(_processHandle, xCameraAddress, xCameraPosition);
        }
        
        /// <inheritdoc/>>
        public void SetYCameraPosition(float yCameraPosition)
        {
            int yCameraAddress = _addressFinder.GetYCameraAddress();
            _processFunctionsService.WriteMemoryFloat(_processHandle, yCameraAddress, yCameraPosition);
        }
        
        /// <inheritdoc/>>
        public void SetZCameraPosition(float zCameraPosition)
        {
            int zCameraAddress = _addressFinder.GetZCameraAddress();
            _processFunctionsService.WriteMemoryFloat(_processHandle, zCameraAddress, zCameraPosition);
        }
        
        /// <inheritdoc/>>
        public void EnableUnlimitedZoom()
        {
            int unlimitedZoomCodeAddress = _addressFinder.GetUnlimitedZoomCodeAddress();
            // Writes: 2x nop
            _processFunctionsService.WriteMemoryBytes(_processHandle, unlimitedZoomCodeAddress, new byte[]{ 0x90, 0x90 });
        }
        
        /// <inheritdoc/>>
        public void DisableUnlimitedZoom()
        {
            int unlimitedZoomCodeAddress = _addressFinder.GetUnlimitedZoomCodeAddress();
            // Writes: jp DragonAge2.exe+25CCA
            _processFunctionsService.WriteMemoryBytes(_processHandle, unlimitedZoomCodeAddress, new byte[]{ 0x7A, 0x12 });
        }
        
        /// <inheritdoc/>>
        public void EnableFreeCamera()
        {
            int freeCameraCodeAddress = _addressFinder.GetFreeCameraCodeAddress();
            // Writes: 7x nop
            _processFunctionsService.WriteMemoryBytes(_processHandle, freeCameraCodeAddress, new byte[]{ 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });
        }
        
        /// <inheritdoc/>>
        public void DisableFreeCamera()
        {
            int freeCameraCodeAddress = _addressFinder.GetFreeCameraCodeAddress();
            // Writes: movaps [esi+000000E0],xmm0
            _processFunctionsService.WriteMemoryBytes(_processHandle, freeCameraCodeAddress, new byte[]{ 0x0F, 0x29, 0x86, 0xE0, 0, 0, 0 });
        }
        
        /// <inheritdoc/>>
        public void EnableCollisionZoomAdjustment()
        {
            int collisionZoomAdjustmentCodeAddress = _addressFinder.GetCollisionZoomAdjustmentCodeAddress();
            // Writes: fld dword ptr [esi+34]
            _processFunctionsService.WriteMemoryBytes(_processHandle, collisionZoomAdjustmentCodeAddress, new byte[] { 0xD9, 0x46, 0x34 });
        }
        
        /// <inheritdoc/>>
        public void DisableCollisionZoomAdjustment()
        {
            int collisionZoomAdjustmentCodeAddress = _addressFinder.GetCollisionZoomAdjustmentCodeAddress();
            // Writes:
            // fldz
            // nop
            _processFunctionsService.WriteMemoryBytes(_processHandle, collisionZoomAdjustmentCodeAddress, new byte[] { 0xD9, 0xEE, 0x90 });
        }
        
        /// <inheritdoc/>>
        public void EnableAutoCameraAngleAdjustment()
        {
            int autoCameraAdjustmentCodeAddress = _addressFinder.GetAutoCameraAngleAdjustmentCodeAddress();
            // Writes: jmp 00448361
            _processFunctionsService.WriteMemoryBytes(_processHandle, autoCameraAdjustmentCodeAddress, new byte[] { 0x7A, 0x14 });
        }
        
        /// <inheritdoc/>>
        public void DisableAutoCameraAngleAdjustment()
        {
            int autoCameraAngleAdjustmentCodeAddress = _addressFinder.GetAutoCameraAngleAdjustmentCodeAddress();
            // Writes: jp 00448361
            _processFunctionsService.WriteMemoryBytes(_processHandle, autoCameraAngleAdjustmentCodeAddress, new byte[] { 0xEB, 0x14 });
        }
        
        /// <inheritdoc/>>
        public void EnableZoom()
        {
            int zoomCodeAddress = _addressFinder.GetZoomStateCodeAddress();
            // Writes: fld st(0)
            _processFunctionsService.WriteMemoryBytes(_processHandle, zoomCodeAddress, new byte[] { 0xD9, 0xC0 });
        }
        
        /// <inheritdoc/>>
        public void DisableZoom()
        {
            int zoomCodeAddress = _addressFinder.GetZoomStateCodeAddress();
            // Writes: fldz
            _processFunctionsService.WriteMemoryBytes(_processHandle, zoomCodeAddress, new byte[] { 0xD9, 0xEE });
        }

        /// <inheritdoc/>>
        public void EnableCenteringCameraBehindCharacter()
        {
            
            int centeringCameraBehindCharacterCodeAddress = _addressFinder.GetCenteringCameraBehindCharacterCodeAddress();
            // Writes: je 0x004842EE
            _processFunctionsService.WriteMemoryBytes
            (
                _processHandle, 
                centeringCameraBehindCharacterCodeAddress, 
                new byte[] { 0x0F, 0x84, 0x89, 0x01, 0x00, 0x00 }
            );
        }
        
        /// <inheritdoc/>>
        public void DisableCenteringCameraBehindCharacter()
        {
            
            int centeringCameraBehindCharacterCodeAddress = _addressFinder.GetCenteringCameraBehindCharacterCodeAddress();
            // Writes: jmp 0x004842EE
            _processFunctionsService.WriteMemoryBytes
            (
                _processHandle, 
                centeringCameraBehindCharacterCodeAddress, 
                new byte[] { 0xE9, 0x8A, 0x01, 0x00, 0x00, 0x90 }
            );
        }
    }
}