using System;
using System.Collections.Generic;
using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Data;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Enums;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Data;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.GameManagement.FunctionHooking
{
    public class GameFunctionHookService : IDisposable, IGameFunctionHookService
    {
        private readonly IDllInjector _dllInjector;
        private readonly IProcessFunctionsService _processFunctionsService;
        private readonly IDllFunctionFinder _dllFunctionFinder;
        private readonly IAddressFinder _addressFinder;
        private readonly string _hookDllPath;
        private readonly Process _gameProcess;
        private InjectedDllData _injectedLibraryData;

        public GameFunctionHookService
        (
                IDllInjector dllInjector, 
                IProcessFunctionsService processFunctionsService, 
                IDllFunctionFinder dllFunctionFinder, 
                IAddressFinder addressFinder,
                string hookDllPath, 
                Process gameProcess
        )
        {
            _dllInjector = dllInjector;
            _processFunctionsService = processFunctionsService;
            _dllFunctionFinder = dllFunctionFinder;
            _addressFinder = addressFinder;
            _hookDllPath = hookDllPath;
            _gameProcess = gameProcess;
            InjectDll(hookDllPath, gameProcess);
        }

        public void EnableAllHooks()
        {
            EnableMenuOrDialogueEnteredHook();
            EnableMenuOrDialogueExitedHook();
            EnableLoadingScreenEnteredHook();
            EnableLoadingScreenExitedHook();
        }
        
        public void EnableMenuOrDialogueEnteredHook()
        {
            int menuOrDialogueEnteredCodeAddress = _addressFinder.GetMenuOrDialogueEnteredCodeAddress();
            InjectHook("MenuOrDialogueEnteredHook",  menuOrDialogueEnteredCodeAddress, HookType.NearJump, 7);
        }
        
        public void EnableMenuOrDialogueExitedHook()
        {
            int menuOrDialogueExitedCodeAddress = _addressFinder.GetMenuOrDialogueExitedCodeAddress();
            InjectHook("MenuOrDialogueExitedHook",  menuOrDialogueExitedCodeAddress, HookType.NearJump, 7);
        }
        
        public void EnableLoadingScreenEnteredHook()
        {
            int loadingScreenEnteredCodeAddress = _addressFinder.GetLoadingScreenEnteredCodeAddress();
            InjectHook("LoadingScreenEnteredHook",  loadingScreenEnteredCodeAddress, HookType.Call, 7);
        }
        
        public void EnableLoadingScreenExitedHook()
        {
            int loadingScreenEnteredCodeAddress = _addressFinder.GetLoadingScreenExitedCodeAddress();
            InjectHook("LoadingScreenExitedHook",  loadingScreenEnteredCodeAddress, HookType.Call, 7);
        }
        
        public void DisableAllHooks()
        {
            DisableMenuOrDialogueEnteredHook();
            DisableMenuOrDialogueExitedHook();
            DisableLoadingScreenEnteredHook();
            DisableLoadingScreenExitedHook();
        }

        public void DisableMenuOrDialogueEnteredHook()
        {
            // Writes: mov byte ptr [00D54492],00
            var bytesToWrite = new byte[] {0xC6, 0x05, 0x92, 0x44, 0xD5, 0x00, 0x00};
            int menuOrDialogueEnteredCodeAddress = _addressFinder.GetMenuOrDialogueEnteredCodeAddress();
            _processFunctionsService.WriteMemoryBytes(_gameProcess.Handle, menuOrDialogueEnteredCodeAddress, bytesToWrite);
        }
        
        public void DisableMenuOrDialogueExitedHook()
        {
            // Writes: mov byte ptr [00D54492],01
            var bytesToWrite = new byte[] {0xC6, 0x05, 0x92, 0x44, 0xD5, 0x00, 0x01};
            int menuOrDialogueExitedCodeAddress = _addressFinder.GetMenuOrDialogueExitedCodeAddress();
            _processFunctionsService.WriteMemoryBytes(_gameProcess.Handle, menuOrDialogueExitedCodeAddress, bytesToWrite);
        }
        
        public void DisableLoadingScreenEnteredHook()
        {
            // Writes: mov byte ptr [esi+00000110],01
            var bytesToWrite = new byte[] {0xC6, 0x86, 0x10, 0x01, 0x00, 0x00, 0x01};
            int loadingScreenEnteredCodeAddress = _addressFinder.GetLoadingScreenEnteredCodeAddress();
            _processFunctionsService.WriteMemoryBytes(_gameProcess.Handle, loadingScreenEnteredCodeAddress, bytesToWrite);
        }
        
        public void DisableLoadingScreenExitedHook()
        {
            // Writes: mov byte ptr [esi+00000110],00
            var bytesToWrite = new byte[] {0xC6, 0x86, 0x10, 0x01, 0x00, 0x00, 0x00};
            int loadingScreenExitedCodeAddress = _addressFinder.GetLoadingScreenExitedCodeAddress();
            _processFunctionsService.WriteMemoryBytes(_gameProcess.Handle, loadingScreenExitedCodeAddress, bytesToWrite);
        }

        
        /// <summary>
        /// Injects a code that redirects the flow of the program to our custom function. That function executes
        /// the original game code along with our custom code
        /// </summary>
        /// <param name="functionName">Function name in our custom dll</param>
        /// <param name="originalCodeAddress">Address of the original code where our call/jump will be placed</param>
        /// <param name="hookType">Specifies whether we should use call or jump to hook</param>
        /// <param name="originalInstructionSize">Size of the original function, if greater than redirection code it will be padded with NOPs</param>
        private void InjectHook(string functionName, int originalCodeAddress, HookType hookType, int originalInstructionSize)
        {
            int libraryFunctionOffset = _dllFunctionFinder.GetLibraryFunctionOffset(_hookDllPath, functionName);
            int absoluteFunctionAddress = _injectedLibraryData.DllProcessModule.BaseAddress.ToInt32() + libraryFunctionOffset;
            
            // Calculate the address for jump/call to our custom function. It is relative to the original code address
            const int hookRedirectionByteCount = 5;
            int destinationAddressOffset = absoluteFunctionAddress - originalCodeAddress - hookRedirectionByteCount;
            var destinationAddressBytes = BitConverter.GetBytes(destinationAddressOffset);

            // We redirect the code to our function using either call or jmp
            byte instructionOpcode = hookType == HookType.Call ? AssemblyInstruction.Call : AssemblyInstruction.NearJump;
            
            // hookRedirectionBytes will contain bytes that will be written to the remote process
            var hookRedirectionBytes = new List<byte>(originalInstructionSize) { instructionOpcode };
            hookRedirectionBytes.AddRange(destinationAddressBytes);
            
            // Pad the remaining bytes with NOPs, so the hook call has the same size as the original call
            for (int i = hookRedirectionBytes.Count; i < originalInstructionSize; i++)
            {
                hookRedirectionBytes.Add(AssemblyInstruction.Nop);
            }
            
            _processFunctionsService.WriteMemoryBytes(_gameProcess.Handle, originalCodeAddress, hookRedirectionBytes.ToArray());
        }
        
        private void InjectDll(string hookDllPath, Process gameProcess)
        {
            _injectedLibraryData = _dllInjector.InjectDll(hookDllPath, gameProcess);
        }

        public void Dispose()
        {
            _dllInjector.UnloadDll(_injectedLibraryData);
            _processFunctionsService.FreeMemory(_gameProcess.Handle, _injectedLibraryData.DllAddress, _injectedLibraryData.DllSize);
        }
    }
}