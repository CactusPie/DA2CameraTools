using WindowsInput.Events;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling
{
    public class KeyMapper : IKeyMapper
    {
        public UserInputKey MapToUserInputKey(KeyCode keyCode)
        {
            // The values in both enums are the same for corresponding keys as of WindowsInput 6.1.1.0
            return (UserInputKey) keyCode;
        }
        
        public UserInputKey MapToUserInputKey(ButtonCode buttonCode)
        {
            // The values in both enums are the same for corresponding keys as of WindowsInput 6.1.1.0
            return (UserInputKey) buttonCode;
        }
        
        public UserInputKey MapToUserInputKey(int mouseScrollOffset)
        {
            return mouseScrollOffset > 0 ? UserInputKey.MouseScrollUp : UserInputKey.MouseScrollDown;
        }
    }
}