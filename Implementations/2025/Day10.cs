namespace Implementations._2025
{
    internal class Day10 : Day
    {
        public override void Run(string puzzleData)
        {
            var machines = ReadMachines(puzzleData);

            var total = 0;
            foreach (var machine in machines)
            {
                total += machine.FindShortestStartSequence();
            }
            _result = total.ToString();
        }

        private List<Machine> ReadMachines(string puzzleData)
        {
            var machines = new List<Machine>();
            foreach (var line in puzzleData.Split('\n'))
            {
                if (string.IsNullOrEmpty(line))
                    continue; 
                var machine = new Machine();
                machine.ReadManual(line);
                machines.Add(machine);
            }
            return machines;
        }

        public override void TestRun()
        {
            string input = "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}";
            Run(input);
        }
    }

    internal class Machine
    {
        public List<bool> StartSequence { get; set; } = new List<bool>();
        public List<List<int>> Buttons { get; set; } = new List<List<int>>();
        public List<int> JoltageRequirements { get; set; } = new List<int>();

        public Machine()
        {
            StartSequence = new List<bool>();
            Buttons = new List<List<int>>();
            JoltageRequirements = new List<int>();
        }

        public int FindShortestStartSequence()
        {
            for (int i = 1; i < Buttons.Count; i++) //it does not do us much good to press same button twice
            {
                if (TryAllCombinations(i, 0, new List<int>()) != -1)
                    return i;
            }
            throw new Exception("Unsolvable");
        }

        private int TryAllCombinations(int buttonCount, int index, List<int> pressedButtons)
        {
            if (StartSequenceMatches(pressedButtons) != -1)
                return pressedButtons.Count;
            if (buttonCount <= pressedButtons.Count)
                return -1;
            //if (index >= Buttons.Count)
            //    return -1;
            for (int i = index; i < Buttons.Count; i++)
            {
                if (pressedButtons.Contains(i))
                    continue;
                pressedButtons.Add(i);
                var val = TryAllCombinations(buttonCount, index + 1, pressedButtons);
                if (val != -1) return val;
                pressedButtons.RemoveAt(pressedButtons.Count - 1);
            }
            return -1;
        }

        private int StartSequenceMatches(List<int> pressedButtons)
        {
            var startSequenceTmp = new List<bool>();
            for (int i = 0; i < StartSequence.Count; i++)
                startSequenceTmp.Add(false);

            foreach (var button in pressedButtons)
            {
                for (int i = 0; i < Buttons[button].Count; i++)
                {
                    int position = Buttons[button][i];
                    if (position >= startSequenceTmp.Count)
                        return -1;
                    startSequenceTmp[position] = !startSequenceTmp[position];
                }
            }
            for(int i = 0; i < StartSequence.Count; i++)
                if (startSequenceTmp[i] != StartSequence[i])
                    return -1;
            return pressedButtons.Count;
        }

        public void ReadManual(string manual)
        {
            foreach (var segment in manual.Split(' '))
            {
                ReadManualSegment(segment);
            }
        }

        private void ReadManualSegment(string segment)
        {
            if (segment.StartsWith("[") && segment.EndsWith("]"))
            {
                ReadStartSequence(segment.Trim('[', ']'));
            }
            else if (segment.StartsWith("(") && segment.EndsWith(")"))
            {
                ReadButton(segment.Trim('(', ')'));
            }
            else if (segment.StartsWith("{") && segment.EndsWith("}"))
            {
                ReadJoltageRequirements(segment);
            }
        }

        private void ReadJoltageRequirements(string data)
        {
            var parts = data.Trim('{', '}').Split(',');
            foreach (var part in parts)
            {
                if (int.TryParse(part, out int value))
                {
                    JoltageRequirements.Add(value);
                }
            }
        }

        private void ReadStartSequence(string data)
        {
            foreach (var ch in data)
            {
                StartSequence.Add(ch == '#' ? true : false);
            }
        }

        private void ReadButton(string data)
        {
            var buttonSequence = new List<int>();
            foreach (var buttonIndex in data.Split(','))
            {
                if (int.TryParse(buttonIndex, out int result))
                    buttonSequence.Add(result);
            }
            Buttons.Add(buttonSequence);
        }
    }
}
