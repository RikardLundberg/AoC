using AoC.Models;
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

        private PuzzleModel ActiveModel { get; set; } = new PuzzleModel();

        private bool _fileCanBeDownloaded { get; set; }
        private bool _actionIsAvailable { get; set; }
        private string _readFileDate { get; set; }
        private string _availableActionString { get; set; }
        private int _year { get; set; }
        private int _day { get; set; }

        public MainViewModel()
        {
            AddData();
            ReadFileDate = "None";
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

        private void UpdateControls()
        {
            FileCanBeDownloaded = !System.IO.File.Exists(GetDownloadPath());
            AvailableActionString = FileCanBeDownloaded ? "Download" : "Read";
            ActionIsAvailable = ActiveModel.Year != Year || ActiveModel.Day != Day || string.IsNullOrEmpty(ActiveModel.PuzzleData);
        }

        public void ReadDownloadFile()
        {
            if (FileCanBeDownloaded)
                DownloadFile();
            else
                ReadFile();
            UpdateControls();
        }

        public void ReadFile()
        {
            using (StreamReader sr = new StreamReader(GetDownloadPath()))
            {
                SetActiveModelData(sr.ReadToEnd());
            }
        }

        private void SetActiveModelData(string puzzleData)
        {
            ActiveModel.PuzzleData = puzzleData;
            ActiveModel.Year = Year;
            ActiveModel.Day = Day;
            ReadFileDate = $"{Year}, {Day}";
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

                SetActiveModelData(str.Result);

                using (StreamWriter sw = new StreamWriter(GetDownloadPath()))
                    sw.Write(str.Result);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Download error");
            }

            UpdateControls();
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
                    UpdateControls();
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
                    UpdateControls();
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
        
        public bool ActionIsAvailable
        {
            get
            {
                return this._actionIsAvailable;
            }
            set
            {
                if (this._actionIsAvailable != value)
                {
                    this._actionIsAvailable = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ReadFileDate
        {
            get
            {
                return this._readFileDate;
            }
            set
            {
                if (this._readFileDate != value)
                {
                    this._readFileDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AvailableActionString
        {
            get
            {
                return this._availableActionString;
            }
            set
            {
                if (this._availableActionString != value)
                {
                    this._availableActionString = value;
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
