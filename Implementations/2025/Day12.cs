namespace Implementations._2025
{
    internal class Day12 : Day
    {
        List<Problem> problems { get; set; } = new List<Problem>();
        public override void Run(string puzzleData)
        {
            Parse(puzzleData);
            int count = 0;
            foreach(var problem in problems)
            {
                if (problem.CanSolve())
                    count++;
            }
            _result = count.ToString();
        }

        private void Parse(string puzzleData)
        {
            foreach(var line in puzzleData.Split('\n'))
            {
                if (!line.Contains(':'))
                    continue;
                if (!line.Split(':')[0].Contains('x'))
                    continue;
                Problem problem = new Problem();
                var colonSplit = line.Split(':');
                var sizeSplit = colonSplit[0].Split('x');
                problem.Width = int.Parse(sizeSplit[0]);
                problem.Height = int.Parse(sizeSplit[1]);
                var valueSplit = colonSplit[1].Trim().Split(' ');
                for(int i = 0; i < valueSplit.Length; i++)
                    problem.PresentCount[i] = int.Parse(valueSplit[i]);
                problems.Add(problem);
            }
        }

        public override void Reset()
        {
            base.Reset();
            problems = new List<Problem>();
        }

        public override void TestRun()
        {
            string input = "0:\n###\n##.\n##.\n\n1:\n###\n##.\n.##\n\n2:\n.##\n###\n##.\n\n3:\n##.\n###\n##.\n\n4:\n###\n#..\n###\n\n5:\n###\n.#.\n###\n\n4x4: 0 0 0 0 2 0\n12x5: 1 0 1 0 2 2\n12x5: 1 0 1 0 3 2";
            Run(input);
        }
    }

    public class Problem
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public int[] PresentCount { get; set; } = new int[6];

        public bool CanSolve()
        {
            var total = 0;
            for(int i = 0; i < PresentCount.Length; i++)
                total += PresentCount[i];

            int widthThird = Width / 3;
            int heightThird = Height / 3;
            if (widthThird * heightThird >= total)
                return true;
            return false;
        }

    }
}
