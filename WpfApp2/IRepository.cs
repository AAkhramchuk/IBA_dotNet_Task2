using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace WpfApp2
{
    interface IRepository : IDisposable
    {
        IEnumerable<Producer> GetProducerList();
        IEnumerable<Movie> GetMovieList();
        Producer GetProducer(int id);
        Movie GetMovie(int id);
        void Create();
        void Add(Producer producer, Movie movie);
        //void Update(Producer producer, Movie movie);
        void Delete(int id);
        void DeleteAll();
        void Save();

    }
}
