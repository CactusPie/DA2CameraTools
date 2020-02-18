using System;
using System.Threading;
using System.Threading.Tasks;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement
{
    public class LoadingScreenMonitor : ILoadingScreenMonitor
    {
        public event EventHandler EnteredLoadingScreen;
        public event EventHandler ExitLoadingScreen;
        
        private CancellationTokenSource _cancellationTokenSource;

        private bool _isMonitoringLoadingScreen = false;
        private bool _previousLoadingScreenState = false;
        
        private readonly IGameValueService _gameValueService;

        public LoadingScreenMonitor(IGameValueService gameValueService)
        {
            _gameValueService = gameValueService;
        }
        
        public void StartMonitoringLoadingScreen()
        {
            if (_isMonitoringLoadingScreen)
            {
                return;
            }

            _isMonitoringLoadingScreen = true;
            _cancellationTokenSource = new CancellationTokenSource();
            _ = MonitorLoadingScreen(_cancellationTokenSource.Token);
        }

        public void StopMonitoringLoadingScreen()
        {
            if (_isMonitoringLoadingScreen)
            {
                _cancellationTokenSource?.Cancel();
            }
        }

        private async Task MonitorLoadingScreen(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    bool isLoadingScreen = _gameValueService.IsGameInLoadingScreen();

                    if (isLoadingScreen && !_previousLoadingScreenState)
                    {
                        OnEnteredLoadingScreen();
                    }

                    if (!isLoadingScreen && _previousLoadingScreenState)
                    {
                        OnExitLoadingScreen();
                    }

                    _previousLoadingScreenState = isLoadingScreen;
                    await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
            {
            }
        }

        private void OnEnteredLoadingScreen()
        {
            EventHandler eventHandler = EnteredLoadingScreen;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnExitLoadingScreen()
        {
            EventHandler eventHandler = ExitLoadingScreen;
            eventHandler?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            StopMonitoringLoadingScreen();
            _cancellationTokenSource?.Dispose();
        }
    }
}