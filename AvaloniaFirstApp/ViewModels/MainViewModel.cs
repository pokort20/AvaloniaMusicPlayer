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
using AvaloniaFirstApp.Support;
using System.Reactive.Threading.Tasks;
using System.Collections.Generic;

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
    private double _songProgress;
    private Bitmap _playPauseImage;
    private Bitmap _volumeImage;
    private Account _account;
    private int _selectedTabIndex;
    private bool _isHomePageVisible;
    private bool _isTabControlVisible;

    private readonly DataHandler dh;
    private SongQueue sq;
    private bool isPlaying;
    private bool isMuted;
    private bool shuffle;
    private bool repeat;
    private double preMuteVolume;
    private Bitmap playImage;
    private Bitmap pauseImage;
    private Bitmap mutedImage;
    private Bitmap volumeLowImage;
    private Bitmap volumeHighImage;

    #region Collections
    public ObservableCollection<Song> SongQueue { get; } = new ObservableCollection<Song>();
    public ObservableCollection<Playlist> Playlists { get; } = new ObservableCollection<Playlist>();
    public ObservableCollection<Artist> Artists { get; } = new ObservableCollection<Artist>();
    public ObservableCollection<Song> SearchSongs { get; } = new ObservableCollection<Song>();
    public ObservableCollection<Album> SearchAlbums { get; } = new ObservableCollection<Album>();
    public ObservableCollection<Artist> SearchArtists { get; } = new ObservableCollection<Artist>();
    public ObservableCollection<Podcast> SearchPodcasts { get; } = new ObservableCollection<Podcast>();
    public ObservableCollection<Playlist> SearchPlaylists { get; } = new ObservableCollection<Playlist>();
    public ObservableCollection<Song> TrendingSongs { get; } = new ObservableCollection<Song>();
    public ObservableCollection<Artist> TrendingArtists { get; } = new ObservableCollection<Artist>();
    public ObservableCollection<Song> SuggestedSongs { get; } = new ObservableCollection<Song>();
    public ObservableCollection<Artist> SuggestedArtists { get; } = new ObservableCollection<Artist>();


    #endregion

    #region Commands
    public ReactiveCommand<Unit, string> PlayCommandA { get; }
    public ReactiveCommand<Unit, Unit> ShuffleCommand { get; }
    public ReactiveCommand<Unit, Unit> RepeatCommand { get; }
    public ReactiveCommand<Song, Unit> PlaySongCommand { get; }
    public ReactiveCommand<Album, Unit> PlayAlbumCommand { get; }
    public ReactiveCommand<Playlist, Unit> PlayPlaylistCommand { get; }
    public ReactiveCommand<Artist, Unit> PlayArtistCommand { get; }
    public ReactiveCommand<Podcast, Unit> PlayPodcastCommand { get; }
    public ReactiveCommand<Unit, Unit> PlayPauseCommand { get; }
    public ReactiveCommand<Unit, Unit> PreviousCommand { get; }
    public ReactiveCommand<Unit, Unit> NextCommand { get; }
    public ReactiveCommand<Unit, Unit> MuteCommand { get; }
    public ReactiveCommand<Unit, Unit> HomeCommand { get; }

    public ReactiveCommand<PointerPressedEventArgs, Unit> OnSongBarClicked { get; }

    #endregion
    public MainViewModel()
    {
        dh = new DataHandler();
        sq = new SongQueue();
        
        InitVariables();
        LoadUserAccountAsync();

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(300))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async text => await ShowSearchResults(text));
        this.WhenAnyValue(x => x.SelectedTabIndex)
            .Throttle(TimeSpan.FromMilliseconds(300))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async text => await ShowSearchResults(SearchText));

        InitUIImages();


        PlayCommandA = ReactiveCommand.CreateFromTask(PlayCommandAsync);
        PlaySongCommand = ReactiveCommand.Create<Song>(PlaySongCommandExecute);
        PlayAlbumCommand = ReactiveCommand.Create<Album>(PlayAlbumCommandExecute);
        PlayArtistCommand = ReactiveCommand.CreateFromTask<Artist>(PlayArtistCommandExecuteAsync);
        PlayPlaylistCommand = ReactiveCommand.CreateFromTask<Playlist>(PlayPlaylistCommandExecuteAsync);
        PlayPodcastCommand = ReactiveCommand.Create<Podcast>(PlayPodcastCommandExecute);



        PlayPauseCommand = ReactiveCommand.Create(PlayPauseCommandExecute);
        MuteCommand = ReactiveCommand.Create(MuteCommandExecute);
        PreviousCommand = ReactiveCommand.Create(PreviousCommandExecute);
        NextCommand = ReactiveCommand.Create(NextCommandExecute);
        ShuffleCommand = ReactiveCommand.Create(ShuffleCommandExecute);
        RepeatCommand = ReactiveCommand.Create(RepeatCommandExecute);
        HomeCommand = ReactiveCommand.Create(HomeCommandExecute);

        OnSongBarClicked = ReactiveCommand.Create<PointerPressedEventArgs>(HandleSongProgressClicked);

        InitHomePage();
        Console.WriteLine("MainViewModel initialized!");
    }
    #region events

    public void OnSliderScroll(PointerWheelEventArgs args)
    {
        Debug.WriteLine("Scroll event called");
    }
    #endregion

    #region executes
    private void HandleSongProgressClicked(PointerPressedEventArgs e)
    {
        var point = e.GetPosition(null); // Get the position relative to the window (or use the ProgressBar itself if needed)
        var progress = point.X / 300; // Assuming the ProgressBar's width is 300

        SongProgress = Math.Min(Math.Max(progress, 0), 1);
        Debug.WriteLine("progress bar click value: " + SongProgress + ", progress: " + progress);
    }
    private void PreviousCommandExecute()
    {
        PlaySongCommandExecute(sq.Previous());
        UpdateUIQueue();
        Debug.WriteLine("Play previous song");
    }
    private void NextCommandExecute()
    {
        PlaySongCommandExecute(sq.Next());
        UpdateUIQueue();
        Debug.WriteLine("Play next song");
    }
    private void ShuffleCommandExecute()
    {
        shuffle = !shuffle;
        if(shuffle)
        {
            sq.ShuffleQueue(includeCurrentPlayingSong: false);
            UpdateUIQueue();
        }
        Debug.WriteLine("Shuffle: " + shuffle);
    }
    private void HomeCommandExecute()
    {
        SearchText = string.Empty;
    }
    private void RepeatCommandExecute()
    {
        repeat = !repeat;
        Debug.WriteLine("Repeat: " + repeat);
    }
    private void MuteCommandExecute()
    {
        if(isMuted)
        {
            Volume = preMuteVolume;
        }
        else
        {
            if(Volume != 0.0)preMuteVolume = Volume;
            Volume = 0;
        }
        isMuted = !isMuted;
    }
    private async Task<string> PlayCommandAsync()
    {
        TimeElapsed = "1:69";
        return "Playing";
    }
    private void PlayPauseCommandExecute()
    {
        if (sq.CurrentPlayingSong == null) return;
        if (isPlaying)
        {
            PlayPauseImage = playImage;
            AudioPlayer.Pause();
            Debug.WriteLine("Paused");
        }
        else
        {
            SongName = sq.CurrentPlayingSong.name;
            AuthorName = sq.CurrentPlayingSong.SongArtistsNames;
            TimeLeft = sq.CurrentPlayingSong.duration.ToString(@"m\:ss");
            TimeElapsed = sq.CurrentPlayingSong.duration.ToString(@"m\:ss");
            PlayPauseImage = pauseImage;
            AudioPlayer.Resume();

            Debug.WriteLine("Playing song: " + sq.CurrentPlayingSong.name + ", made by:" + sq.CurrentPlayingSong.SongArtistsNames + ", duration: " + sq.CurrentPlayingSong.duration);
        }
        isPlaying = !isPlaying;
    }
    private void PlaySongCommandExecute(Song s)
    {
        if (s == null) return;
        sq.CurrentPlayingSong = s;
        SongName = s.name;
        AuthorName = s.SongArtistsNames;
        TimeLeft = s.duration.ToString(@"m\:ss");
        TimeElapsed = s.duration.ToString(@"m\:ss");

        isPlaying = true;
        PlayPauseImage = pauseImage;
        AudioPlayer.Play();
        AudioPlayer.ChangeVolume((float)Utils.MapToRange(Volume, 0.0, 50.0, 0.0, 1.0));

        Debug.WriteLine("Playing song: " + s.name + ", made by:" + s.SongArtistsNames + ", duration: " + s.duration);
    }
    private void PlayAlbumCommandExecute(Album album)
    {
        // Code to handle playing the album
        Debug.WriteLine("Playing: " + album.ToString());
    }
    private async Task PlayPlaylistCommandExecuteAsync(Playlist playlist)
    {
        sq.ClearQueue();
        foreach (var s in await dh.SearchSongs(playlist))
        {
            sq.Add(s);
        }
        if (shuffle) sq.ShuffleQueue(includeCurrentPlayingSong: true);
        UpdateUIQueue();
        PlaySongCommandExecute(sq.CurrentPlayingSong);
        Debug.WriteLine("Playing: " + playlist.ToString());
    }
    private async Task PlayArtistCommandExecuteAsync(Artist artist)
    {
        sq.ClearQueue();
        foreach (var s in await dh.SearchSongs(artist))
        {
            sq.Add(s);
        }
        if (shuffle) sq.ShuffleQueue(includeCurrentPlayingSong: true);
        UpdateUIQueue();
        PlaySongCommandExecute(sq.CurrentPlayingSong);
        Debug.WriteLine("Playing: " + artist.ToString());
    }
    private void PlayPodcastCommandExecute(Podcast podcast)
    {
        // Code to handle playing the podcast
        Debug.WriteLine("Playing: " + podcast.ToString());
    }
    #endregion

    #region properties
    public double SongProgress
    {
        get => _songProgress;
        set
        {
            if(_songProgress != value)
            {
                this.RaiseAndSetIfChanged(ref _songProgress, value);
            }
        }
    }
    public bool IsTabControlVisible
    {
        get => _isTabControlVisible;
        set
        {
            if (_isTabControlVisible != value)
            {
                this.RaiseAndSetIfChanged(ref _isTabControlVisible, value);
            }
        }
    }
    public bool IsHomePageVisible
    {
        get => _isHomePageVisible;
        set
        {
            if (_isHomePageVisible != value)
            {
                this.RaiseAndSetIfChanged(ref _isHomePageVisible, value);
                IsTabControlVisible = !value;
            }
        }
    }
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set
        {
            if(_selectedTabIndex != value)
            {
                this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
                Debug.WriteLine("Selected tab index: " + value);
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if(_searchText != value)
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
                if (value == string.Empty || value == null || value == "")
                {
                    IsHomePageVisible = true;
                }
                else
                {
                    IsHomePageVisible = false;
                }
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
                AudioPlayer.ChangeVolume((float)Utils.MapToRange(value, 0.0, 50.0, 0.0, 1.0));
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
                this.RaiseAndSetIfChanged(ref _authorName, value);
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
    private async void InitHomePage()
    {
        TrendingSongs.Clear();
        TrendingArtists.Clear();
        SuggestedSongs.Clear();
        SuggestedArtists.Clear();
        foreach (var x in await dh.TrendingSongs(3))
        {
            TrendingSongs.Add(x);
        }
        foreach (var x in await dh.TrendingArtists(2))
        {
            TrendingArtists.Add(x);
        }
        foreach (var x in await dh.SuggestedSongs(3))
        {
            SuggestedSongs.Add(x);
        }
        foreach (var x in await dh.SuggestedArtists(2))
        {
            SuggestedArtists.Add(x);
        }
        IsHomePageVisible = true;
    }
    private void UpdateUIQueue()
    {
        SongQueue.Clear();
        foreach(Song s in sq.GetSongs())
        {
            SongQueue.Add(s);
        }
    }
    private async Task ShowSearchResults(string searchTerm)
    {
        try
        {
            switch(SelectedTabIndex)
            {
                case 0:
                    //Song
                    SearchSongs.Clear();
                    foreach (var x in await dh.SearchSongs(searchTerm))
                    {
                        SearchSongs.Add(x);
                    }
                    break;
                case 1:
                    //Artist
                    SearchArtists.Clear();
                    foreach (var x in await dh.SearchItems<Artist>(searchTerm))
                    {
                        SearchArtists.Add(x);
                    }
                    break;
                case 2:
                    //Albums
                    SearchAlbums.Clear();
                    foreach (var x in await dh.SearchAlbums(searchTerm))
                    {
                        SearchAlbums.Add(x);
                    }
                    break;
                case 3:
                    //Playlists
                    SearchPlaylists.Clear();
                    foreach (var x in await dh.SearchItems<Playlist>(searchTerm))
                    {
                        SearchPlaylists.Add(x);
                    }
                    break;
                case 4:
                    //Podcasts
                    break;
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
        isMuted = false;
        isPlaying = false;
        shuffle = false;
        repeat = false;
        SelectedTabIndex = 0;
        Volume = 2;
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
