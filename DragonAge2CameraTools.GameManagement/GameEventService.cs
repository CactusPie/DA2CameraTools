using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement
{
    public class GameEventService : IGameEventService
    {
        public event EventHandler EnteredLoadingScreen;
        public event EventHandler ExitedLoadingScreen;
        public event EventHandler EnteredMenuOrDialogue;
        public event EventHandler ExitedMenuOrDialogue;
        
        private CancellationTokenSource _cancellationTokenSource;

        private bool _isMonitoringEvents;

        private readonly IGameFunctionHookService _functionHookService;

        public GameEventService(IGameFunctionHookService functionHookService)
        {
            _functionHookService = functionHookService;
        }
        
        public void StartMonitoringGameEvents()
        {
            if (_isMonitoringEvents)
            {
                return;
            }

            _isMonitoringEvents = true;
            _cancellationTokenSource = new CancellationTokenSource();

            WaitHandle cancellationHandle = _cancellationTokenSource.Token.WaitHandle;
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            
            _functionHookService.EnableAllHooks();
            
            var menuOrDialogueEnteredWaitHandle = new[] { EventWaitHandle.OpenExisting("GlobalDA2MenuOrDialogueEntered"), cancellationHandle };
            var menuOrDialogueExitedWaitHandle = new[] { EventWaitHandle.OpenExisting("GlobalDA2MenuOrDialogueExited"), cancellationHandle };
            var loadingScreenEnteredWaitHandle = new[] { EventWaitHandle.OpenExisting("GlobalDA2LoadingScreenEntered"), cancellationHandle };
            var loadingScreenExitedWaitHandle = new[] { EventWaitHandle.OpenExisting("GlobalDA2LoadingScreenExited"), cancellationHandle };
            
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    WaitHandle.WaitAny(menuOrDialogueEnteredWaitHandle);
                    OnEnteredMenuOrDialogue();
                }
            }, cancellationToken);
            
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    WaitHandle.WaitAny(menuOrDialogueExitedWaitHandle);
                    OnExitedMenuOrDialogue();
                }
            }, cancellationToken);
            
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    WaitHandle.WaitAny(loadingScreenEnteredWaitHandle);
                    OnEnteredLoadingScreen();
                }
            }, cancellationToken);
            
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    WaitHandle.WaitAny(loadingScreenExitedWaitHandle);
                    OnExitedLoadingScreen();
                }
            }, cancellationToken);
        }

        public void StopMonitoringGameEvents()
        {
            if (_isMonitoringEvents)
            {
                _cancellationTokenSource?.Cancel();
            }
            
            _functionHookService.DisableAllHooks();
        }

        private void OnEnteredMenuOrDialogue()
        {
            EventHandler eventHandler = EnteredMenuOrDialogue;
            eventHandler?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine("Menu or dialogue entered");
        }
        
        private void OnExitedMenuOrDialogue()
        {
            EventHandler eventHandler = ExitedMenuOrDialogue;
            eventHandler?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine("Menu or dialogue exited");
        }
        
        private void OnEnteredLoadingScreen()
        {
            EventHandler eventHandler = EnteredLoadingScreen;
            eventHandler?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine("Loading screen entered");
        }
        
        private void OnExitedLoadingScreen()
        {
            EventHandler eventHandler = ExitedLoadingScreen;
            eventHandler?.Invoke(this, EventArgs.Empty);
            Debug.WriteLine("Loading screen exited");
        }

        public void Dispose()
        {
            StopMonitoringGameEvents();
            _cancellationTokenSource?.Dispose();
        }
    }
}