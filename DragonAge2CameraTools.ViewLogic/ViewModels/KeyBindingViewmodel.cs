using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.ViewLogic.ViewModels.Base;

namespace DragonAge2CameraTools.ViewLogic.ViewModels
{
    public class KeyBindingViewmodel : ViewModelBase
    {
        private UserInputKey? _key;
        private bool _isInBinding;
        
        public UserInputKey? Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public bool IsInBinding
        {
            get => _isInBinding;
            set => SetProperty(ref _isInBinding, value);
        }

        public KeyBindingViewmodel(UserInputKey? key)
        {
            _key = key;
        }
    }
}