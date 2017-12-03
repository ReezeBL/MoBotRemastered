using System.Windows;
using MoBot.Core;
using MoBot.Core.Plugins;

namespace MoBot.GUI
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Логика взаимодействия для BotWindow.xaml
    /// </summary>
    public partial class BotWindow
    {
        public MoBase BotInstance { get; private set; }

        public BotWindow()
        {
            InitializeComponent();
        }

        public void SetProfile(string profile)
        {
            BotInstance = GlobalModules.GetProfile(profile);
            View.Profile = profile;
        }
    }
}
