using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories
{
    public class AddressFinderWithCacheFactory : IAddressFinderFactory
    {
        private readonly IAddressFinderFactory _addressFinderFactory;

        public AddressFinderWithCacheFactory(IAddressFinderFactory addressFinderFactory)
        {
            _addressFinderFactory = addressFinderFactory;
        }
        
        public IAddressFinder CreateAddressFinder(Process process)
        {
            return new AddressFinderWithCache(_addressFinderFactory.CreateAddressFinder(process));
        }
    }
}