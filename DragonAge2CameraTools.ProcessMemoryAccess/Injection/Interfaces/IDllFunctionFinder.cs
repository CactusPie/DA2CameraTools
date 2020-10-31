namespace DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces
{
    public interface IDllFunctionFinder
    {
        int GetLibraryFunctionOffset(string dllPath, string functionName);
    }
}