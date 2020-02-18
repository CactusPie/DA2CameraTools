using System;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Interfaces
{
    public interface IActiveWindowService
    {
        string GetActiveWindowTitle();
        IntPtr GetActiveWindowHandle();
    }
}