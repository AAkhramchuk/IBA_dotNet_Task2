using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using WpfApp2.Interfaces;

namespace WpfApp2.ApplicationRepository.XMLfile
{
    /// <summary>
    /// XML file routines
    /// </summary>
    public class XMLfileRepository : IXMLfileRepository
    {
        // Dependency injection service MovieContext
        private readonly Model.MovieContext _db;

        // Settings
        private string _FileName = ConfigurationManager.ConnectionStrings["OutputXML"].ToString();
        private string _RootNodeNameXML = ConfigurationManager.AppSettings["RootNodeNameXML"]!;
        private string _ParentNodeName = ConfigurationManager.AppSettings["ParentNodeNameXML"]!;
        private string _ParentAttributeName = ConfigurationManager.AppSettings["ParentAttributeNameXML"]!;
        private const string _ExceptionUnknownRecord = @$"Не найден элемент ""Record""";

        public XMLfileRepository(Model.MovieContext movieContext)
        {
            _db = movieContext;
        }

        /// <summary>
        /// Create new XML file
        /// </summary>
        /// <param name="predicate">Fiter predicate</param>
        public void CreateXML(Expression<Func<Model.Movie, bool>>? predicate)
        {
            // Create path to XML file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _FileName);
            using (XmlWriter writerXML = XmlWriter.Create(filePath))
            {
                // Add root node to XML file
                writerXML.WriteStartElement(_RootNodeNameXML);
                // Get enumerated parent nodes with corresponding siblings
                IQueryable<Model.Movie> movies = _db.Movies.AsNoTracking();
                if (predicate != null)
                {
                    movies = movies.Where(predicate);
                }
                movies = movies.Select(m => m);
                foreach (XElement element in NodeXMLgenerator(movies))
                {
                    // Add parent node with sibling nodes to XML document
                    element.WriteTo(writerXML);
                }
                writerXML.WriteEndElement();
            }
        }

        /// <summary>
        /// Use Movie database records to create enumarable XML nodes
        /// </summary>
        /// <param name="data">Movie database set</param>
        /// <returns>Enumerable XML nodes</returns>
        /// <exception cref="ApplicationException"></exception>
        public IEnumerable<XElement> NodeXMLgenerator(IQueryable<Model.Movie> data)
        {
            XElement? parentNode;
            
            // Get all the readable properties for the class
            // Add elements name for XML file
            var entityAccessors = typeof(Model.Movie)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead)
                .Select(p => new
                {
                    Name = p.Name,
                    XMLname = ConfigurationManager.AppSettings[p.Name] ?? p.Name
                });

            foreach (var record in data)
            {
                // Create parent node
                parentNode = new(_ParentNodeName);

                foreach (var property in entityAccessors)
                {
                    var dataProperty = record.GetType().GetProperty(property.Name);
                    if (dataProperty != null)
                    {
                        var dataValue = dataProperty.GetValue(record);
                        // Check required element name from Movie entity
                        if (property.Name.ToLower() == _ParentAttributeName)
                        {
                            // Create parent node attribute
                            parentNode.SetAttributeValue(_ParentAttributeName, dataValue);
                        }
                        else
                        {
                            // Add parent sibling
                            parentNode.Add(new XElement(property.XMLname, dataValue));
                        }
                    }
                }

                if (!parentNode.HasElements)
                {
                    throw new ApplicationException(_ExceptionUnknownRecord + _ParentAttributeName);
                }

                yield return parentNode;
            }
        }
    }
}
