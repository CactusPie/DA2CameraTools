using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories
{
    public class LoadingScreenMonitorFactory : ILoadingScreenMonitorFactory
    {
        public ILoadingScreenMonitor CreateLoadingScreenMonitor(IGameValueService gameValueService)
        {
            return new LoadingScreenMonitor(gameValueService);
        }
    }
}