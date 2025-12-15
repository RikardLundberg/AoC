using System.Windows;

namespace Implementations._2025
{
    internal class Day9 : Day
    {
        private List<NonAngleLine> verticalLines = []; //new Dictionary<int, (int Low, int High)>();
        private List<NonAngleLine> horizontalLines = []; //var horizontalLines = new Dictionary<int, (int Low, int High)>();
        private int xCount = 0;
        private int yCount = 0;
        private List<MyPoint> points = new List<MyPoint>();

        public override void Run(string puzzleData)
        {
            foreach (var line in puzzleData.Split('\n'))
            {
                var coords = line.Split(',');
                if (coords.Length != 2)
                    continue;

                var x = int.Parse(coords[0]);
                var y = int.Parse(coords[1]);
                xCount = x > xCount ? x : xCount;
                yCount = y > yCount ? y : yCount;
                points.Add(new MyPoint(x, y));
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
                int id = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    var start = points[i];
                    var end = i == points.Count - 1 ? points[0] : points[i + 1];
                    var prevPoint = i == 0 ? points[points.Count - 1] : points[i - 1];
                    var nextPoint = i == points.Count - 1 ? points[1] : (i == points.Count - 2 ? points[0] : points[i + 2]);  //stupid
                    if (start.X == end.X)
                    {
                        var interval = new Interval();
                        interval.Low = start.Y < end.Y ? start.Y : end.Y;
                        interval.High = start.Y < end.Y ? end.Y : start.Y;

                        //var interval = start.Y < end.Y ? ((int)start.Y, (int)end.Y) : ((int)end.Y, (int)start.Y);
                        var trans = (prevPoint.X < start.X && nextPoint.X > start.X) || (prevPoint.X > start.X && nextPoint.X < start.X);
                        verticalLines.Add(new NonAngleLine() { Id = id++, Limit = start.X, Interval = interval, Trans = trans });
                    }
                    else
                    {
                        var interval = new Interval();
                        interval.Low = start.X < end.X ? start.X : end.X;
                        interval.High = start.X < end.X ? end.X : start.X;

                        //var interval = start.X < end.X ? ((int)start.X, (int)end.X) : ((int)end.X, (int)start.X);
                        var trans = (prevPoint.Y < start.Y && nextPoint.Y > start.Y) || (prevPoint.Y > start.Y && nextPoint.Y < start.Y);
                        horizontalLines.Add(new NonAngleLine() { Id = id++, Limit = start.Y, Interval = interval, Trans = trans });
                    }
                }

                var someNum = 1000;

                foreach (var line in verticalLines)
                    line.DivideBySomeNum(someNum);
                foreach (var line in horizontalLines)
                    line.DivideBySomeNum(someNum);

                verticalLines.Sort((a, b) => a.Limit.CompareTo(b.Limit));
                horizontalLines.Sort((a, b) => a.Limit.CompareTo(b.Limit));

                var currentIndex = 0;

                //Visualize();
                //return;

                // /= 100 and fix the gaps, then return the totaledArea (which we already have)

                foreach (var p in points)
                {
                    p.X /= someNum; p.Y /= someNum;
                }
                List<Area> allResults = [];

                while (currentIndex < areas.Count)
                {
                    //4594476040 4609258448 // too high
                    //19927600 // not right 
                    //178479512
                    //1050938988
                    var area = areas[currentIndex];
                    //area.DivideBySomeNum(someNum);

                    //if (area.TotalArea == 4609258448)
                    if((area.First.X == 94 && area.First.Y == 48) || (area.Second.X == 94 && area.Second.Y == 48))
                    {
                        string hej = "";
                    }
                    continue;
                    var corners = new List<MyPoint>()
                        {
                            area.First, area.Second, new MyPoint(area.First.X, area.Second.Y), new MyPoint(area.Second.X, area.First.Y)
                        };
                    bool allInside = true;
                    var allFramePoints = area.GetAllFramePoints();


                    foreach (var corner in allFramePoints)
                    {
                        if (!PointIsInside(corner))
                        {
                            allInside = false; break;
                        }
                    }
                    if (allInside)
                    {
                        //allResults.Add(area);
                        _result = area.TotalArea.ToString();
                        break;
                    }
                    currentIndex++;

                }
                
            }

        }

        public override void Reset()
        {
            base.Reset();
            horizontalLines.Clear();
            verticalLines.Clear();
            xCount = 0;
            yCount = 0;
            points.Clear();
        }

        private void Visualize()
        {
            string outputPath = @"c:\test\test.txt";
            var matrix = new List<List<char>>();
            xCount /= 1000; xCount += 1;
            yCount /= 1000; yCount += 1;
            for (int i = 0; i < yCount; i++)
            {
                var line = new List<char>();
                for (int j = 0; j < xCount; j++)
                {
                    line.Add('.');
                }
                matrix.Add(line);
            }

            //foreach(var point in points)
            //{
            //    point.X /= 1000; point.Y /= 1000;
            //}

            foreach (var line in horizontalLines)
            {
                matrix[line.Limit][line.Interval.Low] = 'X';
                matrix[line.Limit][line.Interval.High] = 'X';
                for (int i = line.Interval.Low + 1; i < line.Interval.High; i++)
                    matrix[line.Limit][i] = 'o';
                //matrix[(int)point.Y / 1000][(int)point.X / 1000] = 'X';

            }

            foreach (var line in verticalLines)
            {
                matrix[line.Interval.Low][line.Limit] = 'X';
                matrix[line.Interval.High][line.Limit] = 'X';
                for (int i = line.Interval.Low + 1; i < line.Interval.High; i++)
                    matrix[i][line.Limit] = 'o';
                //matrix[(int)point.Y / 1000][(int)point.X / 1000] = 'X';

            }

            using (StreamWriter sw = new StreamWriter(outputPath))
            {
                for (int y = 0; y < yCount; y++)
                {
                    var line = new string(matrix[y].ToArray());
                    sw.WriteLine(line);
                }
            }
        }

        private bool PointIsInside(MyPoint corner)
        {
            //probably better to figure out closest edge, but this is easier to write
            //var rayYStart = new Point(corner.X, 0);
            //var rayXStart = new Point(0, corner.Y);

            List<int> usedIds = new List<int>();

            bool inside = false;
            for (int i = 0; i <= corner.Y; i++)
            {
                foreach (var interval in horizontalLines.Where(a => a.Limit == i).Select(a => a.Interval))
                //foreach(var horizontalLine in horizontalLines)
                {

                    //if (!usedIds.Contains(horizontalLine.Id) && i > horizontalLine.Limit && corner.X <= horizontalLine.Interval.High && corner.X >= horizontalLine.Interval.Low)
                    if (corner.X <= interval.High && corner.X >= interval.Low)
                    {
                        //usedIds.Add(horizontalLine.Id);
                        if (!(i == corner.Y && inside)) //don't exit prematurely
                            inside = !inside;
                    }
                }
            }

            //if we passed through an interval we are still inside if it is a trans, otherwise we are now outside
            foreach (var transInterval in verticalLines.Where(a => a.Limit == corner.X && a.Trans).Select(a => a.Interval))
            {
                if (corner.Y > transInterval.High)
                    inside = !inside;
            }

            if (!inside)
                return false;
            inside = false;
            for (int i = 0; i <= corner.X; i++)
            {
                foreach (var interval in verticalLines.Where(a => a.Limit == i).Select(a => a.Interval))
                //foreach(var verticalLine in verticalLines)
                {
                    //if (usedIds.Contains(verticalLine.Id) && i > verticalLine.Limit && corner.Y <= verticalLine.Interval.High && corner.Y >= verticalLine.Interval.Low)
                    if (corner.Y <= interval.High && corner.Y >= interval.Low)
                    {
                        //usedIds.Add(verticalLine.Id);
                        if (!(i == corner.X && inside)) //don't exit prematurely
                            inside = !inside;
                    }
                }
            }

            //if we passed through an interval we are still inside if it is a trans, otherwise we are now outside
            foreach (var transInterval in horizontalLines.Where(a => a.Limit == corner.Y && a.Trans).Select(a => a.Interval))
            {
                if (corner.X > transInterval.High)
                    inside = !inside;
            }

            return inside;
        }

        private Area CalculateArea(MyPoint a, MyPoint b)
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


    public class MyPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public MyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Interval
    {
        public int Low { get; set; }
        public int High { get; set; }
    }

    public class NonAngleLine
    {
        public int Limit { get; set; }
        public int Id { get; set; }
        public Interval Interval { get; set; }
        public bool Trans { get; set; }

        public void DivideBySomeNum(int someNum)
        {
            Limit /= someNum;
            Interval.Low /= someNum;
            Interval.High /= someNum;
        }
    }

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
        public MyPoint First { get; set; }
        public MyPoint Second { get; set; }
        public long TotalArea { get; set; }
        public Area(MyPoint first, MyPoint second, long area)
        {
            this.First = first;
            this.Second = second;
            this.TotalArea = area;
        }

        public List<MyPoint> GetAllFramePoints()
        {
            List<MyPoint> points = new List<MyPoint>();
            var lowX = Math.Min(First.X, Second.X);
            var highX = Math.Max(First.X, Second.X);
            var lowY = Math.Min(First.Y, Second.Y);
            var highY = Math.Max(First.Y, Second.Y);

            for (int i = lowX; i <= highX; i++)
            {
                points.Add(new MyPoint(i, lowY));
                points.Add(new MyPoint(i, highY));
            }

            for(int i = lowY + 1; i < highY; i++)
            {
                points.Add(new MyPoint(lowY, i));
                points.Add(new MyPoint(highY, i));
            }

            return points;
        }

        //public void DivideBySomeNum(int someNum)
        //{
        //    First.X /= someNum;
        //    First.Y /= someNum;
        //    Second.X /= someNum;
        //    Second.Y /= someNum;
        //}
    }
}
