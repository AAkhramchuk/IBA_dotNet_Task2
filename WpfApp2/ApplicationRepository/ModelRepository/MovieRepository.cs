using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WpfApp2.Interfaces;

namespace WpfApp2.ApplicationRepository.ModelRepository
{
    /// <summary>
    /// Business logic repository for Movie view model
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private readonly Model.MovieContext _db;
        private readonly IXMLfileRepository _XMLfileRepository;
        private readonly IExcelRepository _ExcelRepository;

        private bool _disposed = false;

        // Page information text 
        private string _PageOfPages = "Страница: {0} из {1}";

        // Number of records per page
        public int NumberOfRecPerPage { get; private set; }

        /// <summary>
        /// The number of records for skiping
        /// </summary>
        public int SkipRecords { get; private set; }

        // Total number of pages
        public int TotalNumberOfPages = 0;

        // Total record count, filter predicate is taken into account
        public int RecordCount;
        // The first Movie record ID displayed in the Data grid
        public int FirstRecordID = 0;
        // The first Movie record ID on last page displayed in the Data grid
        public int FirstOnLastPageRecordID = 0;
        // The last Movie record ID displayed in the Data grid
        public int LastRecordID = 0;
        // The first Movie record ID displayed in the Data grid
        public int PagingFirstRecordID = 0;
        // The last Movie record ID displayed in the Data grid
        public int PagingLastRecordID = 0;
        // The index of the page displayed in the Datagrid
        public int PageIndex = 0;

        // A predicate for filtering data
        // Initial value can has any condition
        public Expression<Func<Model.Movie, bool>>? InitialPredicate;
        public Expression<Func<Model.Movie, bool>>? MovieFilterPredicate;

        private IQueryable<Model.Movie>? _PagedLines;

        public MovieRepository(Model.MovieContext movieContext
                               , IXMLfileRepository xmlFileRepository
                               , IExcelRepository excelRepository)
        {
            _db = movieContext;
            _XMLfileRepository = xmlFileRepository;
            _ExcelRepository = excelRepository;
        }

        /// <summary>
        /// Open Database or new one if not exists
        /// </summary>
        /// <returns>Create query for Movie entity</returns>
        public void Create()
        {
            _db.Database.EnsureCreated();

            InitialPredicate = CombineFilters(InitialPredicate
                // Initial specific filter predicate
                , m => m.ID > PagingFirstRecordID && m.ID < PagingLastRecordID);
        }

        /// <summary>
        /// Initial procedure for Data selection
        /// </summary>
        /// <param name="initializePredicate">Initialize predicate or not</param>
        /// <returns>Return the first portion of records for visualization</returns>
        public IQueryable<Model.Movie> InitializePaging(bool initializePredicate = true)
        {
            IQueryable<Model.Movie> basicQuery;
            IQueryable<Model.Movie> allLines;

            // Initiate filter predicate
            MovieFilterPredicate = initializePredicate ? null : MovieFilterPredicate;

            // Read the number of records per page setting
            NumberOfRecPerPage = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfRecPerPage"]);

            // Create query of all records, initial filter predicate is taken into account
            basicQuery = _db.Movies.AsNoTracking();

            // Add Movie filter predicate to query if necessary
            if (MovieFilterPredicate != null)
            {
                allLines = basicQuery.Where(MovieFilterPredicate).OrderBy(m => m.ID);
            }
            else
            {
                allLines = basicQuery.OrderBy(m => m.ID);
            }

            // Set the number of all records in Movie table with filter predicate
            RecordCount = allLines.Count();

            // Create query statement for Movie entity, filter predicate is taken into account
            if (RecordCount > 0)
            {
                // Get the first line ID number on the page
                FirstRecordID = allLines.First().ID - 1;
                // Get the last line ID number on the page
                LastRecordID = allLines.Last().ID + 1;
            }

            // The total number of pages calculation
            if (NumberOfRecPerPage != 0)
            {
                TotalNumberOfPages = (RecordCount + NumberOfRecPerPage - 1) / NumberOfRecPerPage;
            }

            // Create Where condition for Select statement, filter Predicate is taken into account
            if (InitialPredicate != null)
            {
                if (MovieFilterPredicate != null)
                {
                    _PagedLines = basicQuery.Where(InitialPredicate).Where(MovieFilterPredicate).Select(m => m);
                }
                else
                {
                    _PagedLines = basicQuery.Where(InitialPredicate).Select(m => m);
                }
            }
            else
            {
                if (MovieFilterPredicate != null)
                {
                    _PagedLines = basicQuery.Where(MovieFilterPredicate).Select(m => m);
                }
                else
                {
                    _PagedLines = basicQuery.Select(m => m);
                }
            }

            return Initial();
        }

        /// <summary>
        /// Select and return the first portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the first portion of records for visualization</returns>
        public IQueryable<Model.Movie> Initial()
        {
            PageIndex = 0;
            PagingFirstRecordID = FirstRecordID;
            PagingLastRecordID = LastRecordID;

            return _PagedLines.OrderBy(m => m.ID).Take(NumberOfRecPerPage);
        }

        /// <summary>
        /// Combine two Movie filter expressions into new one 
        /// </summary>
        /// <param name="condition1">First filter expression</param>
        /// <param name="condition2">Second filter expression</param>
        /// <returns>A combination of two filter expressions using logical And into new one</returns>
        public Expression<Func<Model.Movie, bool>> CombineFilters(Expression<Func<Model.Movie, bool>>? condition1
                                                                  , Expression<Func<Model.Movie, bool>> condition2)
        {
            Expression<Func<Model.Movie, bool>> condition;
            ParameterExpression parameter1;
            BinaryExpression body;

            condition = condition2;
            if (condition1 != null)
            {
                parameter1 = condition1.Parameters[0];
                if (ReferenceEquals(parameter1, condition2.Parameters[0]))
                {
                    body = Expression.AndAlso(condition1.Body, condition2.Body);
                }
                else
                {
                    body = Expression.AndAlso(condition1.Body, Expression.Invoke(condition2, parameter1));
                }
                condition = Expression.Lambda<Func<Model.Movie, bool>>(body, parameter1);
            }

            return condition;
        }

        /// <summary>
        /// Combine filter predicate with additional condition
        /// </summary>
        /// <param name="condition">Additional condition</param>
        /// <returns>Combined Movie filer predicate</returns>
        public void CombineMovieFilter(Expression<Func<Model.Movie, bool>>? condition = null)
        {
            MovieFilterPredicate = condition;
        }

        /// <summary>
        /// Get the first record ID numbers on the page
        /// </summary>
        /// <returns>The first record ID number</returns>
        public int GetFirstRecordID(ref IQueryable<Model.Movie> pagedLines)
        {
            // Get first record ID number
            return RecordCount > 0 ? pagedLines.OrderBy(m => m.ID).First().ID : 0;
        }

        /// <summary>
        /// Get the last record ID numbers on the page
        /// </summary>
        /// <returns>The last record ID number</returns>
        public int GetLastRecordID(ref IQueryable<Model.Movie> pagedLines)
        {
            // Get last record ID number
            return RecordCount > 0 ? pagedLines.OrderBy(m => m.ID).Last().ID : 0;
        }

        /// <summary>
        /// Select and return the next portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the next portion of records for visualization</returns>
        public IQueryable<Model.Movie> Next(ref IQueryable<Model.Movie> pagedLines)
        {
            PageIndex++;
            if (PageIndex > TotalNumberOfPages - 1)
            {
                PageIndex = TotalNumberOfPages - 1;
                PagingFirstRecordID = GetFirstRecordID(ref pagedLines) - 1;
                PagingLastRecordID = LastRecordID;
            }
            else
            {
                PagingFirstRecordID = GetLastRecordID(ref pagedLines);
                PagingLastRecordID = LastRecordID;
            }

            return _PagedLines.OrderBy(m => m.ID).Take(NumberOfRecPerPage);
        }

        /// <summary>
        /// Select and return the previous portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the previous portion of records for visualization</returns>
        public IQueryable<Model.Movie> Previous(ref IQueryable<Model.Movie> pagedLines)
        {
            if (--PageIndex < 0)
            {
                PageIndex = 0;
                PagingLastRecordID = GetLastRecordID(ref pagedLines) + 1;
                PagingFirstRecordID = FirstRecordID;
            }
            else
            {
                PagingLastRecordID = GetFirstRecordID(ref pagedLines);
                PagingFirstRecordID = FirstRecordID;
            }

            return _PagedLines.OrderByDescending(m => m.ID).Take(NumberOfRecPerPage).OrderBy(m => m.ID);
        }

        /// <summary>
        /// Select and return the first Movie records, global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return first portion of records for visualization</returns>
        public IQueryable<Model.Movie> First()
        {
            PageIndex = 0;
            PagingFirstRecordID = FirstRecordID;
            PagingLastRecordID = LastRecordID;

            return _PagedLines.OrderBy(m => m.ID).Take(NumberOfRecPerPage);
        }

        /// <summary>
        /// Select and return the last Movie records, global filter Predicate is taken into account
        /// </summary>
        /// <returns>return last portion of records for visualization</returns>
        public IQueryable<Model.Movie> Last()
        {
            PageIndex = TotalNumberOfPages - 1;
            PagingFirstRecordID = FirstRecordID;
            PagingLastRecordID = LastRecordID;

            return _PagedLines.OrderByDescending(m => m.ID)
                .Take(RecordCount - (TotalNumberOfPages - 1) * NumberOfRecPerPage)
                .OrderBy(m => m.ID);
        }

        /// <summary>
        /// Create string for visualizing current and total page numbers
        /// </summary>
        /// <returns>Return current and total page numbers as formated string</returns>
        public string PagedNumberDisplay()
        {
            // Page information string
            return string.Format(_PageOfPages, RecordCount == 0 ? 0 : PageIndex + 1, TotalNumberOfPages);
        }

        /// <summary>
        /// Dispose procedure
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Releases the allocated resources for ApplicationViewModel context
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get total record count, filter predicate is taken into account
        /// </summary>
        public void GetRecordParam()
        {
            IQueryable<Model.Movie> movies = _db.Movies.AsNoTracking();
            if (MovieFilterPredicate != null)
            {
                movies = movies.Where(MovieFilterPredicate);
            }
            RecordCount = movies.Select(m => m).Count();
        }

        /// <summary>
        /// Calculate total number of pages for displaying, fill global variable 
        /// </summary>
        public void CountMoviePages()
        {
            TotalNumberOfPages = (RecordCount + NumberOfRecPerPage - 1) / NumberOfRecPerPage;
        }

        /// <summary>
        /// Create XML file
        /// </summary>
        public void CreateXML()
        {
            // Create XML document and save it to the file
            _XMLfileRepository.CreateXML(MovieFilterPredicate);
        }

        /// <summary>
        /// Output Movie data into Excel file, global filter predicate is taken into account
        /// </summary>
        public void CreateExcelFile()
        {
            // Create XML document and save it to the file
            _ExcelRepository.CreateExcelFile(MovieFilterPredicate, _db);
        }
    }
}
