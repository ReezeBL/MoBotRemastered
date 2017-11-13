using System;
using System.Windows.Input;

namespace ExcaliburLauncher.GUI.Commands
{
    internal class RelayCommand : RelayCommand<object> {
        public RelayCommand(Action<object> execute) : base(execute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute)
        {
        }
    }

    internal class RelayCommand<T> : ICommand
    {
        private Action<T> execute;

        private Predicate<T> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute != null && canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        public void OnCanExecuteChanged()
        {
            var handler = CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ => { };
        }

        private static bool DefaultCanExecute(T parameter)
        {
            return true;
        }
    }
}
