using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using LinqToDB.Data;

namespace WpfApp2
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Movie> Movies { get; set; }
        public ObservableCollection<MovieLibrary> MovieLibraryLines { get; set; }

        public ApplicationViewModel()
        {
            //string connectionDB = @ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (MovieLibraryRepository db = new MovieLibraryRepository())// connectionDB))
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

        public void DBupload()
        {
            using (MovieLibraryRepository db = new MovieLibraryRepository())
            {
                var csvItems = db.ParseCSV("filename");

                db.BulkCopy(
                    csvItems.Select(csv => new DestinationEntity
                    {
                       Field1 = int.Parse(csv[0]),
                       Field2 = double.Parse(csv[1]),
                       LongStr = csv[2]
                    })
                );
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
