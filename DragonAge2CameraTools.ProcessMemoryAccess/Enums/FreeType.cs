using System;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Enums
{
    [Flags]
    public enum FreeType
    {
        Decommit = 0x4000,
        Release = 0x8000,
    }
}