using System.Windows;
using System.Windows.Input;

namespace ExcaliburLauncher.GUI
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
