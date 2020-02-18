using DragonAge2CameraTools.ViewLogic.ViewModels;

namespace DragonAge2CameraTools.ViewLogic.Data
{
    public class ViewModelKeyBindings
    {
        public FullyObservableCollection<KeyBindingViewmodel> TacticalCameraToggleKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> CameraForwardKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> CameraBackKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> CameraLeftKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> CameraRightKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> CameraDownKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> CameraUpKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> ZoomInKeys { get; set; }
        public FullyObservableCollection<KeyBindingViewmodel> ZoomOutKeys { get; set; }
    }
}