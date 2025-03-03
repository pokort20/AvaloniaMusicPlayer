using Avalonia.Controls;
using System.Diagnostics;

namespace AvaloniaFirstApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Binding(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        Debug.WriteLine("Pointer pressed, e: " + e.GetPosition((Avalonia.Visual)sender).ToString());
    }
}
