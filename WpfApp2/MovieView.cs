using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2
{
    public class MovieView : INotifyPropertyChanged
    {
        private readonly Movie _movie;

        public MovieView(Movie m)
        {
            _movie = m;
        }

        public string? Name
        {
            get { return _movie.Name; }
            set
            {
                _movie.Name = value;
                OnPropertyChanged("Movie name");
            }
        }
        public int? Year
        {
            get { return _movie.Year; }
            set
            {
                _movie.Year = value;
                OnPropertyChanged("Year of creation");
            }
        }
        public int? Rating
        {
            get { return _movie.Rating; }
            set
            {
                _movie.Rating = value;
                OnPropertyChanged("Rating");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
