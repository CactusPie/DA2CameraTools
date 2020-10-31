using System;

namespace DragonAge2CameraTools.UserInputHandling.Interfaces
{
    public interface IActionLoopService : IDisposable
    {
        bool IsLoopingAction { get; }
        void StartLoopingAction(Action action, int millisecondDelayBetweenLoops = 10);
        void StopLoopingAction();
    }
}