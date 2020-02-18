using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories.Interfaces
{
    public interface IUserInputHandlerFactory
    {
        IUserInputHandler CreateUserInputHandler(IKeyHandler keyHandler,
            IGameValueService gameValueService);
    }
}