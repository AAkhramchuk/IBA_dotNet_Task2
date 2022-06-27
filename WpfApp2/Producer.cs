using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    [Table("Producer")]
    public partial class Producer : INotifyPropertyChanged
    {
        private int _ID;
        private string? _Name;
        private string? _Surname;

        private ICollection<Movie> _Movies = null!;

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
                OnPropertyChanged("Producer name");
            }
        }
        public string? Surname
        {
            get { return _Surname; }
            set
            {
                _Surname = value;
                OnPropertyChanged("Producer surname");
            }
        }
        public virtual ICollection<Movie> Movies
        {
            get { return _Movies; }
            private set
            {
                _Movies = new ObservableCollection<Movie>();
                OnPropertyChanged("Movie relation");
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
