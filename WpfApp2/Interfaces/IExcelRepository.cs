using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WpfApp2.Interfaces
{
    /// <summary>
    /// Excel file routines
    /// </summary>
    public interface IExcelRepository
    {
        /// <summary>
        /// Output Movie data into Excel file, global filter predicate is taken into account
        /// </summary>
        void CreateExcelFile(Expression<Func<Model.Movie, bool>>? predicate, Model.MovieContext db);

        /// <summary>
        /// Create enumerable Movie entity
        /// </summary>
        /// <returns>Movie record</returns>
        public IEnumerable<Model.Movie> ExcelLineGenerator(Expression<Func<Model.Movie, bool>>? predicate
                                                           , Model.MovieContext db);
    }
}
