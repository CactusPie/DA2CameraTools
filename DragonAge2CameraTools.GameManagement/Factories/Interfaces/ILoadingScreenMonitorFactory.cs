using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories.Interfaces
{
    public interface ILoadingScreenMonitorFactory
    {
        ILoadingScreenMonitor CreateLoadingScreenMonitor(IGameValueService gameValueService);
    }
}