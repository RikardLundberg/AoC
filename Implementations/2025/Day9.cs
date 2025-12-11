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

            var areas = new List<Area>();
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    areas.Add(CalculateArea(points[i], points[j]));
                }
            }

            areas.Sort((a, b) => a.TotalArea.CompareTo(b.TotalArea));
            areas.Reverse();

            if (!_secondStar)
                _result = areas[0].TotalArea.ToString();
            else
            {
                var lines = new List<Vector>();
                for (int i = 0; i < points.Count - 1; i++)
                    lines.Add(Point.Subtract(points[i + 1], points[i]));

                int tryCount = 0;
                while (tryCount < areas.Count)
                {
                    var area = areas[tryCount];
                    bool intersects = false;

                    for (int i = 0; i < lines.Count; i++)
                    {
                        var line = lines[i];
                        var areaLines = new List<Vector>()
                        {
                            new Vector(area.Second.X - area.First.X, 0),
                            new Vector(0, area.Second.Y - area.First.Y),
                            new Vector(area.First.X - area.Second.X, 0),
                            new Vector(0, area.First.Y - area.Second.Y)
                        };
                        //var areaLines = new List<Line2D>()
                        //{
                        //    new Line2D(new Point2D(area.First.X, area.First.Y), new Point2D(area.Second.X, area.First.Y)),
                        //    new Line2D(new Point2D(area.Second.X, area.First.Y), new Point2D(area.Second.X,  area.Second.Y)),
                        //    new Line2D(new Point2D(area.Second.X, area.Second.Y), new Point2D(area.First.X, area.Second.Y)),
                        //    new Line2D(new Point2D(area.First.X, area.Second.Y), new Point2D(area.First.X, area.First.Y))
                        //};
                        foreach (var areaLine in areaLines)
                        {
                            if (line.IntersectsWith(areaLine))
                            {
                                intersects = true;
                                break;
                            }
                        }
                        if (intersects)
                            break;
                    }
                    if (!intersects)
                    {
                        _result = area.TotalArea.ToString();
                        return;
                    }
                    tryCount++;
                }
            }
        }

        //private Point GetIntersection()
        //{
        //    return new Point();
        //}

        private Area CalculateArea(Point a, Point b)
        {
            var area = 0L;
            var x = (long)Math.Abs(a.X - b.X) + 1;
            var y = (long)Math.Abs(a.Y - b.Y) + 1;
            return new Area(a, b, x * y);
        }

        public override void TestRun()
        {
            string input = "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3";
            Run(input);
        }
    }

    //internal class Point2D
    //{
    //    public int X { get; set; }
    //    public int Y { get; set; }
    //    public Point2D(int x, int y)
    //    {
    //        X = x; Y = y;
    //    }
    //}

    //internal class Line2D
    //{
    //    public Point2D Start { get; set; }
    //    public Point2D End { get; set; }
    //    public double Slope { get; set; }
    //    public Line2D(Point2D start, Point2D end)
    //    {
    //        Start = start; End = end;
    //        Slope = (double)(end.Y - start.Y) / (end.X - start.X);
    //    }
    //}

    internal static class Extensions2
    {
        public static bool IntersectsWith(this Vector first, Vector second)
        {
            var slope1 = first.Y / first.X;
            var slope2 = second.Y / second.X;
            if (slope1 == slope2)
                return false;

            //y = mx + b
            var b1 = first.Y - (slope1 * first.X);
            var b2 = second.Y - (slope2 * second.X);

            var intersectX = (b2 - b1) / (slope1 - slope2);
            var intersectY = (slope1 * intersectX) + b1;

            if (intersectY == ((slope2 * intersectX) + b2))
                return true;
            return false;


            //Vector3 lineVec3 = inLinePoint2 - inLinePoint1;
            //Vector3 crossVec1and2 = Vector3.Cross(inLineVec1, inLineVec2);
            //Vector3 crossVec3and2 = Vector3.Cross(lineVec3, inLineVec2);
            //float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //float dot = Vector3.Dot(crossVec3and2, crossVec1and2);
            //float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            //inIntersection = inLinePoint1 + (inLineVec1 * s);
            //return inIntersection;
        }
        //public static bool IntersectsWith(this Line2D first, Line2D second)
        //{
        //    if (Math.Abs(first.Slope).Equals(Math.Abs(second.Slope)))
        //        return false;

        //}

    }

    internal class Area
    {
        public Point First { get; set; }
        public Point Second { get; set; }
        public long TotalArea { get; set; }
        public Area(Point first, Point second, long area)
        {
            this.First = first;
            this.Second = second;
            this.TotalArea = area;
        }
    }
}
