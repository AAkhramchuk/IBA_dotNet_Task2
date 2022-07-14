using System.Collections.Generic;

namespace WpfApp2.Interfaces
{
    /// <summary>
    /// CSV file routines
    /// </summary>
    public interface ICSVfileRepository
    {
        /// <summary>
        /// User dialog for CSV file selection
        /// </summary>
        /// <param name="filePath">Path to CSV file, output parameter</param>
        /// <returns>If file is selected returns true</returns>
        bool? FindCSVfile(out string filePath);

        /// <summary>
        /// CSV file parser
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Enumerate string array for DB uploading</returns>
        IEnumerable<string[]> ReaderCSV(string fileName);
    }
}
