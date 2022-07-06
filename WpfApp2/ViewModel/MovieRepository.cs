using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Win32;
using LinqKit;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WpfApp2.Model;
using WpfApp2.SQL_bulk_copy;

namespace WpfApp2.ViewModel
{
    /// <summary>
    /// Business logic repository for Movie view model
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private bool _disposed = false;

        private readonly MovieContext _db;

        // Number of records per page
        public int NumberOfRecPerPage { get; private set; }

        // Total number of pages
        public int TotalNumberOfPages;

        // Total record count, filter predicate is taken into account
        public int RecordCount;
        // The first Movie record ID displayed in the Datagrid
        public int PagingFirstRecordID = 0;
        // The last Movie record ID displayed in the Datagrid
        public int PagingLastRecordID = 0;
        // The index of the page displayed in the Datagrid
        public int PageIndex = -1;

        // A path to the data source file
        public string _sourceFilePath = string.Empty;

        // A predicate for filtering data
        public ExpressionStarter<Movie>? Predicate;

        public MovieRepository()
        {
            _db = new MovieContext();

            NumberOfRecPerPage = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfRecPerPage"].ToString());
        }

        /// <summary>
        /// Open Database or new one if not exists
        /// </summary>
        public void Create()
        {
            _db.Database.EnsureCreated();
        }

        // <summary>
        // Initial procedure for Data selecting
        // </summary>
        // <returns>Return the first portion of records for visualization</returns>


        /// <summary>
        /// Initial procedure for Data selecting
        /// </summary>
        /// <param name="initializePredicate">Condition for predicate initialization</param>
        /// <returns>Return the first portion of records for visualization</returns>
        public IEnumerable<Movie> InitializePaging(bool initializePredicate = true)
        {
            // Initialize filter Predicate for data visualization
            if (initializePredicate)
            {
                Predicate = PredicateBuilder.New<Movie>().And(f => true);
            }
            // Select total number of records for visualization
            getRecordParam();
            // Count total number of pages
            countMovieRecords();

            return Initial();
        }

        /// <summary>
        /// Select and return the first portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the first portion of records for visualization</returns>
        public IEnumerable<Movie> Initial()
        {
            // Create Where condition for Select statement, filter Predicate is taken into account
            ExpressionStarter<Movie> condition = PredicateBuilder.New<Movie>().And(Predicate);
            condition = condition.And(m => m.ID > 0);

            // Select data from DB
            IEnumerable<Movie> pagedLines = _db.Movies.Where(condition)
                                                      .Select(m => m)
                                                      .Take(NumberOfRecPerPage);
            // Get first and last record ID numbers
            if (pagedLines.Count() > 0)
            {
                PageIndex = 0;
            }
            else PageIndex = -1;
            // Select total number of records for visualization
            getRecordParam();
            // Count total number of pages
            countMovieRecords();
            // Get first and last record ID numbers
            PagingFirstRecordID = pagedLines.FirstOrDefault(new Movie() { ID = 0 }).ID;
            PagingLastRecordID = pagedLines.LastOrDefault(new Movie() { ID = 0 }).ID;

            return pagedLines;
        }

        /// <summary>
        /// Select and return the next portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the next portion of records for visualization</returns>
        public IEnumerable<Movie> Next()
        {
            // Calculate page index for displaing
            PageIndex++;
            if (PageIndex >= TotalNumberOfPages)
            {
                PageIndex--;
            }
            // Create Where condition for Select statement, filter Predicate is taken into account
            int lastRecordID = PagingLastRecordID;
            ExpressionStarter<Movie> condition = PredicateBuilder.New<Movie>().And(Predicate);
            condition = condition.And(m => m.ID > lastRecordID);

            // Select data from DB
            IEnumerable<Movie> pagedLines = _db.Movies.Where(condition)
                                                      .Select(m => m)
                                                      .Take(NumberOfRecPerPage);
            // Get first and last record ID numbers
            if (pagedLines.Count() > 0)
            {
                PagingFirstRecordID = pagedLines.FirstOrDefault(new Movie() { ID = 0 }).ID;
                PagingLastRecordID = pagedLines.LastOrDefault(new Movie() { ID = 0 }).ID;
            }

            return pagedLines;
        }

        /// <summary>
        /// Select and return the previous portion of Movie records,
        /// global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return the previous portion of records for visualization</returns>
        public IEnumerable<Movie> Previous()
        {
            // Calculate page index for displaing
            PageIndex = --PageIndex < 0 ? ++PageIndex : PageIndex;
            // Calculate the number of records for skiping
            int skipRecords = PageIndex < 0 ? 0 : PageIndex * NumberOfRecPerPage;
            // Create Where condition for Select statement, filter Predicate is taken into account
            int firstRecordID = PagingFirstRecordID;
            ExpressionStarter<Movie> condition = PredicateBuilder.New<Movie>().And(Predicate);
            condition = condition.And(m => m.ID < firstRecordID);

            // Select data from DB
            IEnumerable<Movie> pagedLines = _db.Movies.Where(condition)
                                                      .Skip(skipRecords)
                                                      .Select(m => m);
            // Get first and last record ID numbers
            if (pagedLines.Count() > 0)
            {
                PagingFirstRecordID = pagedLines.FirstOrDefault(new Movie() { ID = 0 }).ID;
                PagingLastRecordID = pagedLines.LastOrDefault(new Movie() { ID = 0 }).ID;
            }

            return pagedLines;
        }

        /// <summary>
        /// Select and return the first Movie records, global filter Predicate is taken into account
        /// </summary>
        /// <returns>Return first portion of records for visualization</returns>
        public IEnumerable<Movie> First()
        {
            // Calculate page index for displaing
            PageIndex = 0;
            // Create Where condition for Select statement, filter Predicate is taken into account
            ExpressionStarter<Movie> condition = PredicateBuilder.New<Movie>().And(Predicate);
            condition = condition.And(m => m.ID > 0);

            // Select data from DB
            IEnumerable<Movie> pagedLines = _db.Movies.Where(condition)
                                                      .Select(m => m)
                                                      .Take(NumberOfRecPerPage);
            // Get first and last record ID numbers
            if (pagedLines.Count() > 0)
            {
                PagingFirstRecordID = pagedLines.FirstOrDefault(new Movie() { ID = 0 }).ID;
                PagingLastRecordID = pagedLines.LastOrDefault(new Movie() { ID = 0 }).ID;
            }

            return pagedLines;
        }

        /// <summary>
        /// Select and return the last Movie records, global filter Predicate is taken into account
        /// </summary>
        /// <returns>return last portion of records for visualization</returns>
        public IEnumerable<Movie> Last()
        {
            // Calculate page index for displaing
            PageIndex = TotalNumberOfPages - 1;
            // Calculate the number of records for skiping
            int skipRecords = RecordCount < NumberOfRecPerPage ? 0 : RecordCount - NumberOfRecPerPage;
            // Create Where condition for Select statement, filter Predicate is taken into account
            ExpressionStarter<Movie> condition = PredicateBuilder.New<Movie>().And(Predicate);
            condition = condition.And(m => m.ID > 0);

            // Select data from DB
            IEnumerable<Movie> pagedLines = _db.Movies.Where(condition)
                                                      .Skip(skipRecords)
                                                      .Select(m => m);
            // Get first and last record ID numbers
            if (pagedLines.Count() > 0)
            {
                PagingFirstRecordID = pagedLines.FirstOrDefault(new Movie() { ID = 0 }).ID;
                PagingLastRecordID = pagedLines.LastOrDefault(new Movie() { ID = 0 }).ID;
            }

            return pagedLines;
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
        /// Call procedure for initial data uploading from file using bulk copy procedure
        /// </summary>
        public void SQLBulkCopy()
        {
            string connectionString = @$"{ConfigurationManager.ConnectionStrings["DefaultConnection"]}";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Call procedure for initial data uploading from file using bulk copy procedure
                var _ = InsertDataUsingSQLBulkCopy(
                            ReaderCSV(_sourceFilePath)
                                .Select(csv => new Movie
                                {
                                    ProducerName = csv[0]
                                                         ,
                                    ProducerSurname = csv[1]
                                                         ,
                                    MovieName = csv[2]
                                                         ,
                                    MovieYear = Convert.ToInt32(csv[3] == "" ? 0 : csv[3])
                                                         ,
                                    MovieRating = Convert.ToInt32(csv[4] == "" ? 0 : csv[3])
                                })
                            , connection);
            }

            // Get and calculate global paramaters
            getRecordParam();
            countMovieRecords();
        }

        /// <summary>
        /// Get total record count, filter predicate is taken into account
        /// </summary>
        private void getRecordParam()
        {
            RecordCount = _db.Movies.Where(Predicate)
                                    .Select(m => m)
                                    .Count();
        }

        /// <summary>
        /// Calculate total number of pages for displaying, fill global variable 
        /// </summary>
        private void countMovieRecords()
        {
            TotalNumberOfPages = (RecordCount + NumberOfRecPerPage - 1) / NumberOfRecPerPage;
        }

        /// <summary>
        /// Database fields mapping with internal structure from Movie class and bulk copy into DB
        /// </summary>
        /// <param name="lineCSV"></param>
        /// <param name="connection"></param>
        /// <returns>Empty Task</returns>
        private async static Task InsertDataUsingSQLBulkCopy(IEnumerable<Movie> lineCSV
                                                             , SqlConnection connection)
        {
            // Establish new connection to MS SQL Server Express
            SqlBulkCopy bulkCopy = new(connection);
            bulkCopy.DestinationTableName = "dbo.Movie";
            // Mapping Database fields with internal structure fields for bulk copy into DB
            bulkCopy.ColumnMappings.Add("ProducerName", "ProducerName");
            bulkCopy.ColumnMappings.Add("ProducerSurname", "ProducerSurname");
            bulkCopy.ColumnMappings.Add("MovieName", "MovieName");
            bulkCopy.ColumnMappings.Add("MovieYear", "MovieYear");
            bulkCopy.ColumnMappings.Add("MovieRating", "MovieRating");

            // CSV file reader deffinition
            using (ObjectDataReader<Movie>? dataReader = new(lineCSV))
            {
                // Fill Database asynchronously 
                await bulkCopy.WriteToServerAsync(dataReader);
            }
        }

        /// <summary>
        /// Create user dialog to get path to a CSV file
        /// </summary>
        /// <returns>Dialog result</returns>
        public bool? FindCSVfile()
        {
            OpenFileDialog dialog = new()
            {
                Title = @"Выберите файл для загрузки ""Видеотеки""",
                DefaultExt = ".csv",
                Filter = "CSV Files (*.csv)|*.csv"
            };

            bool? result = dialog.ShowDialog();
            _sourceFilePath = dialog.FileName;

            return result;
        }

        /// <summary>
        /// CSV file parser
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Enumerate string block for DB uploading</returns>
        private IEnumerable<string[]> ReaderCSV(string fileName)
        {
            using (var file = new StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()!) != null)
                {
                    yield return line.Split(';');
                }
            }
        }

        /// <summary>
        /// Create XML file
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CreateXML()
        {
            XElement parentNode;
            XElement siblingNode;
            XDocument xDoc;

            // Select data, take into account filter predicate
            var records = _db.Movies.Where(Predicate).Select(m => m);
            if (records.Any())
            {
                // Create root node
                XElement rootNode = new("TestProgram");

                foreach (var record in records)
                {
                    // Get and create parent node
                    var propertyInfos = record.GetType().GetProperties();
                    if (propertyInfos[0].Name != "ID")
                        throw new Exception(@$"Не найден элемент ""Record id""");
                    var propertyValue = (int?)propertyInfos[0].GetValue(record) ?? 0;
                    parentNode = new("Record", new XAttribute(propertyInfos[0].Name.ToLower()
                                                              , propertyValue.ToString()));
                    rootNode.Add(parentNode);
                    // Parse output stucture by element names and corresponding value types
                    foreach (string item in record)
                    {
                        if (item != "ID")
                        {
                            var getMovieElement = GetMovieElement(record, item);
                            if (getMovieElement != null)
                            {
                                siblingNode = new(item, getMovieElement);

                                // Add parent sibling
                                parentNode.Add(siblingNode);
                            }
                        }
                    }
                }
                // Create new XML document
                xDoc = new();
                // Add root node into the document
                xDoc.Add(rootNode);
                // Save XML document into the file
                var fileName = ConfigurationManager.ConnectionStrings["OutputXML"];
                xDoc.Save(@$"{AppDomain.CurrentDomain.BaseDirectory}{fileName}");
            }
        }

        /// <summary>
        /// Output data into Excel file? take into account global filter predicate
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CreateExcelFile()
        {
            // Create workBook
            IWorkbook workBook = new XSSFWorkbook();
            // Create sheet
            ISheet sheet = workBook.CreateSheet("Movies");
            IRow row;

            // Switch off Excel formula recalculation procedure
            sheet.ForceFormulaRecalculation = false;
            // Select data, take into account filter predicate
            var records = _db.Movies.Where(Predicate).Select(m => m);
            if (records.Any())
            {
                // Create new table header row
                row = sheet.CreateRow(0);
                // Create new table header cells
                int i = 0, j = 0;
                foreach (string item in records.First())
                {
                    row.CreateCell(i).SetCellValue(item);
                    i++;
                }

                i = 1;
                foreach (var record in records)
                {
                    // create new data row
                    row = sheet.CreateRow(i);

                    // create new data cells in a row
                    j = 0;
                    foreach (string item in record)
                    {
                        var getMovieElement = GetMovieElement(record, item);
                        if (getMovieElement != null)
                            row.CreateCell(j).SetCellValue(getMovieElement);
                        j++;
                    }
                    i++;
                }
            }
            // Save document
            var fileName = ConfigurationManager.ConnectionStrings["OutputExcel"];
            using (var f = File.Create(@$"{AppDomain.CurrentDomain.BaseDirectory}{fileName}"))
            {
                workBook.Write(f);
            }
        }

        /// <summary>
        /// Parse output stucture by element names and corresponding value types
        /// </summary>
        /// <param name="record"></param>
        /// <param name="entityName"></param>
        /// <returns> Returns String value or null if something goes wrong</returns>
        public string? GetMovieElement(Movie record, string entityName)
        {
            var propertyInfos = record.GetType().GetProperties();
            foreach (PropertyInfo pInfo in propertyInfos)
            {
                if (pInfo.Name == entityName)
                {
                    var value = pInfo.GetValue(record, null);
                    if (value != null)
                    {
                        return Type.GetTypeCode(value.GetType()!) switch
                        {
                            TypeCode.Int32 => value.ToString()!,
                            TypeCode.String => value.ToString()!,
                            TypeCode.Empty => "",
                            _ => ""
                        };
                    }
                }
            }
            return null;
        }
    }
}
