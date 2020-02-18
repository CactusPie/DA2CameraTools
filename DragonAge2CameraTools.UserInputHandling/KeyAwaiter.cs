using System.Threading.Tasks;
using WindowsInput.Events;
using WindowsInput.Events.Sources;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling
{
    public class KeyAwaiter : IKeyAwaiter
    {
        private readonly IKeyMapper _keyMapper;

        public KeyAwaiter(IKeyMapper keyMapper)
        {
            _keyMapper = keyMapper;
        }

        /// <inheritdoc/>
        public async Task<UserInputKey> WaitForKeyPress(bool blockPressedKey = false)
        {
            var taskCompletionSource = new TaskCompletionSource<UserInputKey>();
            
            using (IKeyboardEventSource keyboard = WindowsInput.Capture.Global.KeyboardAsync()) 
            {
                using (IMouseEventSource mouse = WindowsInput.Capture.Global.MouseAsync())
                {
                    mouse.ButtonScroll += (_, keyEventArgs) =>
                    {
                        if (keyEventArgs?.Data?.Button != null)
                        {
                            UserInputKey mappedKey = _keyMapper.MapToUserInputKey(keyEventArgs.Data.Offset);
                            if (blockPressedKey)
                            {
                                keyEventArgs.Next_Hook_Enabled = false;
                            }
                            taskCompletionSource.SetResult(mappedKey);
                        }
                    };
                    mouse.ButtonDown += (_, keyEventArgs) =>
                    {
                        if (keyEventArgs?.Data?.Button != null && keyEventArgs.Data.Button != ButtonCode.VScroll && keyEventArgs.Data.Button != ButtonCode.HScroll)
                        {
                            UserInputKey mappedKey = _keyMapper.MapToUserInputKey(keyEventArgs.Data.Button);
                            if (blockPressedKey)
                            {
                                keyEventArgs.Next_Hook_Enabled = false;
                            }
                            taskCompletionSource.SetResult(mappedKey);
                        }
                    };
                    keyboard.KeyEvent += (_, keyEventArgs) =>
                    {
                        if (keyEventArgs?.Data?.KeyDown != null)
                        {
                            UserInputKey mappedKey = _keyMapper.MapToUserInputKey(keyEventArgs.Data.KeyDown.Key);
                            if (blockPressedKey)
                            {
                                keyEventArgs.Next_Hook_Enabled = false;
                            }
                            taskCompletionSource.SetResult(mappedKey);
                        }
                    };
                    
                    return await taskCompletionSource.Task.ConfigureAwait(false);
                }
            }
        }
    }
}