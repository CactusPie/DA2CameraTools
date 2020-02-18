using Autofac;
using DragonAge2CameraTools.GameManagement;
using DragonAge2CameraTools.GameManagement.Factories;
using DragonAge2CameraTools.GameManagement.Factories.Interfaces;
using DragonAge2CameraTools.GameManagement.Interfaces;
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
            
            builder.RegisterType<LoadingScreenMonitorFactory>().As<ILoadingScreenMonitorFactory>().SingleInstance();
            builder.RegisterType<CameraToolsFactory>().As<ICameraToolsFactory>().SingleInstance();
            
            builder.Register
            (
                x => new CodeInjectionReadinessChecker
                (
                    x.Resolve<IProcessFunctionsService>(),
                    x.Resolve<AddressFinderFactory>()
                )
            ).As<ICodeInjectionReadinessChecker>();
        }
    }
}