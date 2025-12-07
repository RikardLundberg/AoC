namespace Common.Graphs
{
    public class GridVertex
    {
        public int X { get; }
        public int Y { get; }
        public List<GridVertex> Neighbors { get; } = new();
        public GridVertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static List<GridVertex> ParseGridVertices(string[] grid, char vertexSymbol)
        {
            List<GridVertex> vertices = new();
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] == vertexSymbol)
                    {
                        var vertex = new GridVertex(x, y);
                        AddNeighbors(vertex, vertices);
                        vertices.Add(vertex);
                    }
                }
            }
            return vertices;
        }

        private static void AddNeighbors(GridVertex vertex, List<GridVertex> allVertices)
        {
            foreach (var potentialNeighbor in allVertices)
            {
                if (potentialNeighbor.X == vertex.X && Math.Abs(potentialNeighbor.Y - vertex.Y) == 1 ||
                    potentialNeighbor.Y == vertex.Y && Math.Abs(potentialNeighbor.X - vertex.X) == 1 ||
                    Math.Abs(potentialNeighbor.X - vertex.X) == 1 && Math.Abs(potentialNeighbor.Y - vertex.Y) == 1 ||
                    Math.Abs(potentialNeighbor.Y - vertex.Y) == 1 && Math.Abs(potentialNeighbor.X - vertex.X) == 1)
                {
                    vertex.Neighbors.Add(potentialNeighbor);
                    potentialNeighbor.Neighbors.Add(vertex);
                }
            }
        }

        public static bool operator ==(GridVertex left, GridVertex right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(GridVertex left, GridVertex right)
        {
            return !(left == right);
        }
    }
}
