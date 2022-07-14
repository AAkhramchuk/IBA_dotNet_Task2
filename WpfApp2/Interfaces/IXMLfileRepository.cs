using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace WpfApp2.Interfaces
{
    public interface IXMLfileRepository
    {
        /// <summary>
        /// Create new XML file
        /// </summary>
        /// <param name="predicate">Fiter predicate</param>
        /// <param name="db">Data source</param>
        void CreateXML(Expression<Func<Model.Movie, bool>>? predicate);

        /// <summary>
        /// Use Movie database records to create enumarable XML nodes
        /// </summary>
        /// <param name="data">Movie database set</param>
        /// <returns>Enumerable XML nodes</returns>
        /// <exception cref="ApplicationException"></exception>
        IEnumerable<XElement> NodeXMLgenerator(IQueryable<Model.Movie> data);
    }
}
