using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SevenZipWpf.ViewModels;

namespace SevenZipWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; } = BuildServiceProvider();

        private static ServiceProvider BuildServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceProvider
                .GetRequiredService<MainWindow>()
                .Show();
        }
    }
}
