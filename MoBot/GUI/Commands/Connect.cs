using System;
using System.Windows.Input;
using MoBot.Core;

namespace MoBot.GUI.Commands
{
    internal class Connect : ICommand
    {
        private readonly MoBase instance;
        private bool tryingToConnect;

        public bool TryingToConnect
        {
            get => tryingToConnect;
            set
            {
                tryingToConnect = value;
                OnCanExecuteChanged();
            }
        }

        public Connect(MoBase instance)
        {
            this.instance = instance;
        }

        public bool CanExecute(object parameter)
        {
            return !TryingToConnect && !instance.Connected;
        }

        public async void Execute(object parameter)
        {
            TryingToConnect = true;
            await instance.Connect();
            TryingToConnect = false;
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
