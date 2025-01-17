using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        readonly HttpClient client = new HttpClient();

        // Close the application
        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        // Clear the content and URL input
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtContent.Text = string.Empty;
            txtURL.Text = string.Empty;
        }

        // Fetch data from the provided URL
        private async void btnFetchData_Click(object sender, RoutedEventArgs e)
        {
            string uri = txtURL.Text;

            // Validate if the URL is valid
            if (string.IsNullOrWhiteSpace(uri))
            {
                MessageBox.Show("Please enter a valid URL.");
                return;
            }

            try
            {
                // Fetch the content asynchronously
                string responseBody = await client.GetStringAsync(uri);
                txtContent.Text = responseBody.Trim();  // Display the fetched content
            }
            catch (HttpRequestException ex)
            {
                // Show error message if the URL is invalid or there's an issue with the request
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }
    }
}