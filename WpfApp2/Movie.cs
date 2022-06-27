using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    [Table("Movie")]
    public partial class Movie : INotifyPropertyChanged
    {
        private int _ID;
        private string? _Name;
        private int? _Year;
        private int? _Rating;
        private int _producerID;
        [ForeignKey("ProducerID")]
        public virtual Producer Producer { get; set; } = null!;

        [Key]
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged("ProducerID");
            }
        }
        public string? Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Movie name");
            }
        }
        public int? Year
        {
            get { return _Year; }
            set
            {
                _Year = value;
                OnPropertyChanged("Year of creation");
            }
        }
        public int? Rating
        {
            get { return _Rating; }
            set
            {
                _Rating = value;
                OnPropertyChanged("Rating");
            }
        }

        public int ProducerID
        {
            get { return _producerID; }
            set
            {
                _producerID = value;
                OnPropertyChanged("ProducerID");
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
