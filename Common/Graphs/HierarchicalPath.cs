using System.IO;

namespace Common.Graphs
{
    public class HierarchicalPath
    {
        private List<HierarchicalNode> Nodes { get; set; } = new List<HierarchicalNode>();
        public void AddNode(HierarchicalNode node) { Nodes.Add(node); }
        public bool Contains(string nodeName) { return Nodes.Where(a => a.Name == nodeName).Any(); }
        public HierarchicalPath() { }

        public HierarchicalPath(HierarchicalPath path)
        {
            foreach (var node in path.Nodes)
                AddNode(node);
        }

        public void Print()
        {
            using (StreamWriter streamWriter = new StreamWriter(@"c:\test\test2.txt", append: true))
            {
                var listNames = Nodes.Select(a => a.Name).ToList();
                streamWriter.WriteLine(string.Join(" -> ", listNames));
            }
        }
    }
}
