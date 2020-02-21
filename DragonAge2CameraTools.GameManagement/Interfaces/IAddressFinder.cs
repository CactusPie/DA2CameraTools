namespace DragonAge2CameraTools.GameManagement.Interfaces
{
    public interface IAddressFinder
    {
        int GetXCameraAddress();
        int GetYCameraAddress();
        int GetZCameraAddress();
        int GetHorizontalCameraAngleAddress();
        int GetVerticalCameraAngleAddress();
        int GetCameraZoomDistanceAddress();
        int GetLoadingScreenFlagAddress();
        int GetSaveGameLoadedFlagAddress();
        int GetMenuScreenFlagAddress();
        int GetDialogueFlagAddress();
        int GetMenuOrDialogueFlagAddress();
        int GetFreeCameraCodeAddress();
        int GetCollisionZoomAdjustmentCodeAddress();
        int GetUnlimitedZoomCodeAddress();
        int GetAutoCameraAngleAdjustmentCodeAddress();
        int GetZoomStateCodeAddress();
        int GetCenteringCameraBehindCharacterCodeAddress();
    }
}