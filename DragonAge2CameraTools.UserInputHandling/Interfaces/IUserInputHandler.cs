using System;
using System.Threading.Tasks;

namespace DragonAge2CameraTools.UserInputHandling.Interfaces
{
    /// <summary>
    /// A handler for keyboard and mouse events (key press, click, mouse scroll)
    /// Does not affect mouse movement events
    /// </summary>
    public interface IUserInputHandler : IDisposable
    {
        /// <summary>
        /// Starts listening for keyboard and mouse events and processing them asynchronously
        /// </summary>
        /// <returns></returns>
        Task StartProcessingInputEvents();
        
        /// <summary>
        /// Stops processing keyboard and mouse events
        /// </summary>
        void StopProcessingKeyboardEvents();
    }
}