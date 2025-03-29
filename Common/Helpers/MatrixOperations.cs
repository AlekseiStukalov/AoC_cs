using Common.Helpers.DataStructures;

namespace Common.Helpers
{
    public class MatrixOperations
    {
        public static void ConnectGraphNodes<T>(List<List<T>> graph, bool connectDiagonalNodes = false,
            bool loopHorNodes = false, bool loopVertNodes = false)
            where T : MapNode
        {
            for (var y = 0; y < graph.Count; y++)
            {
                for (int x = 0; x < graph[y].Count; x++)
                {
                    var buff = new List<T?>
                    {
                        y > 0 ? graph[y - 1][x] : null,
                        y < graph.Count - 1 ? graph[y + 1][x] : null,
                        x < graph[y].Count - 1 ? graph[y][x + 1] : null,
                        x > 0 ? graph[y][x - 1] : null
                    };

                    if (loopHorNodes)
                    {
                        if (x == 0)
                        {
                            buff.Add(graph[y][graph[y].Count - 1]);
                        }
                        else if (x == graph[y].Count - 1)
                        {
                            buff.Add(graph[y][0]);
                        }
                    }

                    if (loopVertNodes)
                    {
                        if (y == 0)
                        {
                            buff.Add(graph[graph.Count - 1][x]);
                        }
                        else if (y == graph.Count - 1)
                        {
                            buff.Add(graph[0][x]);
                        }
                    }

                    if (connectDiagonalNodes)
                    {
                        buff.Add(y > 0 && x > 0 ? graph[y - 1][x - 1] : null);
                        buff.Add(y > 0 && x < graph[y].Count - 1 ? graph[y - 1][x + 1] : null);
                        buff.Add(y < graph.Count - 1 && x > 0 ? graph[y + 1][x - 1] : null);
                        buff.Add(y < graph.Count - 1 && x < graph[y].Count - 1 ? graph[y + 1][x + 1] : null);
                    }

                    for (int i = 0; i < buff.Count; i++)
                    {
                        MapNode? item = buff[i];
                        if (item != null)
                        {
                            graph[y][x].Neighbours.Add(item);
                        }
                    }
                }
            }
        }

        public static List<List<T>> RotateRight<T>(List<List<T>> matrix)
        {
            List<List<T>> result = new();
            for (int y = matrix.Count - 1; y >= 0; y--)
            {
                for (int x = 0; x < matrix[y].Count; x++)
                {
                    if (result.Count - 1 < x)
                    {
                        result.Add(new());
                    }

                    result[x].Add(matrix[y][x]);
                }
            }

            return result;
        }

        public static List<List<T>> FlipHorizontal<T>(List<List<T>> matrix)
        {
            List<List<T>> result = GetCopy(matrix);

            int lastRowIdx = matrix.Count - 1;
            for (int y = 0; y < matrix.Count; y++)
            {
                for (int x = 0; x < matrix[y].Count; x++)
                {
                    result[lastRowIdx - y][x] = matrix[y][x];
                }
            }

            return result;
        }

        public static List<List<T>> FlipVertical<T>(List<List<T>> matrix)
        {
            List<List<T>> result = GetCopy(matrix);

            for (int y = 0; y < matrix.Count; y++)
            {
                for (int x = 0; x < matrix[y].Count; x++)
                {
                    result[y][matrix[y].Count - 1 - x] = matrix[y][x];
                }
            }

            return result;
        }

        private static List<List<T>> GetCopy<T>(List<List<T>> matrix)
        {
            List<List<T>> copy = new();

            for (int y = 0; y < matrix.Count; y++)
            {
                copy.Add(new List<T>());
                for (int x = 0; x < matrix[y].Count; x++)
                {
                    copy[y].Add(matrix[y][x]);
                }
            }

            return copy;
        }
    }
}