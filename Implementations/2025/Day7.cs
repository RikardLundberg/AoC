namespace Implementations._2025
{
    internal class Day7 : Day
    {
        private List<long> _tachyonBeams { get; set; } = new List<long>();
        public override void Run(string puzzleData)
        {
            var splitCount = 0;
            var lines = puzzleData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines[0].Length; i++)
            {
                _tachyonBeams.Add(0);
                if (lines[0][i] == 'S')
                    _tachyonBeams[i] = 1;
            }

            foreach (var line in lines)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '^' && _tachyonBeams[i] > 0)
                    {
                        splitCount++;
                        if (i > 0)
                            _tachyonBeams[i - 1] += _tachyonBeams[i];
                        if (i < line.Length - 1)
                            _tachyonBeams[i + 1] += _tachyonBeams[i];
                        _tachyonBeams[i] = 0;
                    }
                }
            }

            _result = _secondStar ? _tachyonBeams.Sum().ToString() : splitCount.ToString();
        }

        public override void TestRun()
        {
            string input = ".......S.......\n...............\n.......^.......\n...............\n......^.^......\n...............\n.....^.^.^.....\n...............\n....^.^...^....\n...............\n...^.^...^.^...\n...............\n..^...^.....^..\n...............\n.^.^.^.^.^...^.\n...............";
            Run(input);
        }

        public override void Reset()
        {
            base.Reset();
            _tachyonBeams.Clear();
        }
    }
}
