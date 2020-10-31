using System.Diagnostics;

namespace DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces
{
    public interface IGameFunctionHookServiceFactory
    {
        IGameFunctionHookService CreateGameFunctionHookService(Process gameProcess);
    }
}