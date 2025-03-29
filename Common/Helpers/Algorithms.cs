using Common.Helpers.DataStructures;

namespace Common.Helpers
{
	public class Algorithms
	{
		public Algorithms()
		{
		}

        public static long CalcManhattanDistance(Position from, Position to)
        {
            int distanceX = Math.Abs(to.X - from.X);
            int distanceY = Math.Abs(to.Y - from.Y);
            int distanceZ = Math.Abs(to.Z - from.Z);

            return distanceX + distanceY + distanceZ;
        }

		public static List<List<T>> GetAllPermutations<T>(List<T> nodes)
		{
            var result = new List<List<T>>();

            Permutations(nodes, 0, result);

            return result;
        }

        private static void Permutations<T>(List<T> nodes, int current, List<List<T>> result)
        {
            void Swap(List<T> nodes, int i, int j)
            {
                T tmp = nodes[i];
                nodes[i] = nodes[j];
                nodes[j] = tmp;
            }

            if (current == nodes.Count - 1)
            {
                result.Add(new List<T>(nodes));
                return;
            }

            for (int j = current; j < nodes.Count; j++)
            {
                Swap(nodes, current, j);

                Permutations(nodes, current + 1, result);

                Swap(nodes, current, j);
            }
        }

        /// <summary>
        /// Calculates are, restricted by set of coordinates, including edges
        /// For looped nodes that have Position
        /// </summary>
        public static int CalcGaussArea(List<MapNode> nodeMap)
        {
            MapNode startNode = nodeMap.First();

            nodeMap.ForEach(n => n.IsVisited = false);
            MapNode nextNode = startNode.Neighbours.First();

            int sum1 = 0;
            MapNode currentNode = startNode;
            while (true)
            {
                sum1 += currentNode.Pos.X * nextNode.Pos.Y;
                currentNode.IsVisited = true;
                currentNode = nextNode;

                var nextNodes = nextNode.Neighbours.Where(n => !n.IsVisited);
                nextNode = nextNodes.Any() ? nextNodes.First() : startNode;

                if (currentNode.Equals(startNode))
                {
                    break;
                }
            }


            nodeMap.ForEach(n => n.IsVisited = false);
            nextNode = startNode.Neighbours.First();

            int sum2 = 0;
            currentNode = startNode;
            while (true)
            {
                sum2 += currentNode.Pos.Y * nextNode.Pos.X;
                currentNode.IsVisited = true;
                currentNode = nextNode;

                var nextNodes = nextNode.Neighbours.Where(n => !n.IsVisited);
                nextNode = nextNodes.Any() ? nextNodes.First() : startNode;

                if (currentNode.Equals(startNode))
                {
                    break;
                }
            }

            int area = Math.Abs(sum1 - sum2)/2;
            return area;
        }

        /// <summary>
        /// Manacher's algorithm
        /// </summary>
        public static string LongestPalindromicSubstring(string str)
        {
            // preparations
            string preparedStr = "^#" + string.Join("#", str.ToCharArray()) + "#$";

            int[] palLengths = new int[preparedStr.Length];
            int currentCenter = 0, rightEdge = 0;

            for (int i = 1; i < preparedStr.Length - 1; i++)
            {
                palLengths[i] = (rightEdge > i) ? Math.Min(rightEdge - i, palLengths[2 * currentCenter - i]) : 0;

                // Extending palindrome with center at pos i
                while (preparedStr[i + palLengths[i] + 1] == preparedStr[i - palLengths[i] - 1])
                {
                    palLengths[i]++;
                }

                // Если палиндром с центром в i выходит за R, обновляем C и R
                if (i + palLengths[i] > rightEdge)
                {
                    currentCenter = i;
                    rightEdge = i + palLengths[i];
                }
            }

            // Найдем максимальное значение в P
            int maxLen = 0;
            int centerIndex = 0;
            for (int i = 1; i < preparedStr.Length - 1; i++)
            {
                if (palLengths[i] > maxLen)
                {
                    maxLen = palLengths[i];
                    centerIndex = i;
                }
            }

            int start = (centerIndex - maxLen) / 2;
            return str.Substring(start, maxLen);
        }

    }
}

