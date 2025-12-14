namespace Common.Graphs
{
    public class HierarchicalNode
    {
        public string Name { get; set; }
        public List<HierarchicalNode> Children { get; set; }
        public List<HierarchicalNode> Parents { get; set; }

        public HierarchicalNode(string name) 
        { 
            this.Name = name; 
            Children = new List<HierarchicalNode>();
            Parents = new List<HierarchicalNode>();
        }

        public void AddChild(HierarchicalNode node)
        {
            Children.Add(node);
            node.Parents.Add(this);
        }
    }
}
