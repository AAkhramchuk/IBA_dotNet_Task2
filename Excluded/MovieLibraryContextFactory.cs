using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WpfApp2
{
/*
    public class MovieLibraryContextFactory : IDesignTimeDbContextFactory<MovieLibraryContext>
    {
        public MovieLibraryContext CreateDbContext(string[] args)
        {
            //    IConfigurationRoot configuration = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("appsettings.json")
            //        .Build();

            // Here we create the DbContextOptionsBuilder manually.        
            var builder = new DbContextOptionsBuilder<MovieLibraryContext>();

            // Build connection string. This requires that you have a connectionstring in the appsettings.json
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            builder.UseSqlServer(@connectionString);
            // Create our DbContext.
            return new MovieLibraryContext(builder.Options);
        }
    }
*/
}
