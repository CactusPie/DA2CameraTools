using System;
using System.Threading;
using System.Threading.Tasks;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling
{
    /// <summary>
    /// A service that loops the specified action. Especially useful for
    /// handling camera movement
    /// </summary>
    public class ActionLoopService : IActionLoopService
    {
        private CancellationTokenSource _cancellationTokenSource;
        
        public bool IsLoopingAction { get; private set; }

        public void StartLoopingAction(Action action, int millisecondDelayBetweenLoops = 1)
        {
            if (IsLoopingAction)
            {
                throw new InvalidOperationException($"{nameof(UserInputHandler)} is already processing keyboard events");
            }

            IsLoopingAction = true;
            
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            _ = LoopActionAsynchronously(action, millisecondDelayBetweenLoops, cancellationToken);
        }

        public void StopLoopingAction()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
            IsLoopingAction = false;
        }

        private static async Task LoopActionAsynchronously(Action action, int millisecondDelayBetweenLoops, CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    action();
                    await Task.Delay(millisecondDelayBetweenLoops, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
            {
            }
        }

        public void Dispose()
        {
            StopLoopingAction();
            _cancellationTokenSource?.Dispose();
        }
    }
}