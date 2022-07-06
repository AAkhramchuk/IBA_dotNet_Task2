using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp2.Model;

namespace WpfApp2
{
    /// <summary>
    /// Movie model definition
    /// </summary>
    public class MovieView : INotifyPropertyChanged
    {
        private readonly Movie _movie;

        public MovieView(Movie m)
        {
            _movie = m;
        }

        public int ID
        {
            get { return _movie.ID; }
            set
            {
                _movie.ID = value;
                OnPropertyChanged("ID");
            }
        }

        public string? ProducerName
        {
            get { return _movie.ProducerName; }
            set
            {
                _movie.ProducerName = value;
                OnPropertyChanged("ProducerName");
            }
        }

        public string? ProducerSurname
        {
            get { return _movie.ProducerSurname; }
            set
            {
                _movie.ProducerSurname = value;
                OnPropertyChanged("ProducerSurname");
            }
        }

        public string? MovieName
        {
            get { return _movie.MovieName; }
            set
            {
                _movie.MovieName = value;
                OnPropertyChanged("MovieName");
            }
        }
        public int? MovieYear
        {
            get { return _movie.MovieYear; }
            set
            {
                _movie.MovieYear = value;
                OnPropertyChanged("MovieYear");
            }
        }
        public int? MovieRating
        {
            get { return _movie.MovieRating; }
            set
            {
                _movie.MovieRating = value;
                OnPropertyChanged("MovieRating");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
