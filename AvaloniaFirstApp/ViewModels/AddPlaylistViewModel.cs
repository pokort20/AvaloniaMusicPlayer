using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.ViewModels
{
    class AddPlaylistViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        public AddPlaylistViewModel(Action closeAction)
        {
            CloseCommand = ReactiveCommand.Create(closeAction);
        }
    }
}
