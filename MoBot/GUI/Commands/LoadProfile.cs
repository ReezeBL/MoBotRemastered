using System;
using System.Windows.Input;
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

        public bool CanExecute(object parameter) => GlobalModules.IsProfileLoaded(profile);

        public void Execute(object parameter)
        {
            
        }

        public event EventHandler CanExecuteChanged;
    }
}
