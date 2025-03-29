using Common.Helpers.DataStructures;

namespace Common.Helpers
{
    public class A_StarNode
    {
        public int CostToStartNode;
        public int HeuristicCostToEndNode;

        public int TotalCost => CostToStartNode + HeuristicCostToEndNode;

        public List<A_StarNode> Neighbours;

        public A_StarNode? Parent;
        public MapNode Data;
        public A_StarNode(MapNode nodeData)
        {
            Data = nodeData;
            Neighbours = new List<A_StarNode>();
        }
    }

    public class A_Star<T> where T : MapNode
    {
        private Dictionary<T, A_StarNode> DataNodeToGraphNode;
        private HashSet<A_StarNode> Graph;

        private A_StarNode StartNode;
        private A_StarNode EndNode;
        private List<A_StarNode> OpenNodes;
        private bool IsNeedPrintOutput;

        public A_Star(List<T> graph, T startNode, T endNode, out bool success, bool printOutput = true)
        {
            IsNeedPrintOutput = printOutput;
            success = true;
            OpenNodes = new List<A_StarNode>();
            DataNodeToGraphNode = new();
            Graph = new();

            graph.ForEach(n => n.IsVisited = false);
            Initialize(graph);
            var node = FindNodeByData(startNode);
            StartNode = node ?? new A_StarNode(startNode);
            success = success && node !=null;

            node = FindNodeByData(endNode);
            EndNode = node ?? new A_StarNode(endNode);
            success = success && node !=null;
            success = success && graph.All(n => n.HasCoordinates);
        }

        public A_Star(List<List<T>> map, T startNode, T endNode, out bool success, bool printOutput = true)
            : this(map.SelectMany(n => n).ToList(), startNode, endNode, out success, printOutput)
        {
        }

        public long CalculateDistance()
        {
            Console.Write(IsNeedPrintOutput ? $"Looking for best way in {Graph.Count} nodes " : "");

            long distance = CalculateDistanceInternal();

            if (IsNeedPrintOutput && distance >= 0)
            {
                Console.WriteLine();
            }

            return distance;
        }

        private long CalculateDistanceInternal()
        {
            OpenNodes.Add(StartNode);
            int visited = 0;

            while (OpenNodes.Count > 0)
            {
                A_StarNode currentNode = OpenNodes[0];
                foreach (var node in OpenNodes)
                {
                    if (node.TotalCost < currentNode.TotalCost || 
                        node.TotalCost == currentNode.TotalCost && node.HeuristicCostToEndNode < currentNode.HeuristicCostToEndNode)
                    {
                        currentNode = node;
                    } 
                }

                OpenNodes.Remove(currentNode);
                currentNode.Data.IsVisited = true;
                visited++;

                if (currentNode == EndNode)
                {
                    return currentNode.CostToStartNode + EndNode.Data.Weight - StartNode.Data.Weight;
                }

                int newCostToStart = currentNode.CostToStartNode + currentNode.Data.Weight;
                foreach(var neighbour in currentNode.Neighbours)
                {
                    if (neighbour.Data.IsVisited)
                    {
                        continue;
                    }

                    bool isExistInOpened = OpenNodes.Contains(neighbour);
                    if (newCostToStart < neighbour.CostToStartNode || !isExistInOpened)
                    {
                        neighbour.CostToStartNode = newCostToStart;
                        neighbour.HeuristicCostToEndNode = (int)Algorithms.CalcManhattanDistance(neighbour.Data.Pos, EndNode.Data.Pos);
                        neighbour.Parent = currentNode;

                        if (!isExistInOpened)
                        {
                            OpenNodes.Add(neighbour);
                        }
                    }
                }

                if(IsNeedPrintOutput && visited % 7000 == 0)
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine("\nFailed to find End Node");

            return -1;
        }

        public List<T> RestorePath()
        {
            return RestorePathInternal().Select(n => (T)n.Data).ToList();
        }

        private List<A_StarNode> RestorePathInternal()
        {
            List<A_StarNode> path = new();
            A_StarNode? currentNode = EndNode;

            while (currentNode != null && currentNode != StartNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            path.Add(StartNode);
            path.Reverse();
            return path;
        }

        private void Initialize(List<T> dataGraph)
        {
            if (IsNeedPrintOutput)
            {
                Console.WriteLine("Initializing internal graph... ");
            }

            foreach(T dataNode in dataGraph)
            {
                var graphNode = new A_StarNode(dataNode);
                DataNodeToGraphNode.Add(dataNode, graphNode);
                Graph.Add(graphNode);
            }

            foreach (A_StarNode graphNode in Graph)
            {
                foreach(T dataNodeNeighbour in graphNode.Data.Neighbours)
                {
                    graphNode.Neighbours.Add(DataNodeToGraphNode[dataNodeNeighbour]);
                }
            }
        }

        private A_StarNode? FindNodeByData(T dataNode)
        {
            return DataNodeToGraphNode.ContainsKey(dataNode) ? DataNodeToGraphNode[dataNode] : null;
        }
    }
}