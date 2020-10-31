using System;
using System.Diagnostics;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Injection.Data
{
    public class InjectedDllData
    {
        public IntPtr DllAddress { get; }
        public ProcessModule DllProcessModule { get; }
        public uint DllSize { get; }

        public InjectedDllData(IntPtr dllAddress, ProcessModule dllProcessModule, uint dllSize)
        {
            DllAddress = dllAddress;
            DllSize = dllSize;
            DllProcessModule = dllProcessModule;
        }
    }
}