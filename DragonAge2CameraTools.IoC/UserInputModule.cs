using Autofac;
using DragonAge2CameraTools.UserInputHandling;
using DragonAge2CameraTools.UserInputHandling.Factories;
using DragonAge2CameraTools.UserInputHandling.Factories.Interfaces;
using DragonAge2CameraTools.UserInputHandling.Interfaces;

namespace DragonAge2CameraTools.IoC
{
    public class UserInputModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserInputHandlerFactory>().As<IUserInputHandlerFactory>().SingleInstance();
            builder.RegisterType<HotkeyConditionServiceFactory>().As<IHotkeyConditionServiceFactory>().SingleInstance();
            builder.RegisterType<KeyAndMouseEventHandlerFactory>().As<IKeyAndMouseEventHandlerFactory>().SingleInstance();
            builder.RegisterType<KeyMapper>().As<IKeyMapper>().SingleInstance();
            builder.RegisterType<KeyAwaiter>().As<IKeyAwaiter>().SingleInstance();
        }
    }
}