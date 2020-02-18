using System;
using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories.Interfaces
{
    public interface ICameraToolsFactory
    {
        IAddressFinder CreateAddressFinder(Process process);
        IGameValueService CreateGameValueService(IntPtr processHandle, Process process);
        ILoadingScreenMonitor CreateLoadingScreenMonitor(IGameValueService gameValueService);
    }
}