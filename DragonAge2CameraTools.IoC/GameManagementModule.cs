using System.IO;
using Autofac;
using DragonAge2CameraTools.GameManagement;
using DragonAge2CameraTools.GameManagement.Factories;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.FunctionHooking;
using DragonAge2CameraTools.GameManagement.FunctionHooking.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;

namespace DragonAge2CameraTools.IoC
{
    public class GameManagementModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GameValueServiceFactory>().As<IGameValueServiceFactory>().SingleInstance();
            
            builder.RegisterType<AddressFinderFactory>().AsSelf().SingleInstance();
            builder.Register
            (
                x => new AddressFinderWithCacheFactory
                (
                    x.Resolve<AddressFinderFactory>()
                )
            ).As<IAddressFinderFactory>().SingleInstance();
            
            builder.RegisterType<GameEventServiceFactory>().As<IGameEventServiceFactory>().SingleInstance();
            builder.RegisterType<CameraToolsFactory>().As<ICameraToolsFactory>().SingleInstance();
            
            builder.Register
            (
                x => new CodeInjectionReadinessChecker
                (
                    x.Resolve<IProcessFunctionsService>(),
                    x.Resolve<AddressFinderFactory>()
                )
            ).As<ICodeInjectionReadinessChecker>();

            // Source code for the DLL is located in \InterceptionLibrary\DragonAge2InterceptionLibrary\DragonAge2InterceptionLibrary.sln
            string dllPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"DragonAge2InterceptionLibrary.dll");
            
            builder.Register
            (x => new GameFunctionHookServiceFactory
                (
                    x.Resolve<IDllInjector>(),
                    x.Resolve<IProcessFunctionsService>(),
                    x.Resolve<IDllFunctionFinder>(),
                    x.Resolve<IAddressFinderFactory>(),
                    dllPath
                )
            ).As<IGameFunctionHookServiceFactory>().SingleInstance();
        }
    }
}