using System;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Enums
{
    [Flags]
    public enum AllocType
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
    }
}