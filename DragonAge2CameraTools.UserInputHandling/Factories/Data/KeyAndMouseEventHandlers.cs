using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories.Data
{
    public class KeyAndMouseEventHandlers 
    {
        public IKeyHandler CameraMovementHandler { get; set; }
        public IKeyHandler CameraHeightHandler { get; set; }
        public IAutoTacticalCameraKeyHandler AutoTacticalCameraHandler { get; set; }
        public IManualTacticalCameraKeyHandler ManualTacticalCameraHandler { get; set; }
    }
}