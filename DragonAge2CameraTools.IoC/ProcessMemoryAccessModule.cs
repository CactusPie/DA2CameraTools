using Autofac;
using DragonAge2CameraTools.ProcessMemoryAccess;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection;
using DragonAge2CameraTools.ProcessMemoryAccess.Injection.Interfaces;
using DragonAge2CameraTools.ProcessMemoryAccess.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Factories;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;

namespace DragonAge2CameraTools.IoC
{
    public class ProcessMemoryAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProcessFunctionsService>().As<IProcessFunctionsService>().SingleInstance();
            builder.RegisterType<ActionLoopServiceFactory>().As<IActionLoopServiceFactory>().SingleInstance();
            builder.RegisterType<ActiveWindowService>().As<IActiveWindowService>().SingleInstance();
            builder.RegisterType<DllInjector>().As<IDllInjector>().SingleInstance();
            builder.RegisterType<DllFunctionFinder>().As<IDllFunctionFinder>().SingleInstance();
        }
    }
}