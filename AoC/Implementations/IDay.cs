namespace AoC.Implementations
{
    interface IDay
    {
        string GetResult();
        void Run(string puzzleData);
        void TestRun();
        public void SetSecondStar(bool secondStar);
    }
}
