using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WpfApp2.Interfaces;

namespace WpfApp2.ApplicationRepository.ExcelFile
{
    /// <summary>
    /// Excel file routines
    /// </summary>
    public class ExcelRepository : IExcelRepository
    {
        /// <summary>
        /// Output Movie data into Excel file, global filter predicate is taken into account
        /// </summary>
        public void CreateExcelFile(Expression<Func<Model.Movie, bool>>? predicate, Model.MovieContext db)
        {
            // Create Excel document
            IWorkbook workBook = new XSSFWorkbook();
            // Create sheet
            ISheet sheet = workBook.CreateSheet("Movies");
            IRow row;
            int i, j;

            // Switch off Excel formula recalculation procedure
            sheet.ForceFormulaRecalculation = false;

            // Create new table header row
            row = sheet.CreateRow(0);

            // Get all the readable properties for the class
            // Add elements name for XML file
            IEnumerable<PropertyInfo> entityAccessors = typeof(Model.Movie)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead)
                .Select(p => p);

            // Create new table header cells
            i = 0;
            foreach (var property in entityAccessors)
            {
                row.CreateCell(i).SetCellValue(property.Name);
                i++;
            }

            i = 1;
            foreach (Model.Movie record in ExcelLineGenerator(predicate, db))
            {
                // create new data row
                row = sheet.CreateRow(i);

                j = 0;
                foreach (PropertyInfo property in entityAccessors)
                {
                    var dataProperty = record.GetType().GetProperty(property.Name);
                    if (dataProperty != null)
                    {
                        // get property value
                        var dataValue = dataProperty.GetValue(record);

                        // create new data cells in a row
                        if (dataValue != null)
                        {
                            row.CreateCell(j).SetCellValue(dataValue.ToString());
                            j++;
                        }
                    }
                }
                i++;
            }

            // Save document
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                                           , ConfigurationManager.ConnectionStrings["OutputExcel"].ToString());
            using (var file = File.Create(filePath))
            {
                workBook.Write(file);
            }
            workBook.Close();
        }

        /// <summary>
        /// Create enumerable Movie entity
        /// </summary>
        /// <returns>Movie record</returns>
        public IEnumerable<Model.Movie> ExcelLineGenerator(Expression<Func<Model.Movie, bool>>? predicate
                                                           , Model.MovieContext db)
        {
            // Select data, filter predicate is taken into account
            IQueryable<Model.Movie> movies = db.Movies.AsNoTracking();
            if (predicate != null)
            {
                movies = movies.Where(predicate);
            }
            movies = movies.Select(m => m);
            foreach (var record in movies)
            {
                yield return record;
            }
        }
    }
}
