using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq.Expressions;
using System;
using System.Linq;
using WpfApp2.Interfaces;

namespace WpfApp2.ViewModel
{
    /// <summary>
    /// Application view model
    /// </summary>
    public partial class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly IMovieRepository _db;
        private readonly ICSVfileRepository _CSVfileRepository;
        private readonly ISQLbulkCopyRepository _SQLBulkCopyRepository;

        // Status information texts
        private const string _StatusFilterActive = "Фильтр установлен";
        private const string _StatusFilterDisabled = "Фильтр снят";
        private string _StatusXMLoutput = "Выгрузка файла {0} в формат XML завершена.";
        private string _StatusExcelOutput = "Выгрузка файла {0} в Excel завершена.";

        // Data grid data collection
        private ObservableCollection<Model.Movie>? _movieLines;
        // Current page number
        private string? _pagedNumber;

        // Relay commands variables
        private RelayCommand? _filterButton;
        private RelayCommand? _bulkCopySQL;
        private RelayCommand? _nextButton;
        private RelayCommand? _previousButton;
        private RelayCommand? _firstButton;
        private RelayCommand? _lastButton;
        private RelayCommand? _createXML;
        private RelayCommand? _createExcel;

        // Filter controls variables
        private int _IDtextBox;
        private string? _ProducerNameTextBox;
        private string? _ProducerSurnameTextBox;
        private string? _MovieNameTextBox;
        private int? _MovieYearTextBox;
        private int? _MovieRatingTextBox;
        private string? _StatusTextBlock;

        private IQueryable<Model.Movie>? _PagedLines;

        // Property changed event handler
        public event PropertyChangedEventHandler? PropertyChanged;

        public ApplicationViewModel(IMovieRepository movieRepository
                                    , ICSVfileRepository csvFileRepository
                                    , ISQLbulkCopyRepository sqlBulkCopyRepository)
        {
            _db = movieRepository;
            _CSVfileRepository = csvFileRepository;
            _SQLBulkCopyRepository = sqlBulkCopyRepository;

            // Create or open database
            _db.Create();

            // Selection of initial records from data grid
            InitializePaging();
        }


        /// <summary>
        /// Getter and setter for ID TextBox field
        /// </summary>
        public int IDtextBox
        {
            get { return _IDtextBox; }
            set
            {
                _IDtextBox = value;
                OnPropertyChanged(nameof(IDtextBox));
            }
        }

        /// <summary>
        /// Getter and setter for ProducerName TextBox field
        /// </summary>
        public string? ProducerNameTextBox
        {
            get { return _ProducerNameTextBox; }
            set
            {
                _ProducerNameTextBox = value;
                OnPropertyChanged(nameof(ProducerNameTextBox));
            }
        }

        /// <summary>
        /// Getter and setter for ProducerSurname TextBox field
        /// </summary>
        public string? ProducerSurnameTextBox
        {
            get { return _ProducerSurnameTextBox; }
            set
            {
                _ProducerSurnameTextBox = value;
                OnPropertyChanged(nameof(ProducerSurnameTextBox));
            }
        }

        /// <summary>
        /// Getter and setter for MovieName TextBox field
        /// </summary>
        public string? MovieNameTextBox
        {
            get { return _MovieNameTextBox; }
            set
            {
                _MovieNameTextBox = value;
                OnPropertyChanged(nameof(MovieNameTextBox));
            }
        }

        /// <summary>
        /// Getter and setter for MovieYear TextBox field
        /// </summary>
        public int? MovieYearTextBox
        {
            get { return _MovieYearTextBox; }
            set
            {
                _MovieYearTextBox = value;
                OnPropertyChanged(nameof(MovieYearTextBox));
            }
        }

        /// <summary>
        /// Getter and setter for MovieRating TextBox field
        /// </summary>
        public int? MovieRatingTextBox
        {
            get { return _MovieRatingTextBox; }
            set
            {
                _MovieRatingTextBox = value;
                OnPropertyChanged(nameof(MovieRatingTextBox));
            }
        }

        /// <summary>
        /// Getter and setter for Status string TextBox field
        /// </summary>
        public string? StatusTextBlock
        {
            get { return _StatusTextBlock; }
            set
            {
                _StatusTextBlock = value;
                OnPropertyChanged(nameof(StatusTextBlock));
            }
        }

        /// <summary>
        /// Getter and setter for Movie entity
        /// </summary>
        public ObservableCollection<Model.Movie>? MovieLines
        {
            get { return _movieLines; }
            set { SetProperty(ref _movieLines, value); }
        }

        /// <summary>
        /// Getter and setter for PageNumber
        /// </summary>
        public string? PagedNumber
        {
            get { return _pagedNumber; }
            set
            {
                _pagedNumber = value;
                OnPropertyChanged(nameof(PagedNumber));
            }
        }

        /// <summary>
        /// Procedure for initializing Datagrid and calculate page number,
        /// filter Predicate is taken into account.
        /// </summary>
        /// <param name="initializePredicate">Condition for predicate initialization</param>
        public void InitializePaging(bool initializePredicate = true)
        {
            _PagedLines = _db.InitializePaging(initializePredicate);
            MovieLines = new(_PagedLines);
            PagedNumber = _db.PagedNumberDisplay();
        }

        /// <summary>
        /// Command execute by Filter button. Filte Predicate definition
        /// </summary>
        public RelayCommand FilterButton
        {
            get
            {
                return _filterButton ??= new RelayCommand(obj =>
                {
                    Expression<Func<Model.Movie, bool>>? condition = null;
                    Expression<Func<Model.Movie, bool>>? predicate = null;

                    // Create Movie filter conditions for each data field
                    if (IDtextBox != 0)
                    {
                        condition = (Model.Movie m) => m.ID == IDtextBox;
                        predicate = _db.CombineFilters(predicate, condition);
                    }
                    if (ProducerNameTextBox != null)
                    {
                        condition = (Model.Movie m) => m.ProducerName == ProducerNameTextBox;
                        predicate = _db.CombineFilters(predicate, condition);
                    }
                    if (_ProducerSurnameTextBox != null)
                    {
                        condition = (Model.Movie m) => m.ProducerSurname == ProducerSurnameTextBox;
                        predicate = _db.CombineFilters(predicate, condition);
                    }
                    if (_MovieNameTextBox != null)
                    {
                        condition = (Model.Movie m) => m.MovieName == MovieNameTextBox;
                        predicate = _db.CombineFilters(predicate, condition);
                    }
                    if (_MovieYearTextBox != null)
                    {
                        condition = (Model.Movie m) => m.MovieYear == MovieYearTextBox;
                        predicate = _db.CombineFilters(predicate, condition);
                    }
                    if (_MovieRatingTextBox != null)
                    {
                        condition = (Model.Movie m) => m.MovieRating == MovieRatingTextBox;
                        predicate = _db.CombineFilters(predicate, condition);
                    }

                    // Add filter condition to Movie filter predicate
                    _db.CombineMovieFilter(predicate);

                    // Get data and calculate page number, filter predicate is taken into account
                    bool initializePredicate = false;
                    InitializePaging(initializePredicate);

                    // Show status information text
                    if (predicate != null)
                    {
                        StatusTextBlock = _StatusFilterActive;
                    }
                    else
                    {
                        StatusTextBlock = _StatusFilterDisabled;
                    }
                });
            }
        }

        /// <summary>
        /// Command execute by Load button. Load data into DB from CSV file
        /// </summary>
        public RelayCommand LoadButton
        {
            get
            {
                return _bulkCopySQL ??= new RelayCommand(obj =>
                {
                    if (_CSVfileRepository.FindCSVfile(out string filePath) == true)
                    {
                        // Bulk copy CSV file's data into DB
                        _SQLBulkCopyRepository.SQLBulkCopy(filePath);

                        // Get and calculate global paramaters
                        _db.GetRecordParam();
                        _db.CountMoviePages();

                        // Data grid initialization
                        bool initializePredicate = false;
                        InitializePaging(initializePredicate);
                    }
                });
            }
        }

        /// <summary>
        /// Command execute by Next button. Select the next portion of data from DB
        /// </summary>
        public RelayCommand NextButton
        {
            get
            {
                return _nextButton ??= new RelayCommand(obj =>
                {
                    // Select the next portion of record from DB
                    _PagedLines = _db.Next(ref _PagedLines);
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = _PagedLines.Count() == 0 ? MovieLines : new(_PagedLines);
                    // Calculate page numbers
                    PagedNumber = _db.PagedNumberDisplay();
                });
            }
        }

        /// <summary>
        /// Command executed by Previouse button. Select the previouse portion of data from DB
        /// </summary>
        public RelayCommand PreviousButton
        {
            get
            {
                return _previousButton ??= new RelayCommand(obj =>
                {
                    // Select the previous portion of record from DB
                    _PagedLines = _db.Previous(ref _PagedLines);
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = _PagedLines.Count() == 0 ? MovieLines : new(_PagedLines);
                    // Calculate page numbers
                    PagedNumber = _db.PagedNumberDisplay();
                });
            }
        }

        /// <summary>
        /// Command executed by First button. Select the first portion of data from DB
        /// </summary>
        public RelayCommand FirstButton
        {
            get
            {
                return _firstButton ??= new RelayCommand(obj =>
                {
                    // Select the first portion of record from DB
                    _PagedLines = _db.First();
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = _PagedLines.Count() == 0 ? MovieLines : new(_PagedLines);
                    // Calculate page numbers
                    PagedNumber = _db.PagedNumberDisplay();
                });
            }
        }

        /// <summary>
        /// Command executed by Last button. Select the last portion of data from DB
        /// </summary>
        public RelayCommand LastButton
        {
            get
            {
                return _lastButton ??= new RelayCommand(obj =>
                {
                    // Select the last portion of record from DB
                    _PagedLines = _db.Last();
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = _PagedLines.Count() == 0 ? MovieLines : new(_PagedLines);
                    // Calculate page numbers
                    PagedNumber = _db.PagedNumberDisplay();
                });
            }
        }

        /// <summary>
        /// Command executed by Create XML button. Create new XML document to the file
        /// </summary>
        public RelayCommand CreateXMLbutton
        {
            get
            {
                return _createXML ??= new RelayCommand(obj =>
                {
                    // Create XML document and save it to the file
                    _db.CreateXML();
                    // Show status message
                    var fileName = ConfigurationManager.ConnectionStrings["OutputXML"];
                    StatusTextBlock = string.Format(_StatusXMLoutput, fileName);
                });
            }
        }

        /// <summary>
        /// Command executed by Create XML button. Create new Excel document to the file
        /// </summary>
        public RelayCommand CreateExcelbutton
        {
            get
            {
                return _createExcel ??= new RelayCommand(obj =>
                {
                    // Create Excel document and save it to the file
                    _db.CreateExcelFile();
                    // Show status message
                    var fileName = ConfigurationManager.ConnectionStrings["OutputExcel"];
                    StatusTextBlock = string.Format(_StatusExcelOutput, fileName);
                });
            }
        }

        /// <summary>
        /// Raise OnPropertyChanged event if property is changed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Event OnPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}