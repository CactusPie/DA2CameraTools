using System;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums;

namespace DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces
{
    public interface IKeyHandler : IDisposable
    {
        bool IsHandlerEnabled { get; set; }
        InputResult OnKeyDown(UserInputKey keyCode, ModifierKeys modifiers);
        InputResult OnKeyUp(UserInputKey keyCode);
        void StopCurrentlyRunningKeyFunction();
    }
}