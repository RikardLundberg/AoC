namespace AoC.Implementations._2025
{
    internal class Day5 : Day
    {
        private List<(long Low, long High)> _ranges { get; set; } = new List<(long Low, long High)>();
        private List<long> _ids { get; set; } = new List<long>();
        private int _freshCount { get; set; } = 0;
        private int currentRangeId { get; set; } = 0;

        public override void Run(string puzzleData)
        {
            parsePuzzleData(puzzleData);
            _result = _secondStar ? SecondStar() : FirstStar();
        }

        private string SecondStar()
        {
            long freshIdCount = 0;
            var mergedRanges = getMergedRanges();

            foreach (var range in mergedRanges)
            {
                freshIdCount += range.High - range.Low + 1;
            }
            return freshIdCount.ToString();
        }

        private List<(long Low, long High)> getMergedRanges()
        {
            var mergedRanges = new List<(long Low, long High)>();
            var currentRange = _ranges[0];
            for (int i = 1; i < _ranges.Count; i++)
            {
                var nextRange = _ranges[i];
                if (nextRange.Low <= currentRange.High + 1)
                {
                    currentRange.High = Math.Max(currentRange.High, nextRange.High);
                }
                else
                {
                    mergedRanges.Add(currentRange);
                    currentRange = nextRange;
                }
            }
            mergedRanges.Add(currentRange);
            return mergedRanges;
        }

        private string FirstStar()
        {
            foreach (var id in _ids)
            {
                while (currentRangeId < _ranges.Count && !checkFreshness(id))
                {
                    currentRangeId++;
                }
            }
            return _freshCount.ToString();
        }

        private bool checkFreshness(long id)
        {
            var currentRange = _ranges[currentRangeId];
            if (id < currentRange.Low)
                return true;
            if (id > currentRange.High)
                return false;
            _freshCount++;
            return true;
        }

        private void parsePuzzleData(string puzzleData)
        {
            resetState();

            foreach (var line in puzzleData.Split('\n'))
            {
                var rangeLimits = line.Split('-');
                if (rangeLimits.Length == 1 && long.TryParse(rangeLimits[0], out long result))
                    _ids.Add(result);
                else if (rangeLimits.Length == 2 && long.TryParse(rangeLimits[0], out long res1) && long.TryParse(rangeLimits[1], out long res2))
                    _ranges.Add((res1, res2));
            }
            _ranges.Sort();
            _ids.Sort();
        }
        private void resetState()
        {
            _ranges.Clear();
            _ids.Clear();
            _freshCount = 0;
            currentRangeId = 0;
        }

        public override void TestRun()
        {
            string input = "3-5\n10-14\n16-20\n12-18\n\n1\n5\n8\n11\n17\n32";
            Run(input);
        }
    }
}
