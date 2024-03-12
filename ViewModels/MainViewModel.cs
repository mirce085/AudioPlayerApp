using AudioPlayerApp.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerApp.ViewModels;

[INotifyPropertyChanged]
public partial class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register<ChangeViewModelMessage>(this, (sender, message) => {
            CurrentViewModel = message.ViewModel;
        });

        var model = App.ServiceProvider.GetService<PlayerViewModel>()!;
        var message = new ChangeViewModelMessage(model);

        WeakReferenceMessenger.Default.Send(message);
    }


    [ObservableProperty]
    private BaseViewModel? _currentViewModel;
}
