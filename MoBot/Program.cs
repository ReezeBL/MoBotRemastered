using System;
using System.Threading.Tasks;
using System.Windows;
using MoBot.Core;
using MoBot.Core.Plugins;
using MoBot.GUI;

namespace MoBot
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            PluginLoader.LoadPlugins();
            var application = new Application();
            var window = new MainWindow();
            application.Run(window);
            //var instance = new MoBase();
            //instance.Notify += (sender, message) => Console.WriteLine(message);
            //Task<bool> task;
            //do
            //{
            //    task = instance.Connect("Default");
            //    if (!task.Result)
            //        Console.WriteLine("Connection failed");
            //} while (!task.Result);
            //Console.ReadLine();
        }
    }
}