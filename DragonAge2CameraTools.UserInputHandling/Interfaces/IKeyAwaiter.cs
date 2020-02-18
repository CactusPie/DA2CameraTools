using System.Threading.Tasks;
using DragonAge2CameraTools.UserInputHandling.Enums;

namespace DragonAge2CameraTools.UserInputHandling.Interfaces
{
    public interface IKeyAwaiter
    {
        /// <summary>
        /// Wait until any key or mouse button is pressed by the user
        /// </summary>
        /// <param name="blockPressedKey">True if the pressed key should be blocked from the system and
        /// other applications, false otherwise</param>
        /// <returns>The key that was pressed</returns>
        Task<UserInputKey> WaitForKeyPress(bool blockPressedKey = false);
    }
}