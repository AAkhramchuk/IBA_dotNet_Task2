using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WpfApp2
{
    internal class PagingRepository
    {
        public int PageIndex { get; set; }

        DataTable PagedList = new();

        public DataTable SetPaging(IEnumerable<Movie> listToPage, int recordsPerPage)
        {
            int PageGroup = PageIndex * recordsPerPage;

            //IList<Movie> PagedList = new List<Movie>();
            IList<Movie> PagedList = listToPage.Skip(PageGroup).Take(recordsPerPage).ToList();

            return PagedTable(PagedList);
        }

        private DataTable PagedTable<T>(IEnumerable<T> sourceList)
        {
            Type columnType = typeof(T);
            DataTable TableToReturn = new();

            foreach (var Column in columnType.GetProperties())
            {
                TableToReturn.Columns.Add(Column.Name);//, Column.PropertyType);
            }

            foreach (object item in sourceList)
            {
                DataRow ReturnTableRow = TableToReturn.NewRow();
                foreach (var Column in columnType.GetProperties())
                {
                    ReturnTableRow[Column.Name] = Column.GetValue(item);
                }
                TableToReturn.Rows.Add(ReturnTableRow);
            }

            return TableToReturn;
        }

        public DataTable Next(IEnumerable<Movie> listToPage, int recordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= listToPage.Count() / recordsPerPage)
            {
                PageIndex = listToPage.Count() / recordsPerPage;
            }
            PagedList = SetPaging(listToPage, recordsPerPage);

            return PagedList;
        }

        public DataTable Previous(IEnumerable<Movie> listToPage, int recordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(listToPage, recordsPerPage);

            return PagedList;
        }

        public DataTable First(IEnumerable<Movie> listToPage, int recordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(listToPage, recordsPerPage);
            return PagedList;
        }

        public DataTable Last(IEnumerable<Movie> listToPage, int recordsPerPage)
        {
            PageIndex = listToPage.Count() / recordsPerPage;
            PagedList = SetPaging(listToPage, recordsPerPage);
            return PagedList;
        }
    }
}
