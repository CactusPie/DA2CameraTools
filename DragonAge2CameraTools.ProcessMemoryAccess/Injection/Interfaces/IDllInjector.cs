using System;
using System.Diagnostics;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Data;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces
{
    public interface IDllInjector
    {
        InjectedDllData InjectDll(string dllPath, Process targetProcess);
        void UnloadDll(InjectedDllData dllData);
    }
}