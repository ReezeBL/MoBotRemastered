using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MoBot.GUI.Commands;
using MoBot.GUI.View;

namespace MoBot.GUI.Controls
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// Логика взаимодействия для UserProfileControl.xaml
    /// </summary>
    public partial class UserProfileControl
    {
        private readonly UserSettingsView viewer;

        public UserProfileControl(string profile)
        {
            InitializeComponent();
            viewer = new UserSettingsView(profile);
            DataContext = viewer;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            viewer.SaveProfile();
        }
    }
}
