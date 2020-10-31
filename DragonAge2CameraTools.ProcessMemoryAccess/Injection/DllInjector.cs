using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using DragonAge2CameraTools.ProcessMemoryAccess.Enums;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Data;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Injection
{
    public class DllInjector : IDllInjector
    {
        public InjectedDllData InjectDll(string dllPath, Process targetProcess)
        {
            IntPtr processHandle = ProcessFunctions.OpenProcess
            (
                ProcessAccessFlags.CreateThread | ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VMOperation | 
                    ProcessAccessFlags.VMWrite | ProcessAccessFlags.VMRead, 
                false, 
                targetProcess.Id
            );

            IntPtr loadLibraryAddr = ProcessFunctions.GetProcAddress(ProcessFunctions.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            // Size of the dll path + size of a pointer to store it
            var dllSize = (uint) ((dllPath.Length + 1) * Marshal.SizeOf(typeof(char)));
            
            IntPtr allocatedAddress = ProcessFunctions.VirtualAllocEx
            (
                processHandle, 
                IntPtr.Zero, 
                dllSize, 
                AllocType.Commit | AllocType.Reserve, 
                ThreadPermission.ExecuteReadWrite
            );

            ProcessFunctions.WriteProcessMemory
            (
                processHandle, 
                allocatedAddress, 
                Encoding.Default.GetBytes(dllPath), 
                (uint) ((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), 
                out _
            );

            IntPtr threadHandle = ProcessFunctions.CreateRemoteThread
            (
                processHandle, 
                IntPtr.Zero, 
                0, 
                loadLibraryAddr, 
                allocatedAddress, 
                0, 
                IntPtr.Zero
            );
            
            ProcessFunctions.WaitForSingleObject(threadHandle, uint.MaxValue);
            ProcessFunctions.CloseHandle(threadHandle);

            // Creating another handle is necessary in order to see the newly added module
            using (var processForModuleRetrieval = Process.GetProcessById(targetProcess.Id))
            {
                ProcessModule dllModule = GetDllModule(processForModuleRetrieval, dllPath);
                return new InjectedDllData(allocatedAddress, dllModule, dllSize);
            }
        }

        public void UnloadDll(InjectedDllData dllData)
        {
            ProcessFunctions.FreeLibrary(dllData.DllProcessModule.BaseAddress);
        }

        private static ProcessModule GetDllModule(Process process, string dllPath)
        {
            ProcessModule dllModule = null;
            string moduleFileName = Path.GetFileNameWithoutExtension(dllPath);
            
            foreach (ProcessModule module in process.Modules)
            {
                if (module.ModuleName.StartsWith(moduleFileName))
                {
                    dllModule = module;
                    break;
                }
            }

            return dllModule;
        }
    }
}