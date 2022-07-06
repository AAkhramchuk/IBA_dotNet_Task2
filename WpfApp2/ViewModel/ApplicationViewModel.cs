using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Text;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows;
using System.Data;
using LinqKit;
using System.Windows.Input;
using System.Configuration;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    /// <summary>
    /// Application view model
    /// </summary>
    public partial class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly MovieRepository _db;

        private ObservableCollection<Movie>? _movieLines;
        private string? _pagedNumber;

        private RelayCommand? _filterButton;
        private RelayCommand? _bulkCopySQL;
        private RelayCommand? _nextButton;
        private RelayCommand? _previousButton;
        private RelayCommand? _firstButton;
        private RelayCommand? _lastButton;
        private RelayCommand? _createXML;
        private RelayCommand? _createExcel;

        private int _IDtextBox;
        private string? _ProducerNameTextBox;
        private string? _ProducerSurnameTextBox;
        private string? _MovieNameTextBox;
        private int? _MovieYearTextBox;
        private int? _MovieRatingTextBox;
        private string? _StatusTextBlock;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ApplicationViewModel()
        {
            _db = new();

            _db.Create();

            InitializePaging();
        }

        /// <summary>
        /// Create string for visualizing current and total page numbers
        /// </summary>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="totalPageNumber">Total pages number</param>
        /// <returns>Return current and total page numbers as formated string</returns>
        public string PagedNumberDisplay(int pageIndex, int totalPageNumber)
        {
            return string.Format($"Страница: {pageIndex + 1} из {totalPageNumber}");
        }

        /// <summary>
        /// Procedure for initializing Datagrid and calculate page number,
        /// filter Predicate is taken into account.
        /// </summary>
        /// <param name="initializePredicate">Condition for predicate initialization</param>
        public void InitializePaging(bool initializePredicate = true)
        {
            MovieLines = new(_db.InitializePaging(initializePredicate));
            PagedNumber = PagedNumberDisplay(_db.PageIndex, _db.TotalNumberOfPages);
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
                    _db.Predicate = PredicateBuilder.New<Movie>().And(f => true);
                    if (_IDtextBox != 0)
                    {
                        _db.Predicate = _db.Predicate.And(m => m.ID == _IDtextBox);
                    }
                    if (_ProducerNameTextBox != null)
                    {
                        _db.Predicate = _db.Predicate.And(m => m.ProducerName == _ProducerNameTextBox);
                    }
                    if (_ProducerSurnameTextBox != null)
                    {
                        _db.Predicate = _db.Predicate.And(m => m.ProducerSurname == _ProducerSurnameTextBox);
                    }
                    if (_MovieNameTextBox != null)
                    {
                        _db.Predicate = _db.Predicate.And(m => m.MovieName == _MovieNameTextBox);
                    }
                    if (_MovieYearTextBox != null)
                    {
                        _db.Predicate = _db.Predicate.And(m => m.MovieYear == _MovieYearTextBox);
                    }
                    if (_MovieRatingTextBox != null)
                    {
                        _db.Predicate = _db.Predicate.And(m => m.MovieRating == _MovieRatingTextBox);
                    }
                    // Procedure for initializing Datagrid and calculate page number
                    bool initializePredicate = false;
                    InitializePaging(initializePredicate);

                    StatusTextBlock = "Фильтр установлен";
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
                    if (_db.FindCSVfile() == true)
                    {
                        // Bulk copy CSV file's data into DB
                        _db.SQLBulkCopy();
                        // Procedure for initializing Datagrid
                        // and page number calculation
                        InitializePaging(false);
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
                    ObservableCollection<Movie> movieLines;

                    // Select the next portion of record from DB
                    movieLines = new(_db.Next());
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = movieLines.Count == 0 ? MovieLines : movieLines;
                    // Calculate page numbers
                    PagedNumber = PagedNumberDisplay(_db.PageIndex, _db.TotalNumberOfPages);
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
                    ObservableCollection<Movie> movieLines;

                    // Select the previous portion of record from DB
                    movieLines = new(_db.Previous());
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = movieLines.Count == 0 ? MovieLines : movieLines;
                    // Calculate page numbers
                    PagedNumber = PagedNumberDisplay(_db.PageIndex, _db.TotalNumberOfPages);
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
                    ObservableCollection<Movie> movieLines;

                    // Select the first portion of record from DB
                    movieLines = new(_db.First());
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = movieLines.Count == 0 ? MovieLines : movieLines;
                    // Calculate page numbers
                    PagedNumber = PagedNumberDisplay(_db.PageIndex, _db.TotalNumberOfPages);
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
                    ObservableCollection<Movie> movieLines;

                    // Select the last portion of record from DB
                    movieLines = new(_db.Last());
                    // Do not change Data grid lines if new portion of records is empty
                    MovieLines = movieLines.Count == 0 ? MovieLines : movieLines;
                    // Calculate page numbers
                    PagedNumber = PagedNumberDisplay(_db.PageIndex, _db.TotalNumberOfPages);
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
                    StatusTextBlock = @$"Выгрузка файла {fileName} в формат XML завершена.";
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
                    StatusTextBlock = @$"Выгрузка файла {fileName} в Excel завершена.";
                });
            }
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
        public ObservableCollection<Movie>? MovieLines
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
        /// Event OnPropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    }
}