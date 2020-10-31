using System.Diagnostics;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;

namespace DragonAge2CameraTools.GameManagement.Factories
{
    public class GameEventServiceFactory : IGameEventServiceFactory
    {
        private readonly IGameFunctionHookServiceFactory _gameFunctionHookServiceFactory;

        public GameEventServiceFactory(IGameFunctionHookServiceFactory gameFunctionHookServiceFactory)
        {
            _gameFunctionHookServiceFactory = gameFunctionHookServiceFactory;
        }
        
        public IGameEventService CreateGameEventService(Process gameProcess)
        {
            IGameFunctionHookService gameFunctionHookService = _gameFunctionHookServiceFactory.CreateGameFunctionHookService(gameProcess);
            return new GameEventService(gameFunctionHookService);
        }
    }
}