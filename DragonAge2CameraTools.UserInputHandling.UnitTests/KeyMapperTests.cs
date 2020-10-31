using WindowsInput.Events;
using DragonAge2CameraTools.UserInputHandling.Enums;
using DragonAge2CameraTools.UserInputHandling.Interfaces;
using Xunit;

namespace DragonAge2CameraTools.UserInputHandling.UnitTests
{
    public class KeyMapperTests
    {
        [Theory]
        [InlineData(KeyCode.W, UserInputKey.W)]
        [InlineData(KeyCode.S, UserInputKey.S)]
        [InlineData(KeyCode.A, UserInputKey.A)]
        [InlineData(KeyCode.D, UserInputKey.D)]
        [InlineData(KeyCode.Up, UserInputKey.Up)]
        [InlineData(KeyCode.Down, UserInputKey.Down)]
        [InlineData(KeyCode.Left, UserInputKey.Left)]
        [InlineData(KeyCode.Right, UserInputKey.Right)]
        [InlineData(KeyCode.NumPad0, UserInputKey.NumPad0)]
        [InlineData(KeyCode.NumPad1, UserInputKey.NumPad1)]
        public void KeyMapper_MapKeyboardKey_ProperlyMapped(KeyCode input, UserInputKey result)
        {
            IKeyMapper keyMapper = CreateKeyMapper();

            UserInputKey mappedKey = keyMapper.MapToUserInputKey(input);
            
            Assert.Equal(mappedKey, result);
        }
        
        [Theory]
        [InlineData(ButtonCode.Left, UserInputKey.LeftMouseButton)]
        [InlineData(ButtonCode.Right, UserInputKey.RightMouseButton)]
        [InlineData(ButtonCode.Middle, UserInputKey.MiddleMouseButton)]
        [InlineData(ButtonCode.XButton1, UserInputKey.Mouse4)]
        [InlineData(ButtonCode.XButton2, UserInputKey.Mouse5)]
        public void KeyMapper_MapMouseKey_ProperlyMapped(ButtonCode input, UserInputKey result)
        {
            IKeyMapper keyMapper = CreateKeyMapper();

            UserInputKey mappedKey = keyMapper.MapToUserInputKey(input);
            
            Assert.Equal(mappedKey, result);
        }

        private static IKeyMapper CreateKeyMapper()
        {
            return new KeyMapper();
        }
    }
}