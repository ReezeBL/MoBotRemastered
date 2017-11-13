using System.ComponentModel;
using System.Runtime.CompilerServices;
using MoBot.Annotations;

namespace MoBot.GUI.View
{
    public abstract class AbsractView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}