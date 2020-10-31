using WindowsInput.Events;
using WindowsInput.Events.Sources;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling
{
    /// <summary>
    /// A service determining whether or not a specific key should be handled by the application.
    /// Prevents keys from being run when there character is in dialogue or in menu (inventory, party picker, etc.)
    /// </summary>
    public class HotkeyConditionService : IHotkeyConditionService
    {
        private readonly IActiveWindowService _activeWindowService;
        private readonly IGameValueService _gameValueService;

        public HotkeyConditionService(IActiveWindowService activeWindowService, IGameValueService gameValueService)
        {
            _activeWindowService = activeWindowService;
            _gameValueService = gameValueService;
        }
        
        public bool ShouldHandleHotkeyEvent(KeyboardEvent keyboardEvent)
        {
            return ShouldHandleHotkeyEvent();
        }
        
        public bool ShouldHandleHotkeyEvent(ButtonEvent buttonEvent)
        {
            return ShouldHandleHotkeyEvent();
        }
         
        public bool ShouldHandleHotkeyEvent(ButtonScroll buttonScroll)
        {
            return ShouldHandleHotkeyEvent();
        }

        private bool ShouldHandleHotkeyEvent()
        {
            return IsDragonAgeWindowActive();
        }
        
        private bool IsDragonAgeWindowActive()
        {
            string activeWindowTitle = _activeWindowService.GetActiveWindowTitle();
            bool shouldHandleHotkey = activeWindowTitle != null && activeWindowTitle.StartsWith("Dragon Age II");
            return shouldHandleHotkey;
        }
    }
}