using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoBot.Annotations;
using MoBot.GUI.Controls;

namespace MoBot.GUI.View
{
    internal class MainWindowView : INotifyPropertyChanged
    {
        private readonly MainWindow window;

        private string selectedProfile;
        private UserProfileControl userProfileControl;

        public MainWindowView(MainWindow window)
        {
            this.window = window;
        }

        public string[] GetProfiles => Settings.GetProfiles();

        public string SelectedProfile
        {
            get => selectedProfile;
            set
            {
                selectedProfile = value;
                if(userProfileControl != null)
                    window.ProfileContent.Children.Remove(userProfileControl);
                userProfileControl = new UserProfileControl(value);
                window.ProfileContent.Children.Add(userProfileControl);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
