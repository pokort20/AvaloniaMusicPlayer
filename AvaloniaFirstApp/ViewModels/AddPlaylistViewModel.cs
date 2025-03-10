using AvaloniaFirstApp.Database;
using AvaloniaFirstApp.Models;
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
        private string _playlistName;
        private Account account; 
        private Action<Playlist> closeAction;

        private DataHandler dh;
        public ReactiveCommand<Unit, Unit> CloseCommand { get; }
        public ReactiveCommand<Unit, Unit> AddPlaylistCommand { get; }

        public AddPlaylistViewModel(DataHandler dh, Account account, Action<Playlist> closeAction)
        {
            this.dh = dh;
            this.account = account;
            this.closeAction = closeAction;
            //CloseCommand = ReactiveCommand.Create(closeAction);
            AddPlaylistCommand = ReactiveCommand.CreateFromTask(AddPlaylistCommandExecute);
        }
        private async Task AddPlaylistCommandExecute()
        {
            Playlist newPlaylist = new Playlist();
            newPlaylist.name = PlaylistName;
            newPlaylist.data = new byte[1];
            bool success = await dh.AddPlaylist(account, newPlaylist);
            if (success)
            {
                closeAction(newPlaylist);
            }
        }

        public string PlaylistName
        {
            get => _playlistName;
            set
            {
                if(value != _playlistName)
                {
                    this.RaiseAndSetIfChanged(ref _playlistName, value);
                }
            }
        }
    }
}
