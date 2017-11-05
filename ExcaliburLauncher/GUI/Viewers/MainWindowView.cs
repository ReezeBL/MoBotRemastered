using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExcaliburLauncher.Annotations;
using ExcaliburLauncher.Core;
using ExcaliburLauncher.GUI.Commands;

namespace ExcaliburLauncher.GUI.Viewers
{
    internal class MainWindowView : INotifyPropertyChanged
    {
        private readonly Task<List<ExcaliburAuth.ServerConfig>> getConfigsTask = ExcaliburAuth.GetConfig();
        private ExcaliburAuth.ServerConfig selectedConfig;

        public MainWindowView()
        {
            ConnectCommand = new ConnectCommand(this);
        }

        public ConnectCommand ConnectCommand { get; }

        public List<ExcaliburAuth.ServerConfig> Configs => getConfigsTask.Result;
        public ExcaliburAuth.ServerConfig SelectedConfig
        {
            get => selectedConfig;
            set
            {
                selectedConfig = value;
                OnPropertyChanged(nameof(WorkingDirectory));
                ConnectCommand.OnCanExecuteChanged();
            }
        }
        public string WorkingDirectory
        {
            get => SelectedConfig?.Directory ?? string.Empty;
            set
            {
                if (value == SelectedConfig.Directory) return;
                SelectedConfig.Directory = value;
                OnPropertyChanged();
            }
        }

        public string Username { get; set; } = "Oleg";
        public string Password { get; set; } = "Loh";

        public string JavaPath { get; set; } = GetDefaultJavaPath();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetDefaultJavaPath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "exjava", "jvm", "bin");
            return path;
        }
    }
}
