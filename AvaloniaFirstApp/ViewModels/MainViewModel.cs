using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvaloniaFirstApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        AccountName = "Tomas Pokorny";
    }
    private string _accountName;
    public string AccountName
    {
        get => _accountName;
        set
        {
            if(_accountName != value)
            {
                _accountName = value;
                OnPropertyChanged();
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
