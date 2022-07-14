using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2
{
    public class ProducerView : INotifyPropertyChanged
    {
        private readonly Producer _producer;

        public ProducerView(Producer p)
        {
            _producer = p;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}