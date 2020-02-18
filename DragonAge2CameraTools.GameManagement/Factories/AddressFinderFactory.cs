using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories
{
    public class AddressFinderFactory : IAddressFinderFactory
    {
        private readonly IProcessFunctionsService _processFunctionsService;

        public AddressFinderFactory(IProcessFunctionsService processFunctionsService)
        {
            _processFunctionsService = processFunctionsService;
        }
        
        public IAddressFinder CreateAddressFinder(Process process)
        {
            return new AddressFinder(_processFunctionsService, process);
        }
    }
}