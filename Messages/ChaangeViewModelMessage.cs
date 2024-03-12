using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioPlayerApp.ViewModels;

namespace AudioPlayerApp.Messages;

public class ChangeViewModelMessage : Message
{
    public ChangeViewModelMessage(BaseViewModel viewModel)
    {
        ViewModel = viewModel;
    }

    public BaseViewModel ViewModel { get; set; }
}