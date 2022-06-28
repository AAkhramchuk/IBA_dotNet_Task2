using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    public class MovieLibraryRepository : IRepository
    {
        private readonly MovieLibraryContext _db;

        public MovieLibraryRepository()
        {
            _db = new MovieLibraryContext();
        }

        public IEnumerable<Producer> GetProducerList()
        {
            return _db.Producers;
        }

        public IEnumerable<Movie> GetMovieList()
        {
            return _db.Movies;
        }

        public Producer GetProducer(int id)
        {
            return _db.Producers.Find(id);
        }

        public Movie GetMovie(int id)
        {
            return _db.Movies.Find(id);
        }

        public void Create()
        {
        //    _db.Producers.Add(library);
        //    _db.Movies.Add(library);


            // this is for demo purposes only, to make it easier
            // to get up and running
            _db.Database.EnsureCreated();

            // load the entities into EF Core
            _db.Producers.Load();
            _db.Movies.Load();
        }

        public void Add(Producer producer, Movie movie)
        {
            
            _db.Producers.Add(producer);
            producer = _db.Producers.Include(o => o.Movies).SingleOrDefault() ?? null!;
            if (producer != null)
                producer.Movies.Add(movie);
        }

        //public void UpdateMovie(Movie movie)
        //{
        //    _db.Entry(movie).State = EntityState.Modified;
        //}

        public void Delete(int id)
        {
            Movie? movie = _db.Movies.Find(id);
            if (movie != null)
            {
                Producer producer = _db.Producers.Find(movie.ProducerID) ?? null!;
                _db.Movies.Remove(movie);
                _db.Producers.Remove(producer);
            }
        }

        public void DeleteAll()
        {
            _db.Movies.RemoveRange(_db.Movies.Local);
            _db.Producers.RemoveRange(_db.Producers.Local);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// CSV file parsering
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Enumerated string block for db upload</returns>
        public IEnumerable<string[]> ParseCSV(string fileName)
        {
            using (var file = new System.IO.StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    yield return line.Split('\t');
                }
            }
        }
    }
}
