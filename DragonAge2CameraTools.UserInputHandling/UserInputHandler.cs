using System;
using System.Threading.Tasks;
using WindowsInput.Events;
using WindowsInput.Events.Sources;
using WindowsInput.Native;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Enums;
using DragonAge2CameraTools.UserInputHandling.KeyHandlers.Interfaces;

namespace DragonAge2CameraTools.UserInputHandling
{
    public class UserInputHandler : IUserInputHandler
    {
        private TaskCompletionSource<bool> _taskCompletionSource;
        private readonly object _lock = new object();
        private bool _isProcessingKeyboardEvents;

        private readonly IHotkeyConditionService _hotkeyConditionService;
        private readonly IKeyMapper _keyMapper;
        private readonly IKeyHandler _keyHandler;
        
        public UserInputHandler
        (
            IHotkeyConditionService hotkeyConditionService, 
            IKeyMapper keyMapper,
            IKeyHandler keyHandler
        )
        {
            _hotkeyConditionService = hotkeyConditionService;
            _keyMapper = keyMapper;
            _keyHandler = keyHandler;
        }

        /// <inheritdoc/>>
        public async Task StartProcessingInputEvents()
        {
            lock (_lock)
            {
                if (_isProcessingKeyboardEvents)
                {
                    throw new InvalidOperationException($"{nameof(UserInputHandler)} is already processing keyboard events");
                }

                _isProcessingKeyboardEvents = true;
            }

            using (IKeyboardEventSource keyboard = WindowsInput.Capture.Global.KeyboardAsync()) 
            {
                using (IMouseEventSource mouse = WindowsInput.Capture.Global.MouseAsync())
                {
                    mouse.ButtonScroll += OnMouseScroll;
                    keyboard.KeyEvent += OnKeyEvent;
                    mouse.ButtonDown += (sender, args) => OnMouseOnButtonEvent(args, MouseEventType.KeyDown);
                    mouse.ButtonUp += (sender, args) => OnMouseOnButtonEvent(args, MouseEventType.KeyUp);
                    
                    _taskCompletionSource = new TaskCompletionSource<bool>();

                    await _taskCompletionSource.Task.ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc/>>
        public void StopProcessingKeyboardEvents()
        {
            lock (_lock)
            {
                if (_isProcessingKeyboardEvents)
                {
                    _isProcessingKeyboardEvents = false;
                    _taskCompletionSource.SetResult(true);
                    _keyHandler.StopCurrentlyRunningKeyFunction();
                }
            }
        }

        /// <summary>
        /// Handler for keyboard key press event
        /// </summary>
        private void OnKeyEvent(object sender, EventSourceEventArgs<KeyboardEvent> keyboardEventArgs)
        {
            if (keyboardEventArgs.Data == null)
            {
                return;
            }
            
            if (!_hotkeyConditionService.ShouldHandleHotkeyEvent(keyboardEventArgs.Data))
            {
                return;
            }
            

            InputResult? inputResult = null;

            if (keyboardEventArgs.Data.KeyDown != null)
            {
                UserInputKey mappedKey = _keyMapper.MapToUserInputKey(keyboardEventArgs.Data.KeyDown.Key);
                ModifierKeys modifierKeys = GetModifierKeys();
                inputResult = _keyHandler.OnKeyDown(mappedKey, modifierKeys);
            }
            else if (keyboardEventArgs.Data.KeyUp != null)
            {
                UserInputKey mappedKey = _keyMapper.MapToUserInputKey(keyboardEventArgs.Data.KeyUp.Key);
                inputResult = _keyHandler.OnKeyUp(mappedKey);
            }

            if (inputResult.HasValue && inputResult.Value.HasFlag(InputResult.HideFromOtherApplications))
            {
                keyboardEventArgs.Next_Hook_Enabled = false;
            }
        }
        
        /// <summary>
        /// Handler for mouse scroll event. Does not affect clicks
        /// </summary>
        private void OnMouseScroll(object sender, EventSourceEventArgs<ButtonScroll> mouseEventArgs)
        {
            if (!_hotkeyConditionService.ShouldHandleHotkeyEvent(mouseEventArgs.Data))
            {
                return;
            }

            UserInputKey mappedKey = _keyMapper.MapToUserInputKey(mouseEventArgs.Data.Offset);
            InputResult? inputResult = _keyHandler.OnKeyDown(mappedKey, ModifierKeys.None); 

            if (inputResult.Value.HasFlag(InputResult.HideFromOtherApplications))
            {
                mouseEventArgs.Next_Hook_Enabled = false;
            }
        }
        
        /// <summary>
        /// Handler for mouse click event
        /// </summary>
        /// <typeparam name="TMouseEvent">Generic parameter, since keyDown and keyUp events have different type for eventArgs</typeparam>
        private void OnMouseOnButtonEvent<TMouseEvent>(EventSourceEventArgs<TMouseEvent> e, MouseEventType eventType) where TMouseEvent : ButtonEvent
        {
            if (!_hotkeyConditionService.ShouldHandleHotkeyEvent(e.Data))
            {
                return;
            }

            InputResult inputResult;
            UserInputKey mappedKey = _keyMapper.MapToUserInputKey(e.Data.Button);

            if (eventType == MouseEventType.KeyDown)
            {
                inputResult = _keyHandler.OnKeyDown(mappedKey, ModifierKeys.None);
            }
            else
            {
                inputResult = _keyHandler.OnKeyUp(mappedKey);
            }


            if (inputResult.HasFlag(InputResult.HideFromOtherApplications))
            {
                e.Next_Hook_Enabled = false;
            }
        }

        /// <summary>
        /// Gets currently pressed modifier keys: left control, left shift, left alt or none
        /// </summary>
        /// <returns>Bit field of pressed keys</returns>
        private static ModifierKeys GetModifierKeys()
        {
            var modifierKeys = ModifierKeys.None;
            
            if (KeyboardState.GetKeyState(KeyCode.LShift).HasFlag(KeyboardKeyState.KeyDown))
            {
                modifierKeys |= ModifierKeys.Shift;
            }
            else if (KeyboardState.GetKeyState(KeyCode.LControl).HasFlag(KeyboardKeyState.KeyDown))
            {
                modifierKeys |= ModifierKeys.Control;
            } 
            else if (KeyboardState.GetKeyState(KeyCode.LAlt).HasFlag(KeyboardKeyState.KeyDown))
            {
                modifierKeys |= ModifierKeys.Alt;
            }

            return modifierKeys;
        }
        
        public void Dispose()
        {
            StopProcessingKeyboardEvents();
        }
    }
}