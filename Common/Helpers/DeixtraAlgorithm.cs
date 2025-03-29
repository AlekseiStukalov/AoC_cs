using Common.Helpers.DataStructures;

namespace Common.Helpers
{
    public class DeixtraAlgorithm : DeixtraAlgorithm<MapNode>
    {
        public DeixtraAlgorithm(List<MapNode> graph, MapNode startNode, bool printOutput = true) : base(graph, startNode, printOutput){}
        public DeixtraAlgorithm(List<List<MapNode>> map, MapNode startNode, bool printOutput = true) : base(map, startNode, printOutput){ }
    }

    public class DeixtraAlgorithm<T> where T : MapNode
    {
        private Dictionary<int, Dictionary<int, int>> Distances;
        private List<T> NodesInTheQueue;
        private List<T> Graph;
        private T StartNode;
        private bool IsNeedPrintOutput;

        public DeixtraAlgorithm(List<T> graph, T startNode, bool printOutput = true)
        {
            Distances = new Dictionary<int, Dictionary<int, int>>();
            Graph = graph;
            StartNode = startNode;
            IsNeedPrintOutput = printOutput;

            NodesInTheQueue = new List<T>() { StartNode };
        }

        public DeixtraAlgorithm(List<List<T>> map, T startNode, bool printOutput = true)
            : this(map.SelectMany(n => n).ToList(), startNode, printOutput){}

        public void CalculateDistances()
        {
            CreateDistancesDictionary();

            if (IsNeedPrintOutput)
            {
                Console.Write($"Calculating all distances ({Distances.Count} nodes) ");
            }

            Graph.ForEach(n => n.IsVisited = false);
            Graph.ForEach(n => n.DeixtraMark = int.MaxValue);
            StartNode.DeixtraMark = 0;
            NodesInTheQueue.Add(StartNode);

            int visited = 0;

            while (visited < Graph.Count)
            {
                T closestNode = GetClosestNode();

                CheckNeighbours(closestNode);

                closestNode.IsVisited = true;
                visited++;

                NodesInTheQueue.Remove(closestNode);

                if(IsNeedPrintOutput && visited % 5000 == 0)
                {
                    Console.Write(".");
                }
            }

            if (IsNeedPrintOutput)
            {
                Console.WriteLine();
            }
        }

        public List<T> RestorePath_Optimized(T endNode)
        {
            var shortestPath = new LinkedList<T>();

            T? currentNode = endNode;
            while(currentNode != StartNode)
            {
                shortestPath.AddFirst(currentNode);
                currentNode = FindPreviousNode(currentNode);

                if (currentNode == null)
                    break;

                if (IsNeedPrintOutput)
                {
                    Console.Write(".");
                }
            }

            shortestPath.AddFirst(StartNode);

            return shortestPath.ToList();
        }

        public List<T> RestorePath(T endNode)
        {
            var shortestPath = new List<T>(Math.Max(Graph.Count/10, 10));

            T? currentNode = endNode;
            while(currentNode != StartNode)
            {
                shortestPath.Add(currentNode);
                currentNode = FindPreviousNode(currentNode);

                if (currentNode == null)
                    break;

                if (IsNeedPrintOutput)
                {
                    Console.Write(".");
                }
            }

            shortestPath.Add(StartNode);

            shortestPath.Reverse();
            return shortestPath;
        }

        private T? FindPreviousNode(T current)
        {
            foreach(T neighbour in current.Neighbours)
            {
                try
                {
                    int distance = Distances[neighbour.DeixtraArrayIndex][current.DeixtraArrayIndex];

                    if (current.DeixtraMark - distance == neighbour.DeixtraMark)
                    {
                        return neighbour;
                    }
                }
                catch{}
            }

            Console.WriteLine("Error. Can't find previous node");
            return null;
        }

        private void CheckNeighbours(T currentNode)
        {
            foreach (var neighbour in currentNode.Neighbours)
            {
                if (!neighbour.IsVisited)
                {
                    int distance = Distances[currentNode.DeixtraArrayIndex][neighbour.DeixtraArrayIndex];
                    int newMark = currentNode.DeixtraMark + distance;

                    if (newMark < neighbour.DeixtraMark)
                    {
                        neighbour.DeixtraMark = newMark;
                    }

                    if (!NodesInTheQueue.Contains(neighbour))
                    {
                        NodesInTheQueue.Add((T)neighbour);
                    }
                }
            }
        }

        private T GetClosestNode()
        {
            if (NodesInTheQueue.Any())
            {
                return NodesInTheQueue.OrderBy(n => n.DeixtraMark).First();
            }
            else
            {
                return Graph.Where(n => !n.IsVisited).OrderBy(n => n.DeixtraMark).First();
            }
        }

        private void CreateDistancesDictionary()
        {
            if (IsNeedPrintOutput)
            {
                Console.Write("Creating distances dictionary... ");
            }

            for (int i = 0; i < Graph.Count; i++)
            {
                Graph[i].DeixtraArrayIndex = i;
            }

            for (int y = 0; y < Graph.Count; y++)   //y - source; x - destination
            {
                Distances.Add(y, new Dictionary<int, int>());
                foreach (var neighbour in Graph[y].Neighbours)
                {
                    Distances[y].Add(neighbour.DeixtraArrayIndex, Graph[neighbour.DeixtraArrayIndex].Weight);
                }
            }

            if (IsNeedPrintOutput)
            {
                var totalElems = Distances.SelectMany(node => node.Value).Count();
                Console.WriteLine($"Total distances = {totalElems}");
            }
        }
    }
}

