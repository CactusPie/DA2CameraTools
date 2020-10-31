using System;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Enums
{
    [Flags]
    public enum ThreadPermission
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
    }
}