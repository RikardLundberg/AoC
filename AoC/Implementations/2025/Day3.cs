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
            var total = 0;
            foreach (var bank in puzzleData.Split("\n"))
            {
                total += GetLargestFromBank(bank);
            }
            _result = total.ToString();
        }

        private int GetLargestFromBank(string bank)
        {
            var highestJoltage = "";
            var currentHighest = '0';
            int start = 0;
            for (int i = 0; i < bank.Length - 1; i++)
            {
                if (bank[i] > currentHighest)
                {
                    currentHighest = bank[i];
                    start = i + 1;
                }
                if (currentHighest == '9')
                    break;
            }
            highestJoltage += currentHighest;
            currentHighest = '0';
            for (int i = start; i < bank.Length; i++)
            {
                if (bank[i] > currentHighest)
                {
                    currentHighest = bank[i];
                }
                if (currentHighest == '9')
                    break;
            }
            highestJoltage += currentHighest;
            return int.Parse(highestJoltage);
        }

        public void TestRun()
        {
            string input = "987654321111111\n811111111111119\n234234234234278\n818181911112111";
            Run(input);
        }
    }
}
