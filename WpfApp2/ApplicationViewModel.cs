using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Configuration;

namespace WpfApp2
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Movie> Movies { get; set; }
        public ObservableCollection<MovieLibrary> MovieLibraryLines { get; set; }

        public ApplicationViewModel()
        {
            string connectionDB = @ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            //using (MovieLibraryRepository db = new MovieLibraryRepository(connectionDB))
            using (var db = new MovieLibraryContext(connectionDB))
            {
            }
            /*
            using (MovieLibraryRepository db = new MovieLibraryRepository(connectionDB))
            {
                db.Create();

                db.GetMovieList();
                db.GetProducerList();

                MovieLibraryLines = new ObservableCollection<MovieLibrary>
                {
                    new MovieLibrary { ProducerName = "Эльдар", ProducerSurname = "Рязанов"
                        , MovieName = "Иро́ния судьбы́, и́ли С лёгким па́ром!", MovieYear = 1975, MovieRating = 100 }
                };
                MovieLibraryLines.Add
                (
                    new MovieLibrary { ProducerName = db.GetProducer(1).Name, ProducerSurname = db.GetProducer(1).Surname
                        , MovieName = db.GetMovie(1).Name, MovieYear = db.GetMovie(1).Year, MovieRating = db.GetMovie(1).Rating }
                );
            }
            */


        }

        private MovieLibraryView _selectedLine;

        public MovieLibraryView SelectedLine
        {
            get { return _selectedLine; }
            set
            {
                _selectedLine = value;
                OnPropertyChanged("SelectedLine");
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
