using System.Windows;
using ExcaliburLauncher.GUI.Viewers;

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
            var viewer = new MainWindowView();
            DataContext = viewer;
        }
    }
}
