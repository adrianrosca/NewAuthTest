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
            ProgressTextBox.Document.Blocks.Clear();  // Clear any initial empty blocks
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await RunAuthentication();
        }

        private async void AuthenticateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProgressTextBox.Document.Blocks.Clear();  // Clear the RichTextBox content
            AppendProgress("Starting new authentication...");
            await RunAuthentication();
        }

        private async Task RunAuthentication()
        {
            var (data, logs) = await AuthUtil.GetSecureDataAsync();

            // Append all logs to ProgressTextBox
            foreach (var (message, isSuccess) in logs)
            {
                AppendProgress(message, isSuccess);
            }

            AppendProgress(data, isSuccess: true);  // Display final result or error
        }

        private void AppendProgress(string message, bool isSuccess = false)
        {
            var paragraph = new Paragraph
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0)
            };
            paragraph.Inlines.Add(new Run($"{DateTime.Now:T}: {message}")
            {
                Foreground = isSuccess ? Brushes.Green : Brushes.Black
            });
            ProgressTextBox.Document.Blocks.Add(paragraph);
            ProgressTextBox.ScrollToEnd();
        }
    }
}
