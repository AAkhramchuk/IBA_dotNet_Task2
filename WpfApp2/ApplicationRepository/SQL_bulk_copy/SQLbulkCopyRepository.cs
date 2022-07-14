using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using WpfApp2.Interfaces;

namespace WpfApp2.ApplicationRepository.SQL_bulk_copy
{
    /// <summary>
    /// Routines for bulk copy data to database
    /// </summary>
    public class SQLbulkCopyRepository : ISQLbulkCopyRepository
    {
        Model.MovieContext _MovieContext;
        ICSVfileRepository _CSVfileRepository;

        public SQLbulkCopyRepository(Model.MovieContext movieContext, ICSVfileRepository csvFileRepository)
        {
            _MovieContext = movieContext;
            _CSVfileRepository = csvFileRepository;
        }

        /// <summary>
        /// Call procedure for initial data uploading from file using bulk copy procedure
        /// </summary>
        /// <param name="filePath">Path to CSV file</param>
        public void SQLBulkCopy(string filePath)
        {
            // New SQL connection
            string connectionString = _MovieContext.connectionString;
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                // Call procedure for initial data uploading from file using bulk copy procedure
                InsertDataUsingSQLBulkCopy(
                    _CSVfileRepository.ReaderCSV(filePath)
                        .Select(csv => new Model.Movie
                        {
                            ProducerName = csv[0],
                            ProducerSurname = csv[1],
                            MovieName = csv[2],
                            MovieYear = Convert.ToInt32(csv[3] == "" ? 0 : csv[3]),
                            MovieRating = Convert.ToInt32(csv[4] == "" ? 0 : csv[4])
                        })
                        , connection);
            }
        }

        /// <summary>
        /// Database fields mapping with internal structure from Movie class and bulk copy into DB
        /// </summary>
        /// <param name="lineCSV"></param>
        /// <param name="connection"></param>
        /// <returns>Empty Task</returns>
        public void InsertDataUsingSQLBulkCopy(IEnumerable<Model.Movie> lineCSV
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
            using (ObjectDataReader<Model.Movie>? dataReader = new(lineCSV))
            {
                // Fill Database asynchronously 
                bulkCopy.WriteToServer(dataReader);
            }
        }
    }
}
