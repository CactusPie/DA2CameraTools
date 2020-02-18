using System.Diagnostics;
using System.Threading.Tasks;

namespace DragonAge2CameraTools.GameManagement.Interfaces
{
    public interface ICodeInjectionReadinessChecker
    {
        /// <summary>
        /// Waits until the game is in a state when the code is ready to be injected
        /// </summary>
        /// <param name="gameProcess">Game process</param>
        Task WaitUntilCodeIsReadyToBeInjected(Process gameProcess);
    }
}