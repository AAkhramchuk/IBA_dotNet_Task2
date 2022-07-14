using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using WpfApp2.Interfaces;

namespace WpfApp2.ApplicationRepository.CSVfile
{
    /// <summary>
    /// CSV file routines
    /// </summary>
    public class CSVfileRepository : ICSVfileRepository
    {
        /// <summary>
        /// User dialog for CSV file selection
        /// </summary>
        /// <param name="filePath">Path to CSV file, output parameter</param>
        /// <returns>If file is selected returns true</returns>
        public bool? FindCSVfile(out string filePath)
        {
            OpenFileDialog dialog = new()
            {
                Title = @"Выберите файл для загрузки ""Видеотеки""",
                DefaultExt = ".csv",
                Filter = "CSV Files (*.csv)|*.csv"
            };

            bool? result = dialog.ShowDialog();
            filePath = dialog.FileName;

            return result;
        }

        /// <summary>
        /// CSV file parser
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Enumerate string array for DB uploading</returns>
        public IEnumerable<string[]> ReaderCSV(string fileName)
        {
            using (var file = new StreamReader(fileName))
            {
                string? line;
                while ((line = file.ReadLine()) != null)
                {
                    yield return line.Split(';');
                }
            }
        }
    }
}
