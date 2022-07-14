using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfApp2.Interfaces;
using WpfApp2.ApplicationRepository.ModelRepository;
using WpfApp2.ApplicationRepository.SQL_bulk_copy;
using WpfApp2.ApplicationRepository.XMLfile;
using WpfApp2.ApplicationRepository.CSVfile;
using WpfApp2.ApplicationRepository.ExcelFile;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _container;

        public App()
        {
            // Create dependency enjection service collection
            ServiceCollection services = new ();
            // Configure collection
            ConfigureServices(services);
            // Create service collection builder
            _container = services.BuildServiceProvider();
        }

        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services">Service collection</param>
        private void ConfigureServices(ServiceCollection services)
        {
            // Add services to the collection
            services.AddSingleton<Model.Movie>();
            services.AddSingleton<Model.MovieConfiguration>();
            services.AddDbContext<Model.MovieContext>();
            services.AddSingleton<ViewModel.ApplicationViewModel>();
            services.AddSingleton<IMovieRepository, MovieRepository>();
            services.AddSingleton<ISQLbulkCopyRepository, SQLbulkCopyRepository>();
            services.AddSingleton<ICSVfileRepository, CSVfileRepository>();
            services.AddSingleton<IXMLfileRepository, XMLfileRepository>();
            services.AddSingleton<IExcelRepository, ExcelRepository>();
            services.AddSingleton<MainWindow>();
        }

        /// <summary>
        /// OnStartup event
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void OnStartup(object Sender, StartupEventArgs e)
        {
            // ViewModel instance
            var applicationViewModel = _container.GetRequiredService(typeof(ViewModel.ApplicationViewModel));
            // MainWindow instatnce
            var mainWindows = _container.GetRequiredService<MainWindow>()!;
            // Create database context
            mainWindows.DataContext = applicationViewModel;
            // Show main form
            mainWindows.Show();
        }
    }
}
