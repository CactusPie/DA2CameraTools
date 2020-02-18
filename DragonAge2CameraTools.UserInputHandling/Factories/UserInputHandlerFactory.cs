using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories
{
    public class UserInputHandlerFactory : IUserInputHandlerFactory
    {
        private readonly IHotkeyConditionServiceFactory _hotkeyConditionServiceFactory;
        private readonly IKeyMapper _keyMapper;

        public UserInputHandlerFactory(IHotkeyConditionServiceFactory hotkeyConditionServiceFactory, IKeyMapper keyMapper)
        {
            _hotkeyConditionServiceFactory = hotkeyConditionServiceFactory;
            _keyMapper = keyMapper;
        }
        
        public IUserInputHandler CreateUserInputHandler(IKeyHandler keyHandler, IGameValueService gameValueService)
        {
            IHotkeyConditionService hotkeyConditionService = _hotkeyConditionServiceFactory.CreateHotkeyConditionService(gameValueService);
            return new UserInputHandler(hotkeyConditionService, _keyMapper, keyHandler);
        }
    }
}