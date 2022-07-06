using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;

namespace WpfApp2.Model
{
    /// <summary>
    /// Model Movie
    /// </summary>
    [Table("Movie")]
    public partial class Movie : INotifyPropertyChanged, IEnumerable
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
                OnPropertyChanged("ID");
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
                OnPropertyChanged("ProducerName");
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
                OnPropertyChanged("ProducerSurname");
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
                OnPropertyChanged("MovieName");
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
                OnPropertyChanged("MovieYear");
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
                OnPropertyChanged("MovieRating");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Data structure enumerated names 
        /// </summary>
        private IEnumerable Events
        {
            get
            {
                yield return "ID";
                yield return "ProducerName";
                yield return "ProducerSurname";
                yield return "MovieName";
                yield return "MovieYear";
                yield return "MovieRating";
            }
        }

        public IEnumerator GetEnumerator() => Events.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
