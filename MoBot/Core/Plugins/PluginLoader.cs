using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;

namespace MoBot.Core.Plugins
{
    internal static class PluginLoader
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private const string ExtensionFolder = "Extensions";

        private static readonly List<IPlugin> LoadedPlugins = new List<IPlugin>();

        public static void LoadPlugins()
        {

            var extensionsPath = Path.Combine(Environment.CurrentDirectory, ExtensionFolder);
            string[] assemblies;

            try
            {
                assemblies = Directory.GetFiles(extensionsPath, "*.dll");
            }
            catch (DirectoryNotFoundException)
            {
                Log.Error("Can't find Extensions folder, creating empty one");
                Directory.CreateDirectory(extensionsPath);
                return;
            }

            foreach (var assemblyPath in assemblies)
            {
                Assembly assembly;

                try
                {
                    assembly = Assembly.LoadFrom(assemblyPath);
                }
                catch (Exception e)
                {
                    Log.Error($"Failed to load assembly {assemblyPath}: {e}");
                    continue;
                }

                LoadExtension(assembly);
            }

            foreach (var plugin in LoadedPlugins)
            {
                try
                {
                    plugin.Initialize();
                    Log.Info($"Plugin {plugin.Name} succesfully loaded!");
                }
                catch (Exception e)
                {
                    Log.Error($"Failed to load plugin {plugin}: {e}");
                }
            }

        }

        private static void LoadExtension(Assembly extensionAssembly)
        {
            var pluginType = typeof(IPlugin);
            var plugins = extensionAssembly.GetTypes()
                .Where(type => type.IsAssignableFrom(pluginType) && !type.IsAbstract && type.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .Cast<IPlugin>().ToArray();
            foreach (var plugin in plugins)
            {
                LoadedPlugins.Add(plugin);
            }
        }
    }
}