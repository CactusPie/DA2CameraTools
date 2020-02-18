namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces
{
    /// <summary>
    /// Key handler for automatic tactical camera toggling - the camera will get toggled
    /// when te zoom reaches a certain value
    /// </summary>
    public interface IAutoTacticalCameraKeyHandler : ITacticalCameraStateHandler
    {
        void EnableTacticalCamera();
        void DisableTacticalCamera();
    }
}