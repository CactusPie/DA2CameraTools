using System;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces;

namespace DragonAge2CameraTools.ProcessMemoryAccess.Injection
{
    public class DllFunctionFinder : IDllFunctionFinder
    {
        public int GetLibraryFunctionOffset(string dllPath, string functionName)
        {
            IntPtr libraryHandle = ProcessFunctions.LoadLibrary(dllPath);
            
            int functionOffset = ProcessFunctions.GetProcAddress(libraryHandle, functionName).ToInt32() - libraryHandle.ToInt32();

            ProcessFunctions.FreeLibrary(libraryHandle);

            return functionOffset;
        }
    }
}