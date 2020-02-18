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
        private readonly ILoadingScreenMonitorFactory _loadingScreenMonitorFactory;

        public CameraToolsFactory(
            IAddressFinderFactory addressFinderFactory,
            IGameValueServiceFactory gameValueServiceFactory,
            ILoadingScreenMonitorFactory loadingScreenMonitorFactory)
        {
            _addressFinderFactory = addressFinderFactory;
            _gameValueServiceFactory = gameValueServiceFactory;
            _loadingScreenMonitorFactory = loadingScreenMonitorFactory;
        }

        public IAddressFinder CreateAddressFinder(Process process)
        {
            return _addressFinderFactory.CreateAddressFinder(process);
        }

        public IGameValueService CreateGameValueService(IntPtr processHandle, Process process)
        {
            return _gameValueServiceFactory.CreateGameValueService(processHandle, process);
        }

        public ILoadingScreenMonitor CreateLoadingScreenMonitor(IGameValueService gameValueService)
        {
            return _loadingScreenMonitorFactory.CreateLoadingScreenMonitor(gameValueService);
        }
    }
}