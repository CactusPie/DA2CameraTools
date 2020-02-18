namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Data
{
    public class TacticalCameraSettings
    {
        public TacticalCameraKeyBindings TacticalCameraKeyBindings { get; set; }
        public bool UnlimitedZoomEnabled { get; set; }
        public bool ManualTacticalCameraEnabled { get; set; }
        public bool AutomaticTacticalCameraEnabled { get; set; }
        public float AutomaticTacticalCameraThreshold { get; set; }
        public float HorizontalCameraSpeed { get; set; }
        public float VerticalCameraSpeed { get; set; }
    }
}