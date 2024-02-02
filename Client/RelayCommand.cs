using System;
using System.Windows.Input;

namespace Monopoly
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        public RelayCommand(Action<object> execute) : this(null, execute) { }

        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            if (execute == null) throw new MonopolyException($"Parameter {nameof(execute)} can't be null at {nameof(RelayCommand)}");

            this.canExecute = canExecute;
            this.execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}