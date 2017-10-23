namespace MoBot.Core.Plugins
{
    public interface IPlugin
    {
        string Name { get; }
        string Author { get; }
        string Version { get; }

        void Initialize();
    }
}