using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MoBot.Core;
using MoBot.Core.Plugins;
using MoBot.GUI.Commands;

namespace MoBot.GUI.View
{
    internal class MainWindowView : AbsractView
    {
        private UserSettingsView selectedProfile;

        public MainWindowView()
        {
            SaveCommand = new RelayCommand(o => SelectedProfile.SaveProfile());
            LoadCommand = new RelayCommand(LoadProfile, CanLoadProfile);
        }

        public ObservableCollection<UserSettingsView> SettingsViews { get; set; } = GetSettingsViews();

        public UserSettingsView SelectedProfile
        {
            get => selectedProfile;
            set
            {
                if (Equals(value, selectedProfile)) return;
                selectedProfile = value;
                selectedProfile.Select();
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }

        private static ObservableCollection<UserSettingsView> GetSettingsViews()
        {
            return new ObservableCollection<UserSettingsView>(Settings.GetProfiles().Select(profile => new UserSettingsView(profile)));
        }

        private void LoadProfile(object context)
        {
            var botInstance = new MoBase(selectedProfile.Profile);
            botInstance.Notify += (_, e) => Console.WriteLine(e);
            GlobalModules.AddProfile(selectedProfile.Profile, botInstance);
            var botWindow = new BotWindow();
            botWindow.SetProfile(selectedProfile.Profile);
            botWindow.Closed += OnBotWindowClosed;
            botWindow.Show();
        }

        private static void OnBotWindowClosed(object sender, EventArgs eventArgs)
        {
            if(!(sender is BotWindow botWindow))
                return;
            botWindow.Closed -= OnBotWindowClosed;
            GlobalModules.UnloadInstance(botWindow.BotInstance);
        }

        private bool CanLoadProfile(object context)
        {
            if (selectedProfile == null)
                return false;
            return !GlobalModules.IsProfileLoaded(selectedProfile.Profile);
        }
    }
}
