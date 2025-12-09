using System.Windows;

namespace Implementations._2025
{
    internal class Day9 : Day
    {
        public override void Run(string puzzleData)
        {
            var points = new List<Point>();
            foreach (var line in puzzleData.Split('\n'))
            {
                var coords = line.Split(',');
                if (coords.Length != 2)
                    continue;
                points.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1])));
            }

            var largestArea = 0L;
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    var area = CalculateArea(points[i], points[j]);
                    if (area > largestArea)
                        largestArea = area;
                }
            }
            _result = largestArea.ToString();
        }

        private long CalculateArea(Point a, Point b)
        {
            var x = (long)Math.Abs(a.X - b.X) + 1;
            var y = (long)Math.Abs(a.Y - b.Y) + 1;
            return x * y;
        }

        public override void TestRun()
        {
            string input = "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3";
            Run(input);
        }
    }
}
