using System.Collections.Generic;
using System.Data.SqlClient;

namespace WpfApp2.Interfaces
{
    /// <summary>
    /// Routines for bulk copy data to database
    /// </summary>
    public interface ISQLbulkCopyRepository
    {
        /// <summary>
        /// Call procedure for initial data uploading from file using bulk copy procedure
        /// </summary>
        /// <param name="filePath">Path to CSV file</param>
        void SQLBulkCopy(string filePath);

        /// <summary>
        /// Database fields mapping with internal structure from Movie class and bulk copy into DB
        /// </summary>
        /// <param name="lineCSV"></param>
        /// <param name="connection"></param>
        /// <returns>Empty Task</returns>
        void InsertDataUsingSQLBulkCopy(IEnumerable<Model.Movie> lineCSV
                                        , SqlConnection connection);
    }
}
