namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces
{
    /// <summary>
    /// Handler for camera horizontal movement (forward, back, left, right). Should be enabled only
    /// when tactical camera is enabled, otherwise it will conflict with
    /// game built-in camera movement
    /// </summary>
    public interface ICameraMovementKeyHandler : IKeyHandler
    {
        float CameraMovementOffset { get; set; }
    }
}