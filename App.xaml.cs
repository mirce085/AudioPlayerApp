using AudioPlayerApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using AudioPlayerApp.Views;

namespace AudioPlayerApp;

public partial class App : Application
{
    public static ServiceCollection ServiceCollection { get; private set; } = null!;
    public static ServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ServiceCollection = new ServiceCollection();
        ServiceCollection.AddSingleton<PlayerViewModel>();
        ServiceCollection.AddSingleton<MainViewModel>();
        ServiceCollection.AddSingleton<MainView>();

        ServiceProvider = ServiceCollection.BuildServiceProvider();

        var view = ServiceProvider.GetService<MainView>()!;

        view.Show();

    }
}
