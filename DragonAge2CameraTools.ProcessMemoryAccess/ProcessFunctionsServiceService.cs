using System;
using System.Runtime.InteropServices;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.ProcessMemoryAccess
{
    public class ProcessFunctionsServiceService : IProcessFunctionsService
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(IntPtr hProcess);
        
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
        
        public IntPtr OpenProcess(ProcessAccessFlags desiredAccess, int processId)
        {
            return OpenProcess(desiredAccess, false, processId);
        }
        
        public void CloseProcessHandle(IntPtr processHandle)
        {
            CloseHandle(processHandle);
        }
        
        public void WriteMemoryFloat(IntPtr processHandle, int address, float value)
        {
            var bytes = BitConverter.GetBytes(value);

            WriteProcessMemory(processHandle, 
                new IntPtr(address), 
                bytes,
                (uint)bytes.LongLength, 
                out _);
        }
        
        public void WriteMemoryBytes(IntPtr processHandle, int address, byte[] bytesToWrite)
        {
            WriteProcessMemory(processHandle, 
                new IntPtr(address), 
                bytesToWrite,
                (uint)bytesToWrite.LongLength, 
                out _);
        }
        
        public float ReadMemoryFloat(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(float)]; 

            ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);
            return BitConverter.ToSingle(buffer, 0);
        }
        
        public int ReadMemoryInt(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(int)]; 

            ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);

            return BitConverter.ToInt32(buffer, 0);
        }
        
        public long ReadMemoryLong(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(int)]; 

            ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);

            return BitConverter.ToInt32(buffer, 0);
        }
        
        public byte ReadMemoryByte(IntPtr processHandle, long address)
        {
            var buffer = new byte[sizeof(byte)]; 

            ReadProcessMemory(processHandle, new IntPtr(address), buffer, buffer.Length, out _);

            return buffer[0];
        }
        
        public byte[] ReadMemoryBytes(IntPtr processHandle, long address, int bytesToRead)
        {
            var buffer = new byte[bytesToRead]; 

            ReadProcessMemory(processHandle, new IntPtr(address), buffer, bytesToRead, out _);
            return buffer;
        }
    }
}