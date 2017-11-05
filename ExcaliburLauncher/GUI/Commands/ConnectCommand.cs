using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using ExcaliburLauncher.Core;
using ExcaliburLauncher.GUI.Viewers;

namespace ExcaliburLauncher.GUI.Commands
{
    internal class ConnectCommand : ICommand
    {
        private readonly MainWindowView view;
        public ConnectCommand(MainWindowView view)
        {
            this.view = view;
        }

        public bool CanExecute(object parameter)
        {
            return view.SelectedConfig != null && !string.IsNullOrEmpty(view.Username) && !string.IsNullOrEmpty(view.Password);
        }

        public async void Execute(object parameter)
        {
            var authParams = await ExcaliburAuth.GetAuthSession(view.Username, view.Password);
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(view.JavaPath, "java.exe"),
                WorkingDirectory = view.WorkingDirectory,
                Arguments = GetArguments(view.SelectedConfig, authParams)

            };
            process.StartInfo = startInfo;
            process.Start();
        }

        private string GetArguments(ExcaliburAuth.ServerConfig serverConfig, ExcaliburAuth.AuthData authData)
        {
            var stringBuilder = new StringBuilder(1024);
            var javaPath = @"-Djava.library.path=""/bin/natives""";
            var arguments = serverConfig.Arguments.Replace("@SESSION@", authData.Session).Replace("@USER@", authData.Username);
            var javaParams = $@"{serverConfig.ExtraArguments} - XX:+UseConcMarkSweepGC - XX:+CMSIncrementalMode - XX:-UseAdaptiveSizePolicy ";
            return stringBuilder.ToString();
        }

        public event EventHandler CanExecuteChanged;

        public virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
