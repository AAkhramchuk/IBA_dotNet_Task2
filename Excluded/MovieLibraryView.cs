using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace WpfApp2
{
    public class MovieLibraryView : INotifyPropertyChanged
    {
        private readonly Movie _movie;
        private readonly Producer _producer;

        public MovieLibraryView(Producer p, Movie m)
        {
            _producer = p;
            _movie = m;
        }

        public string? ProducerName
        {
            get { return _producer.Name; }
            set
            {
                _producer.Name = value;
                OnPropertyChanged("Producer name");
            }
        }
        public string? ProducerSurname
        {
            get { return _producer.Surname; }
            set
            {
                _producer.Surname = value;
                OnPropertyChanged("Producer surname");
            }
        }

        public string? MovieName
        {
            get { return _movie.MovieName; }
            set
            {
                _movie.MovieName = value;
                OnPropertyChanged("Movie name");
            }
        }
        public int? MovieYear
        {
            get { return _movie.MovieYear; }
            set
            {
                _movie.MovieYear = value;
                OnPropertyChanged("Year of creation");
            }
        }
        public int? MovieRating
        {
            get { return _movie.MovieRating; }
            set
            {
                _movie.MovieRating = value;
                OnPropertyChanged("Rating");
            }
        }

        //       public string ID
        //       {
        //           get { return _library.ID; }
        //           set
        //           {
        //               _library.ProducerName = value;
        //               OnPropertyChanged("Producer name");
        //           }
        //       }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
