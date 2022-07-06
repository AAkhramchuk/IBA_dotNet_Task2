using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LinqKit;
using System.Reflection;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    /// <summary>
    /// Procedures and functions repository interface
    /// </summary>
    interface IMovieRepository : IDisposable
    {
        void Create();
        IEnumerable<Movie> InitializePaging(bool initializePredicate = true);
        IEnumerable<Movie> Initial();
        void Dispose(bool disposing);

        IEnumerable<Movie> Next();
        IEnumerable<Movie> Previous();
        IEnumerable<Movie> First();
        IEnumerable<Movie> Last();

        void SQLBulkCopy();

        bool? FindCSVfile();

        string? GetMovieElement(Movie record, string entityName);
    }
}
