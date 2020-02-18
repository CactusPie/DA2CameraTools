using System;
using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories.Interfaces
{
    public interface IGameValueServiceFactory
    {
        IGameValueService CreateGameValueService(IntPtr processHandle, Process process);
    }
}