using WindowsInput.Events;
using WindowsInput.Events.Sources;

namespace DragonAge2CameraTools.UserInputHandling.Interfaces
{
    public interface IHotkeyConditionService
    {
        bool ShouldHandleHotkeyEvent(KeyboardEvent keyboardEvent);
        bool ShouldHandleHotkeyEvent(ButtonEvent buttonEvent);
        bool ShouldHandleHotkeyEvent(ButtonScroll buttonScroll);
    }
}