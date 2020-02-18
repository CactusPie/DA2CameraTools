using System;
using System.Windows;
using Serilog;
using Serilog.Core;

namespace DragonAge2CameraTools.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Logger log = new LoggerConfiguration()
                    .WriteTo.File("logs/error-log.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
            
                log.Fatal(args.ExceptionObject as Exception, "Fatal exception occured");
            };
        }
    }
}