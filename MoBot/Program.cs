using System;
using System.Threading.Tasks;
using MoBot.Core;
using MoBot.Core.Plugins;

namespace MoBot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            PluginLoader.LoadPlugins();

            var instance = new MoBase();
            instance.Notify += (sender, message) => Console.WriteLine(message);
            Task<bool> task;
            do
            {
                task = instance.Connect("Default");
                if (!task.Result)
                    Console.WriteLine("Connection failed");
            } while (!task.Result);
            Console.ReadLine();
        }
    }
}