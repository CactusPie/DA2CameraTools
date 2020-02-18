using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories.Interfaces
{
    public interface IHotkeyConditionServiceFactory
    {
        IHotkeyConditionService CreateHotkeyConditionService(IGameValueService gameValueService);
    }
}