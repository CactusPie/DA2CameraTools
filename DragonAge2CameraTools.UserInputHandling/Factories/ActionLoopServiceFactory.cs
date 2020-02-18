using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling.Factories
{
    public class ActionLoopServiceFactory : IActionLoopServiceFactory
    {
        public IActionLoopService CreateActionLoopService()
        {
            return new ActionLoopService();
        }
    }
}