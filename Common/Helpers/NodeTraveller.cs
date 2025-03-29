using Common.Helpers.DataStructures;

namespace Common.Helpers
{
    public class NodeTraveller : NodeTraveller<MapNode>
    {
        public NodeTraveller(MapNode current, bool saveStory) : base(current, saveStory){}

        public override NodeTraveller CreateInstance(MapNode nextNode)
        {
            return new NodeTraveller(nextNode, false);
        }
        public override NodeTraveller GetNextTraveller(MapNode nextNode, int addedWeight = 0)
        {
            return (NodeTraveller)base.GetNextTraveller(nextNode, addedWeight);
        }
    }

    public class NodeTraveller<T> : IEquatable<NodeTraveller<T>> where T : MapNode
    {
        public bool SaveStory;
        public T CurrentNode;
        public List<T> VisitedNodes;
        public int WalkedDistance;
        public int WayWeight;

        public NodeTraveller(T current, bool saveStory)
        {
            CurrentNode = current;
            VisitedNodes = new List<T>();
            SaveStory = saveStory;
            WayWeight = 0;

            if (SaveStory)
            {
                VisitedNodes.Add(current);
            }
        }

        public virtual NodeTraveller<T> CreateInstance(T nextNode)
        {
            return new NodeTraveller<T>(nextNode, false);
        }

        public virtual NodeTraveller<T> GetNextTraveller(T nextNode, int addedWeight = 0)
        {
            NodeTraveller<T> nt = CreateInstance(nextNode);
            nt.WalkedDistance = this.WalkedDistance + 1;
            nt.WayWeight = this.WayWeight + addedWeight;
            nt.SaveStory = this.SaveStory;

            CopyStory(nextNode, nt);

            return nt;
        }

        protected void CopyStory(T nextNode, NodeTraveller<T> t)
        {
            if (SaveStory)
            {
                foreach (var node in this.VisitedNodes)
                {
                    t.VisitedNodes.Add(node);
                }

                t.VisitedNodes.Add(nextNode);
            }
        }

        public bool Equals(NodeTraveller<T>? other)
        {
            if (other == null)
                return false;

            return other.GetHashCode() == this.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is NodeTraveller<T>))
            {
                return false;
            }
            return Equals(obj as NodeTraveller<T>);
        }

        public override int GetHashCode()
        {
            return CurrentNode.GetHashCode();
        }
    }
}
