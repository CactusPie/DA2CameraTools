using System;

namespace DragonAge2CameraTools.GameManagement.Interfaces
{
    public interface ILoadingScreenMonitor : IDisposable
    {
        event EventHandler EnteredLoadingScreen;
        event EventHandler ExitLoadingScreen;
        void StartMonitoringLoadingScreen();
        void StopMonitoringLoadingScreen();
    }
}