using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.GameManagement.FunctionHooking
{
    public class GameFunctionHookServiceFactory : IGameFunctionHookServiceFactory
    {
        private readonly IDllInjector _dllInjector;
        private readonly IProcessFunctionsService _processFunctionsService;
        private readonly IDllFunctionFinder _dllFunctionFinder;
        private readonly IAddressFinderFactory _addressFinderFactory;
        private readonly string _hookDllPath;

        public GameFunctionHookServiceFactory
        (
            IDllInjector dllInjector, 
            IProcessFunctionsService processFunctionsService, 
            IDllFunctionFinder dllFunctionFinder, 
            IAddressFinderFactory addressFinderFactory,
            string hookDllPath 
        )
        {
            _dllInjector = dllInjector;
            _processFunctionsService = processFunctionsService;
            _dllFunctionFinder = dllFunctionFinder;
            _addressFinderFactory = addressFinderFactory;
            _hookDllPath = hookDllPath;
        }

        public IGameFunctionHookService CreateGameFunctionHookService(Process gameProcess)
        {
            IAddressFinder addressFinder = _addressFinderFactory.CreateAddressFinder(gameProcess);
            return new GameFunctionHookService(_dllInjector, _processFunctionsService, _dllFunctionFinder, addressFinder, _hookDllPath, gameProcess);
        }
    }
}