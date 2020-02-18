namespace DragonAge2CameraTools.ViewLogic.Settings.Data
{
    public class ApplicationSettings
    {
        public KeyBindingSettings KeyBindingSettings { get; set; }
        public bool UnlimitedZoomEnabled { get; set; }
        public bool ManualTacticalCameraEnabled { get; set; }
        public bool AutomaticTacticalCameraEnabled { get; set; }
        public float AutomaticCameraThresholdValue { get; set; }
        public float HorizontalCameraSpeed { get; set; }
        public float VerticalCameraSpeed { get; set; }
    }
}