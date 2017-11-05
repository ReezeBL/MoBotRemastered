using System;
using System.Windows.Input;
using MoBot.Core;
using MoBot.Core.Plugins;

namespace MoBot.GUI.Commands
{
    internal class LoadProfile : ICommand
    {
        private readonly string profile;

        public LoadProfile(string profile)
        {
            this.profile = profile;
        }

        public bool CanExecute(object parameter) => !GlobalModules.IsProfileLoaded(profile);

        public void Execute(object parameter)
        {
            GlobalModules.AddProfile(profile, new MoBase(profile));
            OnCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
