using AoC.Common.Graphs;

namespace AoC.Implementations._2025
{
    internal class Day4 : Day
    {
        public override void Run(string puzzleData)
        {
            string[] lines = puzzleData.Split('\n');
            List<GridVertex> vertices = GridVertex.ParseGridVertices(lines, '@');

            if (!_secondStar)
                FirstStar(vertices);
            else
                SecondStar(vertices);
        }

        private void SecondStar(List<GridVertex> vertices)
        {
            var count = 0;
            var accessibles = GetAccessible(vertices);
            while (accessibles.Count > 0)
            {
                count += accessibles.Count;
                foreach (var access in accessibles)
                {
                    vertices.RemoveVertex(access);
                }
                accessibles = GetAccessible(vertices);
            }
            _result = count.ToString();
        }

        private void FirstStar(List<GridVertex> vertices)
        {
            int totalVertices = GetAccessible(vertices).Count;
            _result = totalVertices.ToString();
        }

        private List<GridVertex> GetAccessible(List<GridVertex> vertices)
        {
            var accessors = new List<GridVertex>();
            foreach (GridVertex vertex in vertices)
            {
                if (vertex.Neighbors.Count < 4)
                    accessors.Add(vertex);
            }
            return accessors;
        }

        public override void TestRun()
        {
            string input = "..@@.@@@@.\n@@@.@.@.@@\n@@@@@.@.@@\n@.@@@@..@.\n@@.@@@@.@@\n.@@@@@@@.@\n.@.@.@.@@@\n@.@@@.@@@@\n.@@@@@@@@.\n@.@.@@@.@.";
            Run(input);
        }
    }

    internal static class extensions
    {
        internal static void RemoveVertex(this List<GridVertex> list, GridVertex v)
        {
            foreach (GridVertex v1 in v.Neighbors)
            {
                v1.Neighbors.Remove(v);
            }
            list.Remove(v);
        }
    }
}
