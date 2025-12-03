namespace AoC.Implementations
{
    abstract class Day : IDay
    {
        protected string _result { get; set; } = string.Empty;
        protected bool _secondStar { get; set; } = false;

        public string GetResult()
        {
            return _result;
        }

        public void SetSecondStar(bool secondStar)
        {
            _secondStar = secondStar;
        }

        public abstract void Run(string puzzleData);
        public abstract void TestRun();
    }
}
