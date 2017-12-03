using System;
using System.Threading.Tasks;
using System.Windows;
using MoBot.Core;
using MoBot.Core.Plugins;
using MoBot.GUI;

namespace MoBot
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            PluginLoader.LoadPlugins();
            var application = new App();
            application.Run();
         
            //var instance = new MoBase("Default");
            //var profile = Settings.LoadProfile("Default");
            //instance.Notify += (sender, message) => Console.WriteLine(message);
            //Task<bool> task;
            //do
            //{
            //    task = instance.Connect();
            //    if(task.Result == false)
            //        Console.WriteLine("Connection failed!");
            //} while (task.Result == false);
            //Console.WriteLine(task.Result);
            //Console.ReadLine();
        }
    }
}