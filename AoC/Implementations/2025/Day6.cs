using AoC.Common.Extensions;

namespace AoC.Implementations._2025
{
    internal class Day6 : Day
    {
        private List<MathProblem> _problems { get; set; } = new List<MathProblem>();

        public override void Run(string puzzleData)
        {
            if (_secondStar) ParsePuzzleDataSecondStar(puzzleData); else ParsePuzzleData(puzzleData);
            long total = 0;
            foreach (var problem in _problems)
                total += problem.Calculate();
            _result = total.ToString();
        }

        private void ParsePuzzleData(string puzzleData)
        {
            foreach (var line in puzzleData.Split('\n'))
            {
                var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tokens.Length; i++)
                {
                    MathProblem problem = _problems.ElementAtOrDefault(i) ?? _problems.AddIfNotExists(new MathProblem());
                    if (tokens[i].Equals("*") || tokens[i].Equals("+"))
                        problem.Operator = tokens[i][0];
                    else if (long.TryParse(tokens[i], out long operand))
                        problem.Operands.Add(operand);
                }
            }
        }

        private void ParsePuzzleDataSecondStar(string puzzleData)
        {
            var emptyColumns = new List<int>();
            var lines = puzzleData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            lines.Last().ToCharArray().Select((c, i) => (c, i)).Where(t => t.c != ' ').ToList().ForEach(t => emptyColumns.Add(t.i - 1));

            var currentProblemIndex = 0;

            foreach (var line in puzzleData.Split('\n'))
            {
                var currentProblem = _problems.ElementAtOrDefault(currentProblemIndex) ?? _problems.AddIfNotExists(new MathProblem());
                var problemStartIndex = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    if (emptyColumns.Contains(i))
                    {
                        currentProblem = _problems.ElementAtOrDefault(++currentProblemIndex) ?? _problems.AddIfNotExists(new MathProblem());
                        problemStartIndex = i + 1;
                        continue;
                    }
                    if (line[i] == ' ')
                        continue;
                    if (line[i] == '*' || line[i] == '+')
                        currentProblem.Operator = line[i];
                    else
                    {
                        while (currentProblem.OperandBuilders.Count <= i - problemStartIndex)
                            currentProblem.OperandBuilders.Add(string.Empty);
                        currentProblem.OperandBuilders[i - problemStartIndex] += line[i];
                    }
                }
                currentProblemIndex = 0;
            }

            foreach (var problem in _problems)
                problem.Operands = problem.OperandBuilders.Select(ob => long.Parse(ob)).ToList();
        }

        public override void TestRun()
        {
            string input = "123 328  51 64 \n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  ";
            Run(input);
        }

        public override void Reset()
        {
            base.Reset();
            _problems.Clear();
        }
    }

    internal class MathProblem
    {
        public List<long> Operands { get; set; } = new List<long>();
        public char Operator { get; set; } //throws on default
        public List<string> OperandBuilders { get; set; } = new List<string>();

        public long Calculate()
        {
            return Operator switch
            {
                '+' => Operands.Sum(),
                '*' => Operands.Aggregate((long)1, (acc, val) => acc * val),
                _ => throw new InvalidOperationException($"Unsupported operator: {Operator}"),
            };
        }
    }
}