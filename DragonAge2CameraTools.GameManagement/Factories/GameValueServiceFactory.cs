using System;
using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories
{
    public class GameValueServiceFactory : IGameValueServiceFactory
    {
        private readonly IAddressFinderFactory _addressFinderFactory;
        private readonly IProcessFunctionsService _processFunctionsService;

        public GameValueServiceFactory(IAddressFinderFactory addressFinderFactory, IProcessFunctionsService processFunctionsService)
        {
            _addressFinderFactory = addressFinderFactory;
            _processFunctionsService = processFunctionsService;
        }
        
        public IGameValueService CreateGameValueService(Process process)
        {
            IAddressFinder addressFinder = _addressFinderFactory.CreateAddressFinder(process);
            return new GameValueService(addressFinder, _processFunctionsService, process.Handle);
        }
    }
}