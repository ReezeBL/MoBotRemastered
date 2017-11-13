using System.Windows;
using MoBot.Core;

namespace MoBot.GUI
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Логика взаимодействия для BotWindow.xaml
    /// </summary>
    public partial class BotWindow
    {
        public MoBase BotInstance { get; }
        public BotWindow(MoBase instance)
        {
            InitializeComponent();
            BotInstance = instance;
        }
    }
}
