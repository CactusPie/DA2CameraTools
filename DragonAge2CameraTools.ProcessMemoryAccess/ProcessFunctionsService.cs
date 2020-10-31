using System;
using System.Runtime.InteropServices;
using DragonAge2CameraTools.ProcessMemoryAccess.Enums;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.ProcessMemoryAccess
{
    public class ProcessFunctionsService : IProcessFunctionsService
    {
        public IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
        {
            return ProcessFunctions.OpenProcess(desiredAccess, false, processId);
        }
        
        public void CloseProcessHandle(IntPtr processHandle)
        {
            ProcessFunctions.CloseHandle(processHandle);
        }
        
        public void WriteMemoryFloat(IntPtr processHandle, int address, float value)
        {
            var bytes = BitConverter.GetBytes(value);

            ProcessFunctions.WriteProcessMemory(processHandle, 
                new IntPtr(address), 
                bytes,
                (uint)bytes.LongLength, 
                out _);
        }
        
        public void WriteMemoryBytes(IntPtr processHandle, int address, byte[] bytesToWrite)
        {
            ProcessFunctions.WriteProcessMemory(processHandle, 
                new IntPtr(address), 
                bytesToWrite,
                (uint)bytesToWrite.LongLength, 
                out _);
        }
        
        public float ReadMemoryFloat(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(float)]; 

            ProcessFunctions.ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);
            return BitConverter.ToSingle(buffer, 0);
        }
        
        public int ReadMemoryInt(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(int)]; 

            ProcessFunctions.ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);

            return BitConverter.ToInt32(buffer, 0);
        }
        
        public long ReadMemoryLong(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(int)]; 

            ProcessFunctions.ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);

            return BitConverter.ToInt32(buffer, 0);
        }
        
        public byte ReadMemoryByte(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(byte)]; 

            ProcessFunctions.ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);

            return buffer[0];
        }
        
        public byte[] ReadMemoryBytes(IntPtr processHandle, long address, int bytesToRead)
        {
            var buffer = new byte[bytesToRead]; 

            ProcessFunctions.ReadProcessMemory(processHandle, new IntPtr(address), buffer, bytesToRead, out _);
            return buffer;
        }

        public void FreeMemory(IntPtr processHandle, IntPtr addressToFree, uint bytesToFree)
        {
            ProcessFunctions.VirtualFreeEx(processHandle, addressToFree, bytesToFree, FreeType.Release);
        }
    }
}