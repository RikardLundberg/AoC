using System.Windows.Media.Media3D;

namespace Implementations._2025
{
    internal class Day8 : Day
    {
        private int _connectionCount { get; set; } = 1000;
        private List<List<JunctionBox>> circuits { get; set; } = new List<List<JunctionBox>>();
        public override void Run(string puzzleData)
        {
            var boxes = GetBoxes(puzzleData);
            var pairs = GetPairs(boxes);
            pairs.Sort((a, b) => a.Distance.CompareTo(b.Distance));

            int index = -1;
            if (!_secondStar)
            {
                for (int i = 0; i < _connectionCount; i++)
                {
                    MakeConnection(pairs[++index]);
                }
                var circuitList = circuits.Select(circuit => circuit.Count).ToList();
                circuitList.Sort();
                circuitList.Reverse();
                _result = (circuitList[0] * circuitList[1] * circuitList[2]).ToString();
            }
            else
            {
                while (circuits.Count < 1 || circuits[0].Count < boxes.Count)
                {
                    MakeConnection(pairs[++index]);
                }
                _result = (pairs[index].First.Position.X * pairs[index].Second.Position.X).ToString();
            }
        }

        private void MakeConnection(Pair pair)
        {
            int indexFirst = circuits.FindIndex(a => a.Contains(pair.First));
            int indexSecond = circuits.FindIndex(a => a.Contains(pair.Second));

            if (indexFirst == indexSecond && indexFirst != -1)
            {
                return;
            }
            else if (indexFirst != -1 && indexSecond != -1)
            {
                circuits[indexFirst].AddRange(circuits[indexSecond]);
                circuits.RemoveAt(indexSecond);
            }
            else if (indexFirst != -1)
            {
                circuits[indexFirst].Add(pair.Second);
            }
            else if (indexSecond != -1)
            {
                circuits[indexSecond].Add(pair.First);
            }
            else
            {
                circuits.Add(new List<JunctionBox>() { pair.First, pair.Second });
            }
        }

        private List<Pair> GetPairs(List<JunctionBox> boxes)
        {
            List<Pair> pairs = new List<Pair>();
            for (int i = 0; i < boxes.Count; i++)
            {
                for (int j = i + 1; j < boxes.Count; j++)
                {
                    pairs.Add(new Pair(boxes[i], boxes[j]));
                }
            }
            return pairs;
        }

        private List<JunctionBox> GetBoxes(string puzzleData)
        {
            List<JunctionBox> boxes = new List<JunctionBox>();
            int id = 0;

            foreach (var line in puzzleData.Split('\n'))
            {
                var coords = line.Split(',');
                if (coords.Length != 3)
                    continue;
                boxes.Add(new JunctionBox(new Point3D(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2])), id++));
            }
            return boxes;
        }

        public override void TestRun()
        {
            string input = "162,817,812\n57,618,57\n906,360,560\n592,479,940\n352,342,300\n466,668,158\n542,29,236\n431,825,988\n739,650,466\n52,470,668\n216,146,977\n819,987,18\n117,168,530\n805,96,715\n346,949,466\n970,615,88\n941,993,340\n862,61,35\n984,92,344\n425,690,689";
            _connectionCount = 10;
            Run(input);
        }

        public override void Reset()
        {
            base.Reset();
            circuits = new List<List<JunctionBox>>();
            _connectionCount = 1000;
        }
    }

    public class Pair
    {
        public JunctionBox First { get; set; }
        public JunctionBox Second { get; set; }
        public double Distance { get; set; }

        public Pair(JunctionBox first, JunctionBox second)
        {
            this.First = first;
            this.Second = second;
            this.Distance = (first.Position - second.Position).Length;
        }
    }

    public class JunctionBox
    {
        public Point3D Position { get; set; }
        public int Id { get; set; }
        public JunctionBox(Point3D position, int id)
        {
            this.Position = position;
            this.Id = id;
        }
    }
}
