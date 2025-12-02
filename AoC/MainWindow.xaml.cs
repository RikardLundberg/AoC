using AoC.ViewModels;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace AoC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        private async void DownloadDataFile(int year, int day)
        {
            try
            {
                var cookie = "53616c7465645f5f1914d309222a999fe1bb10ed8d668d64cef250330ec8404e30b6a168d9df17b7ebb38edbf0c6fdf2652f67fbf5554a45fb15c2d2c554a137";
                var uri = new Uri("https://adventofcode.com");
                var cookies = new CookieContainer();
                cookies.Add(uri, new System.Net.Cookie("session", cookie));
                using var handler = new HttpClientHandler() { CookieContainer = cookies };
                using var client = new HttpClient(handler) { BaseAddress = uri };
                using var response = await client.GetAsync($"/{year}/day/{day}/input");
                var str = response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Download error");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.AddDays();
        }

        private void ReadDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ReadDownloadFile();
        }

        private void RunCodeButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RunCode();
        }

        private void RunTestButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RunTest();
        }
    }
}