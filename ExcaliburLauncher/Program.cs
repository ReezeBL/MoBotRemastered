using System;
using System.Linq;
using System.Windows;
using ExcaliburLauncher.Core;
using ExcaliburLauncher.GUI;

namespace ExcaliburLauncher
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //var configs = ExcaliburAuth.GetConfig().Result;
            //Console.WriteLine(string.Join("\n", configs.Select(config => config.Name)));
            //Console.ReadLine();
            var application = new Application();
            var window = new MainWindow();
            application.Run(window);
        }
    }
}
