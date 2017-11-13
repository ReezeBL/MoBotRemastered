using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoBot.GUI.Commands
{
    internal class AsyncCommand : AsyncCommand<object>
    {
        public AsyncCommand(Func<Task> executeMethod) : base(executeMethod)
        {
        }

        public AsyncCommand(Func<object, Task> executeMethod, Predicate<object> canExecuteMethod) : base(executeMethod, canExecuteMethod)
        {
        }
    }

    internal class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, Task> executeMethod;
        private readonly RelayCommand<T> underlyingCommand;
        private bool isExecuting;

        public AsyncCommand(Func<Task> executeMethod) : this(o => executeMethod(), _ => true)
        {
        }

        public AsyncCommand(Func<T, Task> executeMethod, Predicate<T> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            underlyingCommand = new RelayCommand<T>(x => { }, canExecuteMethod);
        }

        public async Task ExecuteAsync(T obj)
        {
            try
            {
                isExecuting = true;
                RaiseCanExecuteChanged();
                await executeMethod(obj);
            }
            finally
            {
                isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public bool CanExecute(object parameter)
        {
            return !isExecuting && underlyingCommand.CanExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => underlyingCommand.CanExecuteChanged += value;
            remove { underlyingCommand.CanExecuteChanged -= value; }
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            underlyingCommand.OnCanExecuteChanged();
        }
    }
}