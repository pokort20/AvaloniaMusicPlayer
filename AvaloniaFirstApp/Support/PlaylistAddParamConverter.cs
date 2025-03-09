using Avalonia.Data.Converters;
using AvaloniaFirstApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaFirstApp.Support
{
    public class PlaylistCommandParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null)Debug.WriteLine(value.ToString());
            if(parameter != null)Debug.WriteLine(parameter.ToString());
            if (value is Playlist playlist && parameter is Song song)
            {
                return new AddToPlaylistParameter(song, playlist);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
