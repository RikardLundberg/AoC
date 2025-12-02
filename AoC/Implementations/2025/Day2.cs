namespace AoC.Implementations._2025
{
    internal class Day2 : IDay
    {
        private string _result { get; set; } = string.Empty;
        private bool secondPart { get; set; } = false; //should be triggered in UI

        public string GetResult()
        {
            return _result;
        }

        public void Run(string puzzleData)
        {
            long total = 0;
            foreach (var range in puzzleData.IterateRanges())
            {
                for (long i = range.Lower; i <= range.Higher; i++)
                {
                    if (ValidID(i.ToString()))
                        total += i;
                }
            }
            _result = total.ToString();
        }

        public bool ValidID(string input)
        {
            if (input.Length % 2 != 0)
                return false;

            if (input.StartsWith(input.Substring(input.Length / 2)))
                return true;
            return false;
        }

        public void TestRun()
        {
            string input = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";
            Run(input);
        }
    }

    internal static class Extensions
    {
        public static IEnumerable<(long Lower, long Higher)> IterateRanges(this string str)
        {
            foreach (var range in str.Split(','))
            {
                if (string.IsNullOrEmpty(range))
                    continue;

                var bounds = range.Split('-');
                if (bounds.Length != 2)
                    continue;

                if (long.TryParse(bounds[0], out long start) && long.TryParse(bounds[1], out long end))
                {
                    yield return (start, end);
                }
            }
        }
    }

}
