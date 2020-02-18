namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces
{
    /// <summary>
    /// Key handler for manual tactical camera toggling
    /// </summary>
    public interface IManualTacticalCameraKeyHandler : ITacticalCameraStateHandler
    {
        void EnableTacticalCamera();
        void DisableTacticalCamera();
    }
}