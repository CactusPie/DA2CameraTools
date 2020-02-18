using Autofac;
using DragonAge2CameraTools.IoC;
using DragonAge2CameraTools.ViewLogic.ViewModels;

namespace DragonAge2CameraTools.Wpf
{
    public class ViewModelLocator 
    {
        private readonly IContainer _container;
        
        public MainWindowViewModel MainWindowViewModel => _container.Resolve<MainWindowViewModel>();
            
        public ViewModelLocator()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new GameManagementModule());
            containerBuilder.RegisterModule(new UserInputModule());
            containerBuilder.RegisterModule(new ProcessMemoryAccessModule());
            containerBuilder.RegisterModule(new ViewLogicModule());
            
            _container = containerBuilder.Build();
        }

    }
}