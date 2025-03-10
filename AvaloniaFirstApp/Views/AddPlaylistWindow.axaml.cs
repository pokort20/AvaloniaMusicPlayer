using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaFirstApp.Models;

namespace AvaloniaFirstApp.Views;

public partial class AddPlaylistWindow : Window
{
    public Playlist? Result { get; private set; }
    public AddPlaylistWindow()
    {
        InitializeComponent();
    }
    public void CloseWithResult(Playlist playlist)
    {
        Result = playlist;
        Close();
    }
}