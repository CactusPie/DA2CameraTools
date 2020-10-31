using System;
using System.Diagnostics;

namespace DragonAge2CameraTools.GameManagement.Interfaces
{
    public interface IGameEventService : IDisposable
    {
        event EventHandler EnteredLoadingScreen;
        event EventHandler ExitedLoadingScreen;
        event EventHandler EnteredMenuOrDialogue;
        event EventHandler ExitedMenuOrDialogue;
        void StartMonitoringGameEvents();
        void StopMonitoringGameEvents();
    }
}