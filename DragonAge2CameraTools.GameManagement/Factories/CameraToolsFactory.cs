using System;
using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories
{
    public class CameraToolsFactory : ICameraToolsFactory
    {
        private readonly IAddressFinderFactory _addressFinderFactory;
        private readonly IGameValueServiceFactory _gameValueServiceFactory;

        public CameraToolsFactory
        (
            IAddressFinderFactory addressFinderFactory,
            IGameValueServiceFactory gameValueServiceFactory
        )
        {
            _addressFinderFactory = addressFinderFactory;
            _gameValueServiceFactory = gameValueServiceFactory;
        }

        public IAddressFinder CreateAddressFinder(Process gameProcess)
        {
            return _addressFinderFactory.CreateAddressFinder(gameProcess);
        }

        public IGameValueService CreateGameValueService(Process gameProcess)
        {
            return _gameValueServiceFactory.CreateGameValueService(gameProcess);
        }
    }
}