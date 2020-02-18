using System;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Interfaces
{
    public interface IProcessFunctionsService
    {
        float ReadMemoryFloat(IntPtr processHandle, long address);
        int ReadMemoryInt(IntPtr processHandle, long address);
        long ReadMemoryLong(IntPtr processHandle, long address);
        byte ReadMemoryByte(IntPtr processHandle, long address);
        byte[] ReadMemoryBytes(IntPtr processHandle, long address, int bytesToRead);
        IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId);
        void CloseProcessHandle(IntPtr processHandle);
        void WriteMemoryFloat(IntPtr processHandle, int address, float value);
        void WriteMemoryBytes(IntPtr processHandle, int address, byte[] bytesToWrite);
    }
}