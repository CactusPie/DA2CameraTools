namespace DragonAge2CameraTools.GameManagement.FunctionHooking.Data
{
    public static class AssemblyInstruction
    {
        public const byte Nop = 0x90;
        public const byte Call = 0xE8;
        public const byte NearJump = 0xE9;
    }
}