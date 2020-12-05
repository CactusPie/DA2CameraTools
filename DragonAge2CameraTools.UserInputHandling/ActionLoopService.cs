using System;
using System.Diagnostics;
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
        private Timer _timer;
        
        public bool IsLoopingAction { get; private set; }

        public void StartLoopingAction(Action action, int millisecondDelayBetweenLoops)
        {
            if (IsLoopingAction)
            {
                throw new InvalidOperationException($"{nameof(ActionLoopService)} is already looping an action");
            }

            IsLoopingAction = true;

            if (_timer == null)
            {
                _timer = new Timer(state => action(), null, 0, millisecondDelayBetweenLoops);
            }
            else
            {
                _timer.Change(0, millisecondDelayBetweenLoops);
            }
        }

        public void StopLoopingAction()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            IsLoopingAction = false;
        }

        public void Dispose()
        {
            StopLoopingAction();
            _timer?.Dispose();
        }
    }
}