using System;
using System.Windows;
using ExcaliburLauncher.GUI;

namespace ExcaliburLauncher
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var application = new Application();
            var window = new MainWindow();
            application.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
            application.Run(window);
        }

        private static void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var errorMessage = $"{e.Exception.Message}";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
