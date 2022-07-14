using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    public partial class MovieLibrary : INotifyPropertyChanged
    {
//        private int _ID;
        private string? _ProducerName;
        private string? _ProducerSurname;

//        private int _ID;
        private string? _MovieName;
        private int? _MovieYear;
        private int? _MovieRating;
//        private int _producerID;
//        public virtual Producer Producer { get; set; } = null!;

        //        private ICollection<Movie> _Movies = null!;

        //        public int ID
        //        {
        //            get { return _ID; }
        //            set
        //            {
        //                _ID = value;
        //                OnPropertyChanged("ProducerID");
        //            }
        //        }
        public string? ProducerName
        {
            get { return _ProducerName; }
            set
            {
                _ProducerName = value;
                OnPropertyChanged("Producer name");
            }
        }
        public string? ProducerSurname
        {
            get { return _ProducerSurname; }
            set
            {
                _ProducerSurname = value;
                OnPropertyChanged("Producer surname");
            }
        }
        //        public virtual ICollection<Movie> Movies
        //        {
        //            get { return _Movies; }
        //            private set
        //            {
        //                _Movies = new ObservableCollection<Movie>();
        //                OnPropertyChanged("Movie relation");
        //            }
        //        }

//        public int ID
//        {
//            get { return _ID; }
//            set
//            {
//                _ID = value;
//                OnPropertyChanged("ProducerID");
//            }
//        }
        public string? MovieName
        {
            get { return _MovieName; }
            set
            {
                _MovieName = value;
                OnPropertyChanged("Movie name");
            }
        }
        public int? MovieYear
        {
            get { return _MovieYear; }
            set
            {
                _MovieYear = value;
                OnPropertyChanged("Year of creation");
            }
        }
        public int? MovieRating
        {
            get { return _MovieRating; }
            set
            {
                _MovieRating = value;
                OnPropertyChanged("Rating");
            }
        }

//        public int ProducerID
//        {
//            get { return _producerID; }
//            set
//            {
//                _producerID = value;
//                OnPropertyChanged("ProducerID");
//            }
//        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
