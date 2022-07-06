using System;
using System.Windows.Input;

namespace WpfApp2.ViewModel
{
    /// <summary>
    /// The class implements the interface for working with commands
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        /// <summary>
        /// CanExecuteChanged event handler
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Method determines if the command can be executed
        /// </summary>
        /// <param name="parameter">Command object</param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Executes command logic
        /// </summary>
        /// <param name="parameter">Command object</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
