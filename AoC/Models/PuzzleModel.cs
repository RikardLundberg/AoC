using AoC.Implementations;

namespace AoC.Models
{
    public class PuzzleModel
    {
        public int Day { get; private set; }
        public int Year { get; private set; }
        public string PuzzleData { get; private set; } = string.Empty;
        public string ResultString { get; private set; } = string.Empty;

        public bool CanRunImplementation { get { return _implementation != null; } private set { } }
        private IDay? _implementation { get; set; } = null;

        public void UpdateModel(int year, int day, string puzzleData)
        {
            Year = year;
            Day = day;
            PuzzleData = puzzleData;
            _implementation = null;

            try
            {
                var implementationClassName = $"AoC.Implementations._{year}.Day{day}, AoC";
                var _implementationClass = Type.GetType(implementationClassName);

                if (_implementationClass != null)
                {
                    var _instance = Activator.CreateInstance(_implementationClass);
                    if (_instance != null && _instance is IDay)
                        _implementation = (IDay)_instance;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void Run()
        {
            try
            {
                _implementation?.Run(PuzzleData);
                ResultString = _implementation?.GetResult() ?? string.Empty;
            }
            catch (Exception e)
            {
                ResultString = $"Error during execution: {e.Message}";
            }
        }

        public void RunTest() //rm and make unit tests?
        {
            try
            {
                _implementation?.TestRun();
                ResultString = _implementation?.GetResult() ?? string.Empty;
            }
            catch (Exception e)
            {
                ResultString = $"Error during test execution: {e.Message}";
            }
        }
    }
}
