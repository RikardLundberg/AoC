namespace AoC.Implementations._2025
{
    internal class Day1 : IDay
    {
        private string _result { get; set; } = string.Empty;
        private bool secondPart { get; set; } = true; //should be triggered in UI

        public string GetResult()
        {
            return _result;
        }

        public void Run(string puzzleData)
        {
            var counter = 0;
            var currentPosition = 50;
            foreach (var rotation in puzzleData.Split("\n"))
            {
                if (string.IsNullOrEmpty(rotation))
                    continue;

                var rotationValue = GetRotation(rotation);

                if (secondPart)
                    counter += Math.Abs(rotationValue) / 100;
                rotationValue %= 100;

                currentPosition += rotationValue;

                if (currentPosition < 0)
                {
                    if (secondPart && currentPosition != rotationValue)
                        counter++;

                    currentPosition += 100;
                }
                if (secondPart && currentPosition > 100)
                    counter++;
                currentPosition %= 100;

                if (currentPosition == 0)
                    counter++;
            }
            _result = counter.ToString();
        }

        public void TestRun()
        {
            string input = "L68\nL30\nR48\nL5\nR60\nL55\nL1\nL99\nR14\nL82";
            Run(input);
        }

        private int GetRotation(string rotation)
        {
            if (rotation.StartsWith('L') == true)
            {
                if (int.TryParse(rotation.Substring(1), out int result))
                {
                    return -result;
                }
            }
            else if (rotation.StartsWith('R') == true)
            {
                if (int.TryParse(rotation.Substring(1), out int result))
                    return result;
            }
            throw new Exception("Invalid rotation");
        }

    }
}
