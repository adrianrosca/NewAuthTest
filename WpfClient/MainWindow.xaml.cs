using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            logTextBox.Document.Blocks.Clear();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshDataAsync();
        }

        private async void RefreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await RefreshDataAsync();
        }

        private async Task RefreshDataAsync()
        {
            await AuthUtil.GetSecureDataAsync(logTextBox, htmlViewer);
        }
    }
}