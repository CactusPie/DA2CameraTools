using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories.Interfaces
{
    public interface IActionLoopServiceFactory
    {
        IActionLoopService CreateActionLoopService();
    }
}