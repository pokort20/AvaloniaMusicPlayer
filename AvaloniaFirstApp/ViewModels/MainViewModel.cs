using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Joins;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;
using Avalonia.Input;
using System.Collections.ObjectModel;
using AvaloniaFirstApp.Models;
using AvaloniaFirstApp.Database;
using System.Linq;
using System.Collections;
using System.Security.Principal;
using System.Reactive.Linq;
using DynamicData.Kernel;

namespace AvaloniaFirstApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    //private variables
    private string _songName;
    private string _authorName;
    private string _timeElapsed;
    private string _timeLeft;
    private string _accountName;
    private string _searchText;
    private double _volume;
    private Bitmap _playPauseImage;
    private Bitmap _volumeImage;
    private Account _account;

    private readonly DataHandler dh;
    private bool isPlaying;
    private Bitmap playImage;
    private Bitmap pauseImage;
    private Bitmap mutedImage;
    private Bitmap volumeLowImage;
    private Bitmap volumeHighImage;

    //Collections
    public ObservableCollection<Song> SongQueue { get; } = new ObservableCollection<Song>();
    public ObservableCollection<Playlist> Playlists { get; } = new ObservableCollection<Playlist>();
    public ObservableCollection<Artist> Artists { get; } = new ObservableCollection<Artist>();
    public ObservableCollection<Song> SearchSongs { get; } = new ObservableCollection<Song>();

    //commands
    public ReactiveCommand<Unit, string> PlayCommandA { get; }
    public ReactiveCommand<Unit, Unit> PlayCommand { get; }
    public MainViewModel()
    {
        isPlaying = false;
        SongName = "sTraNgeRs";
        AuthorName = "Bring me the horizon";
        TimeElapsed = "1:01";
        TimeLeft = "2:11";

        dh = new DataHandler();
        LoadUserAccountAsync();

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(300))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async text => await ShowSearchResults(text));

        InitUIImages();
        InitVariables();

        PlayCommandA = ReactiveCommand.CreateFromTask(PlayCommandAsync);
        PlayCommand = ReactiveCommand.Create(PlayCommandExecute);

        Console.WriteLine("MainViewModel initialized!");
    }
    #region events

    public void OnSliderScroll(PointerWheelEventArgs args)
    {
        Debug.WriteLine("Scroll event called");
    }
    #endregion

    #region executes
    private async Task<string> PlayCommandAsync()
    {
        TimeElapsed = "1:69";
        return "Playing";
    }
    private void PlayCommandExecute()
    {
        //do something
        if (isPlaying)
        {
            PlayPauseImage = playImage;
            Debug.WriteLine("Paused");
        }
        else
        {
            PlayPauseImage = pauseImage;
            Debug.WriteLine("Playing");
        }
        isPlaying = !isPlaying;
    }
    #endregion

    #region properties
    public string SearchText
    {
        get => _searchText;
        set
        {
            if(_searchText != value)
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
                Debug.WriteLine("Search: " + value);
            }
        }
    }
    public string AccountName
    {
        get => _accountName;
        set
        {
            if(_accountName != value)
            {
                this.RaiseAndSetIfChanged(ref _accountName, value);
            }
        }
    }
    public Account UserAccount
    {
        get => _account;
        set
        {
            if(_account != value)
            {
                _account = value;
                this.RaiseAndSetIfChanged(ref _account, value);
            }
        }
    }
    public double Volume
    {
        get => _volume;
        set
        {
            if (_volume != value)
            {
                this.RaiseAndSetIfChanged(ref _volume, value);
                ChangeVolumeImage(value);
                Debug.WriteLine("Volume: " + value);
            }
        }
    }
    public Bitmap VolumeImage
    {
        get => _volumeImage;
        set
        {
            if (_volumeImage != value)
            {
                this.RaiseAndSetIfChanged(ref _volumeImage, value);
            }
        }
    }
    public Bitmap PlayPauseImage
    {
        get => _playPauseImage;
        set
        {
            if (_playPauseImage != value)
            {
                this.RaiseAndSetIfChanged(ref _playPauseImage, value);
            }
        }
    }
    public string SongName
    {
        get => _songName;
        set
        {
            if (_songName != value)
            {
                this.RaiseAndSetIfChanged(ref _songName, value);
            }
        }
    }

    public string AuthorName
    {
        get => _authorName;
        set
        {
            if (_authorName != value)
            {
                this.RaiseAndSetIfChanged(ref _timeElapsed, value);
            }
        }
    }

    public string TimeElapsed
    {
        get => _timeElapsed;
        set
        {
            if (_timeElapsed != value)
            {
                this.RaiseAndSetIfChanged(ref _timeElapsed, value);
            }
        }
    }
    public string TimeLeft
    {
        get => _timeLeft;
        set
        {
            if (_timeLeft != value)
            {
                this.RaiseAndSetIfChanged(ref _timeLeft, value);
            }
        }
    }
    #endregion

    #region methods
    private async Task ShowSearchResults(string searchTerm)
    {
        try
        {
            SearchSongs.Clear();
            foreach(var x in await dh.SearchSongs(searchTerm))
            {
                SearchSongs.Add(x);
            }
        }
        catch(Exception e)
        {
            Debug.WriteLine("An error occured, " + e.Message);
        }
    }
    private async Task LoadUserAccountAsync()
    {
        try
        {
            // Assuming you have some logic to get account id, here using a hardcoded account id for example
            int accountId = 1;  // Replace with actual account ID logic

            // Get the account asynchronously
            UserAccount = await dh.GetUserAccount(accountId);
            AccountName = UserAccount.username;
            Debug.WriteLine("Account:" + UserAccount);
        }
        catch (Exception ex)
        {
            // Handle any potential exceptions, e.g., logging, displaying error message
            Console.WriteLine($"Error loading account: {ex.Message}");
        }
    }
    private void InitVariables()
    {
        isPlaying = false;
        Volume = 25;
    }
    private void InitUIImages()
    {
        playImage = new Bitmap("../../../../AvaloniaFirstApp/Assets/Images/play.png");
        pauseImage = new Bitmap("../../../../AvaloniaFirstApp/Assets/Images/pause.png");

        mutedImage = new Bitmap("../../../../AvaloniaFirstApp/Assets/Images/volume0.png");
        volumeLowImage = new Bitmap("../../../../AvaloniaFirstApp/Assets/Images/volume1.png");
        volumeHighImage = new Bitmap("../../../../AvaloniaFirstApp/Assets/Images/volume2.png");
        PlayPauseImage = playImage;
    }
    private void ChangeVolumeImage(double volume)
    {
        if(volume <= 0)
        {
            VolumeImage = mutedImage;
        }
        else if(volume > 0 && volume <= 25)
        {
            VolumeImage = volumeLowImage;
        }
        else
        {
            VolumeImage = volumeHighImage;
        }
    }

    #endregion
}
