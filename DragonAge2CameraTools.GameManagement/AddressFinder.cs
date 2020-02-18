using System;
using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.GameManagement
{
    /// <summary>
    /// Finds addresses in memory that contain values representing the current state of the game
    /// or specific parts of the game code. Look into GameValueService for more explanation
    /// </summary>
    public class AddressFinder : IAddressFinder
    {
        private readonly IProcessFunctionsService _processFunctionsService;
        private readonly IntPtr _processHandle;
        private readonly int _baseAddress;

        public AddressFinder(IProcessFunctionsService processFunctionsService, Process process)
        {
            _processFunctionsService = processFunctionsService;
            _processHandle = process.Handle;
            _baseAddress = GetBaseAddress(process);
        }
        
        public int GetXCameraAddress()
        {
            int address = GetAddressFromOffsets(0xD54494, new[] {0x144, 0x88, 0x10, 0x120, 0, 0x1C, 0xE0});
            
            return address;
        }
        
        public int GetYCameraAddress()
        {
            int address = GetAddressFromOffsets(0xD54494, new[] {0x144, 0x88, 0x10, 0x120, 0, 0x1C, 0xE4});

            return address;
        }
        
        public int GetZCameraAddress()
        {
            int address = GetAddressFromOffsets(0xD54494, new[] {0x144, 0x88, 0x10, 0x120, 0, 0x1C, 0xE8});
            
            return address;
        }
        
        public int GetHorizontalCameraAngleAddress()
        {
            int address = GetAddressFromOffsets(0xD54494, new[] {0x144, 0x9C, 0x80, 0x1C});
            
            return address;
        }
        
        public int GetVerticalCameraAngleAddress()
        {
            int address = GetAddressFromOffsets(0xD54494, new[] {0x144, 0x88, 0x10, 0x120, 0, 0x38});
            
            return address;
        }
        
        public int GetCameraZoomDistanceAddress()
        {
            int address = GetAddressFromOffsets(0xD54494, new[] {0x144, 0x88, 0x74});
            
            return address;
        }
        
        public int GetLoadingScreenFlagAddress()
        {
            int address = GetAddressFromOffsets(0xD5869C, new[] {0x1D0});
            return address;
        }
        
        public int GetSaveGameLoadedFlagAddress()
        {
            return 0xD544A8;
        }

        public int GetMenuScreenFlagAddress()
        {
            int address = GetAddressFromOffsets(0xD54494, new[] {0x154, 0, 0x1C});
            return address;
        }
        
        public int GetDialogueFlagAddress()
        {
            int address = GetAddressFromOffsets(0xD5869C, new[] {0x104});
            return address;
        }
        
        public int GetMenuOrDialogueFlagAddress()
        {
            return 0xD54492;
        }
        
        public int GetUnlimitedZoomCodeAddress()
        {
            return _baseAddress + 0x25CB6;
        }
        
        public int GetFreeCameraCodeAddress()
        {
            return _baseAddress + 0x697C2;
        }

        public int GetCollisionZoomAdjustmentCodeAddress()
        {
            return _baseAddress + 0xA5903;
        }
        
        public int GetAutoCameraAngleAdjustmentCodeAddress()
        {
            return _baseAddress + 0x4834B;
        }
        
        public int GetZoomStateCodeAddress()
        {
            return _baseAddress + 0x84339;
        }
        
        /// <summary>
        /// Since the addresses of values in memory are usually not constant we need to retrieve them
        /// every time based on pointers. This allows us to quickly follow multiple pointers
        /// in order to find a specific address
        /// </summary>
        /// <param name="basePointerAddress">Constant address of the first pointer</param>
        /// <param name="offsets">Offsets for each pointer</param>
        /// <returns>Address retrieved by following the pointers</returns>
        private int GetAddressFromOffsets(int basePointerAddress, int[] offsets)
        {
            int address = basePointerAddress;
            foreach (int offset in offsets)
            {
                address = _processFunctionsService.ReadMemoryInt(_processHandle,  address) + offset;
            }

            return address;
        }

        /// <summary>
        /// Returns the memory offset of DragonAge2.exe module in the process
        /// </summary>
        /// <param name="process">Dragon Age 2 process</param>
        /// <returns>Base address of DragonAge2.exe module</returns>
        /// <exception cref="EntryPointNotFoundException">Occurs when DragonAge2.exe module is not found</exception>
        private static int GetBaseAddress(Process process)
        {
            ProcessModule mainModule = null;
            foreach (ProcessModule module in process.Modules)
            {
                if (module.ModuleName.Equals("DragonAge2.exe", StringComparison.InvariantCultureIgnoreCase))
                {
                    mainModule = module;
                    break;
                }
            }

            if (mainModule != null)
            {
                return mainModule.BaseAddress.ToInt32();
            }

            throw new EntryPointNotFoundException("Unable to find DA2 process module");
        }
    }
}