namespace AoC.Implementations._2025
{
    internal class Day2 : Day
    {
        public override void Run(string puzzleData)
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

        private bool ValidID(string input)
        {
            return _secondStar ? ValidIDSecondStar(input) : ValidIDFirstStar(input);
        }

        public bool ValidIDFirstStar(string input)
        {
            if (input.Length % 2 != 0)
                return false;

            if (input.StartsWith(input.Substring(input.Length / 2)))
                return true;
            return false;
        }

        public bool ValidIDSecondStar(string input)
        {
            for (int i = 1; i <= input.Length / 2; i++)
            {
                var segment = input.Substring(0, i);
                var compareString = segment;
                while (compareString.Length < input.Length)
                    compareString += segment;
                if (compareString.Equals(input))
                    return true;
            }
            return false;
        }

        public override void TestRun()
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
