using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp2.Model
{
    /// <summary>
    /// Model Movie
    /// </summary>
    [Table("Movie")]
    public partial class Movie : INotifyPropertyChanged
    {
        private int _ID;
        private string? _producerName;
        private string? _producerSurname;
        private string? _movieName;
        private int? _movieYear;
        private int? _movieRating;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Getter and setter for the DB field ID
        /// </summary>
        [Key]
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        /// <summary>
        /// Getter and setter for the DB field ProducerName
        /// </summary>
        [MaxLength(50)]
        public string? ProducerName
        {
            get { return _producerName; }
            set
            {
                _producerName = value;
                OnPropertyChanged(nameof(ProducerName));
            }
        }

        /// <summary>
        /// Getter and setter for the DB field ProducerSurname
        /// </summary>
        [MaxLength(50)]
        public string? ProducerSurname
        {
            get { return _producerSurname; }
            set
            {
                _producerSurname = value;
                OnPropertyChanged(nameof(ProducerSurname));
            }
        }

        /// <summary>
        /// Getter and setter for the DB field MovieName
        /// </summary>
        [MaxLength(50)]
        public string? MovieName
        {
            get { return _movieName; }
            set
            {
                _movieName = value;
                OnPropertyChanged(nameof(MovieName));
            }
        }

        /// <summary>
        /// Getter and setter for the DB field MovieYear
        /// </summary>
        public int? MovieYear
        {
            get { return _movieYear; }
            set
            {
                _movieYear = value;
                OnPropertyChanged(nameof(MovieYear));
            }
        }

        /// <summary>
        /// Getter and setter for the DB field MovieRating
        /// </summary>
        public int? MovieRating
        {
            get { return _movieRating; }
            set
            {
                _movieRating = value;
                OnPropertyChanged(nameof(MovieRating));
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
