using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AoC.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<int> Years { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<int> Days { get; set; } = new ObservableCollection<int>();

        private bool _fileCanBeDownloaded { get; set; }
        private int _year { get; set; }
        private int _day { get; set; }

        public MainViewModel()
        {
            AddData();
        }

        private void AddData()
        {
            var dateTimeToday = DateTime.Today;
            if (dateTimeToday.Month > 11)
                Years.Add((int)dateTimeToday.Year);
            for (int i = dateTimeToday.Year - 1; i > 2015; i--)
                Years.Add(i);

            Year = Years.FirstOrDefault();
            AddDays();
        }

        public void AddDays()
        {
            Days.Clear();
            int dayCount = 25;

            var dateTimeToday = DateTime.Today;
            if (dateTimeToday.Year == Year && dateTimeToday.Month == 12)
                dayCount = dateTimeToday.Day > 25 ? 25 : dateTimeToday.Day;

            for (int i = dayCount; i > 0; i--)
                Days.Add(i);

            int actDay = _day;

            if (!Days.Contains(actDay))
                Day = Days.FirstOrDefault();
            else
                Day = actDay;
        }

        private string GetDownloadPath()
        {
            return $@"C:\TEMP\AoC\{Year}\{Day}.txt";
        }

        private void UpdateFileCanBeDownloaded()
        {
            FileCanBeDownloaded = !System.IO.File.Exists(GetDownloadPath());
        }

        public async void DownloadFile()
        {
            try
            {
                var dir = Path.GetDirectoryName(GetDownloadPath());
                if (string.IsNullOrEmpty(dir))
                    throw new Exception("Unable to find download path");

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var cookie = "53616c7465645f5f1914d309222a999fe1bb10ed8d668d64cef250330ec8404e30b6a168d9df17b7ebb38edbf0c6fdf2652f67fbf5554a45fb15c2d2c554a137";
                var uri = new Uri("https://adventofcode.com");
                var cookies = new CookieContainer();
                cookies.Add(uri, new System.Net.Cookie("session", cookie));
                using var handler = new HttpClientHandler() { CookieContainer = cookies };
                using var client = new HttpClient(handler) { BaseAddress = uri };
                using var response = await client.GetAsync($"/{Year}/day/{Day}/input");
                var str = response.Content.ReadAsStringAsync();

                using (StreamWriter sw = new StreamWriter(GetDownloadPath()))
                    sw.Write(str.Result);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Download error");
            }

            UpdateFileCanBeDownloaded();
        }

        public int Day
        {
            get
            {
                return this._day;
            }
            set
            {
                //if (this._day != value)
                {
                    this._day = value;
                    OnPropertyChanged();
                    UpdateFileCanBeDownloaded();
                }
            }
        }

        public int Year
        {
            get
            {
                return this._year;
            }
            set
            {
                if (this._year != value)
                {
                    this._year = value;
                    OnPropertyChanged();
                    UpdateFileCanBeDownloaded();
                }
            }
        }

        public bool FileCanBeDownloaded
        {
            get
            {
                return this._fileCanBeDownloaded;
            }
            set
            {
                if (this._fileCanBeDownloaded != value)
                {
                    this._fileCanBeDownloaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
