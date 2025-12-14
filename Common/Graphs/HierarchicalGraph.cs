namespace Common.Graphs
{
    public class HierarchicalGraph
    {
        public HierarchicalNode TopNode { get; set; }

        private Dictionary<string, HierarchicalNode> nodes = new Dictionary<string, HierarchicalNode>();

        public HierarchicalNode GetNode(string name)
        {
            if (nodes.ContainsKey(name))
                return nodes[name];
            HierarchicalNode node = new HierarchicalNode(name);
            nodes.Add(name, node);
            return node;
        }

        public void DeleteParents(HierarchicalNode node)
        {
            foreach(var parent in node.Parents)
                DeleteParents(parent);
            node.Parents.Clear();
        }

        public void DeleteChildren(HierarchicalNode node)
        {
            foreach (var child in node.Children)
                DeleteChildren(child);
            node.Children.Clear();
        }

        public List<HierarchicalPath> GetPathsToNode(string endNodeName, string startNodeName = "")
        {
            paths = new List<HierarchicalPath>();
            var node = startNodeName == "" ? TopNode : GetNode(startNodeName);
            GetPathsToNode(endNodeName, new HierarchicalPath(), node);
            return paths;
        }

        public List<HierarchicalPath> GetReversePathsToNode(string endNodeName, string startNodeName = "")
        {
            paths = new List<HierarchicalPath>();
            var node = startNodeName == "" ? TopNode : GetNode(startNodeName);
            GetReversePathsToNode(endNodeName, new HierarchicalPath(), node);
            return paths;
        }

        public long GetTotalPathCount(string endNodeName, string startNodeName = "")
        {
            foundCount = new Dictionary<string, int>();
            var node = startNodeName == "" ? TopNode : GetNode(startNodeName);
            return GetTotalPathCount(endNodeName, node);
        }

        private Dictionary<string, int> foundCount = new Dictionary<string, int>();
        private int GetTotalPathCount(string endNodeName, HierarchicalNode currentNode)
        {
            if(currentNode.Name.Equals(endNodeName))
            {
                return 1;
            }
            var count = 0;
            foreach(var child in currentNode.Children)
            {
                if (foundCount.ContainsKey(child.Name))
                {
                    count += foundCount[child.Name];
                }
                else
                {
                    var childCount = GetTotalPathCount(endNodeName, child);
                    foundCount[child.Name] = childCount;
                    count += childCount;
                }
            }
            return count;
        }

        private void GetReversePathsToNode(string endNodeName, HierarchicalPath currentPath, HierarchicalNode currentNode)
        {
            currentPath.AddNode(currentNode);
            if (currentNode.Name.Equals(endNodeName))
            {
                paths.Add(currentPath);
                return;
            }

            foreach (var node in currentNode.Parents)
            {
                if (!currentPath.Contains(node.Name))
                    GetReversePathsToNode(endNodeName, new HierarchicalPath(currentPath), node);
            }
        }

        //move to HierarchicalPath?
        private List<HierarchicalPath> paths { get; set; } = new List<HierarchicalPath>();
        private void GetPathsToNode(string endNodeName, HierarchicalPath currentPath, HierarchicalNode currentNode)
        {
            currentPath.AddNode(currentNode);
            if (currentNode.Name.Equals(endNodeName))
            {
                paths.Add(currentPath);
                return;
            }

            foreach (var node in currentNode.Children)
            {
                if(!currentPath.Contains(node.Name))
                    GetPathsToNode(endNodeName, new HierarchicalPath(currentPath), node);
            }
        }
    }
}
