using Autofac;
using DragonAge2CameraTools.UserInputHandling.Factories;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.ViewLogic;
using DragonAge2CameraTools.ViewLogic.Factories;
using DragonAge2CameraTools.ViewLogic.Factories.Interfaces;
using DragonAge2CameraTools.ViewLogic.Interfaces;
using DragonAge2CameraTools.ViewLogic.Settings;
using DragonAge2CameraTools.ViewLogic.Settings.Interfaces;
using DragonAge2CameraTools.ViewLogic.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using JsonSerializer = DragonAge2CameraTools.ViewLogic.Settings.JsonSerializer;

namespace DragonAge2CameraTools.IoC
{
    public class ViewLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            
            builder.RegisterType<SettingsMapper>().As<ISettingsMapper>().SingleInstance();
            builder.RegisterType<TacticalCameraService>().As<ITacticalCameraService>().InstancePerDependency();
            builder.RegisterType<TacticalCameraServiceFactory>().As<ITacticalCameraServiceFactory>().SingleInstance();
            builder.RegisterType<TacticalCameraKeyHandlerFactory>().As<ITacticalCameraKeyHandlerFactory>().SingleInstance();

            builder.RegisterInstance
            (
                new JsonSerializer
                (
                    new JsonSerializerSettings
                    {
                        Converters = {new StringEnumConverter(new CamelCaseNamingStrategy())},
                        Formatting = Formatting.Indented
                    }
                )
            ).As<IStringSerializer>();
            
            builder.Register
            (
                x => new ApplicationSettingsRepository
                (
                    x.Resolve<IStringSerializer>(), 
                    "settings.cfg"
                )
            )
            .As<IApplicationSettingsRepository>()
            .SingleInstance();
        }
    }
}