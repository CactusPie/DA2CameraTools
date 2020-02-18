using WindowsInput.Events;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.UserInputHandling.Interfaces
{
    /// <summary>
    /// A mapper between keys from WindowsInput library and our custom keys
    /// </summary>
    public interface IKeyMapper
    {
        UserInputKey MapToUserInputKey(KeyCode keyCode);
        UserInputKey MapToUserInputKey(ButtonCode buttonCode);
        UserInputKey MapToUserInputKey(int mouseScrollOffset);
    }
}