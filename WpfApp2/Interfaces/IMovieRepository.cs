using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace WpfApp2.Interfaces
{
    /// <summary>
    /// Business logic repository interface for Movie view model
    /// </summary>
    public interface IMovieRepository : IDisposable
    {
        /// <summary>
        /// Open Database or new one if not exists
        /// </summary>
        void Create();

        void Dispose(bool disposing);

        /// <summary>
        /// Initial procedure for Data selection
        /// </summary>
        /// <param name="initializePredicate">Initialize predicate or not</param>
        /// <returns>Return the first portion of records for visualization</returns>
        IQueryable<Model.Movie> InitializePaging(bool initializePredicate = true);

        /// <summary>
        /// Select and return the first portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the first portion of records for visualization</returns>
        IQueryable<Model.Movie> Initial();
        /// <summary>
        /// Select and return the next portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the next portion of records for visualization</returns>
        IQueryable<Model.Movie> Next(ref IQueryable<Model.Movie> pagedLines);
        /// <summary>
        /// Select and return the previous portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the previous portion of records for visualization</returns>
        IQueryable<Model.Movie> Previous(ref IQueryable<Model.Movie> pagedLines);
        /// <summary>
        /// Select and return the first Movie records, global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return first portion of records for visualization</returns>
        IQueryable<Model.Movie> First();
        /// <summary>
        /// Select and return the last Movie records, global filter Predicate is taken into account
        /// </summary>
        /// <returns>return last portion of records for visualization</returns>
        IQueryable<Model.Movie> Last();

        /// <summary>
        /// Create string for visualizing current and total page numbers
        /// </summary>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="totalPageNumber">Total pages number</param>
        /// <returns>Return current and total page numbers as formated string</returns>
        string PagedNumberDisplay();

        /// <summary>
        /// Combine two Movie filter expressions into new one 
        /// </summary>
        /// <param name="condition1">First filter nullable expression</param>
        /// <param name="condition2">Second filter expression</param>
        /// <returns>A combination of two filter expressions using logical And into new one</returns>
        Expression<Func<Model.Movie, bool>> CombineFilters(Expression<Func<Model.Movie, bool>>? condition1
                                                           , Expression<Func<Model.Movie, bool>> condition2);

        /// <summary>
        /// Combine filter predicate with additional condition
        /// </summary>
        /// <param name="condition">Additional condition</param>
        /// <returns>Combined Movie filer predicate</returns>
        void CombineMovieFilter(Expression<Func<Model.Movie, bool>>? condition = null);

        /// <summary>
        /// Get the first record ID numbers 
        /// </summary>
        /// <param name="pagedLines">Basic query</param>
        /// <returns>The first record ID number</returns>
        int GetFirstRecordID(ref IQueryable<Model.Movie> pagedLines);

        /// <summary>
        /// Get the last record ID numbers 
        /// </summary>
        /// <param name="pagedLines">Basic query</param>
        /// <returns>The last record ID number</returns>
        int GetLastRecordID(ref IQueryable<Model.Movie> pagedLines);

        /// <summary>
        /// Get total record count, filter predicate is taken into account
        /// </summary>
        void GetRecordParam();

        /// <summary>
        /// Calculate total number of pages for displaying, fill global variable 
        /// </summary>
        void CountMoviePages();

        /// <summary>
        /// New XML file creation based on Movie table in the database
        /// </summary>
        void CreateXML();

        /// <summary>
        /// Output Movie data into Excel file, global filter predicate is taken into account
        /// </summary>
        void CreateExcelFile();
    }
}
