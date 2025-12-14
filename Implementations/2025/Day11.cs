using Common.Graphs;

namespace Implementations._2025
{
    internal class Day11 : Day
    {
        public override void Run(string puzzleData)
        {
            var graph = ParseGraph(puzzleData);
            if (!_secondStar)
            {
                var paths = graph.GetPathsToNode("out");
                _result = paths.Count.ToString();
            }
            else
            {
                // should figure out order programmatically as well .. or sort after getting all paths
                var pathsOut = graph.GetPathsToNode("out", "dac");
                var pathsToFft = graph.GetReversePathsToNode("svr", "fft");
                graph.DeleteChildren(graph.GetNode("dac"));
                graph.DeleteParents(graph.GetNode("fft"));

                var pathsToDac = graph.GetTotalPathCount("dac", "fft");

                long count = pathsOut.Count * pathsToFft.Count * pathsToDac;

                _result = count.ToString();
            }
        }

        private HierarchicalGraph ParseGraph(string puzzleData)
        {
            HierarchicalGraph graph = new HierarchicalGraph();
            foreach (var line in puzzleData.Split('\n'))
            {
                var colonSplit = line.Split(':');
                if (colonSplit.Length < 2)
                    continue;
                var name = colonSplit[0].Trim();
                var node = graph.GetNode(name);

                var valueSplit = colonSplit[1].Trim().Split(' ');
                foreach (var value in valueSplit)
                {
                    var valueNode = graph.GetNode(value);
                    node.AddChild(valueNode);
                }

                if (name.Equals("you", StringComparison.InvariantCultureIgnoreCase))
                    graph.TopNode = node;

            }

            return graph;
        }

        public override void TestRun()
        {
            var inputFirstStar = "aaa: you hhh\nyou: bbb ccc\nbbb: ddd eee\nccc: ddd eee fff\nddd: ggg\neee: out\nfff: out\nggg: out\nhhh: ccc fff iii\niii: out";
            var inputSecondStar = "svr: aaa bbb\naaa: fft\nfft: ccc\nbbb: tty\ntty: ccc\nccc: ddd eee\nddd: hub\nhub: fff\neee: dac\ndac: fff\nfff: ggg hhh\nggg: out\nhhh: out";
            var input = _secondStar ? inputSecondStar : inputFirstStar;
            Run(input);
        }
    }
}
