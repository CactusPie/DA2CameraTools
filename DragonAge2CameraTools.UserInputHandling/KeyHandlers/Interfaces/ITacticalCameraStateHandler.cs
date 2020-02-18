namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces
{
    public interface ITacticalCameraStateHandler : IKeyHandler
    {
        event TacticalCameraStateChangedHandler TacticalCameraStateChanged;
    }
}