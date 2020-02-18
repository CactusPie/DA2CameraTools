using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories
{
    public class HotkeyConditionServiceFactory : IHotkeyConditionServiceFactory
    {
        private readonly IActiveWindowService _activeWindowService;

        public HotkeyConditionServiceFactory(IActiveWindowService activeWindowService)
        {
            _activeWindowService = activeWindowService;
        }
        
        public IHotkeyConditionService CreateHotkeyConditionService(IGameValueService gameValueService)
        {
            return new HotkeyConditionService(_activeWindowService, gameValueService);
        }
    }
}