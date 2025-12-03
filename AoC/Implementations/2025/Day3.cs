namespace AoC.Implementations._2025
{
    internal class Day3 : IDay
    {
        private string _result { get; set; } = string.Empty;
        private bool secondPart { get; set; } = true; //should be triggered in UI

        public string GetResult()
        {
            return _result;
        }

        public void Run(string puzzleData)
        {
            long total = 0;
            var size = secondPart ? 12 : 2;
            foreach (var bank in puzzleData.Split("\n"))
            {
                total += GetLargestVoltageFromBank(bank, size);
            }
            _result = total.ToString();
        }

        private long GetLargestVoltageFromBank(string bank, int size)
        {
            var highestJoltage = "";
            for (int i = size - 1; i >= 0; i--)
            {
                var largest = FindLargestNumberInSegment(bank, i);
                bank = bank.Substring(largest.position + 1);
                highestJoltage += largest.number;
            }
            return long.Parse(highestJoltage);
        }

        private (int position, char number) FindLargestNumberInSegment(string segment, int skipTailCount)
        {
            char highest = '0';
            int position = -1;
            for (int i = 0; i < segment.Length - skipTailCount; i++)
            {
                if (segment[i] > highest)
                {
                    highest = segment[i];
                    position = i;
                }
                if (highest == '9')
                    break;
            }
            return (position, highest);
        }

        public void TestRun()
        {
            string input = "987654321111111\n811111111111119\n234234234234278\n818181911112111";
            Run(input);
        }
    }
}
