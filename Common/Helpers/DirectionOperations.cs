using Common.Helpers.DataStructures;
using Common.Helpers.Enums;

namespace Common.Helpers
{
    public static class DirectionOperations
    {
        private static LinkedList<Direction> Directions90;
        private static LinkedList<Direction> Directions45;

        static DirectionOperations()
        {
            Directions90 = new ();
            Directions90.AddLast(Direction.Up);
            Directions90.AddLast(Direction.Right);
            Directions90.AddLast(Direction.Down);
            Directions90.AddLast(Direction.Left);

            Directions45 = new ();
            Directions45.AddLast(Direction.Up);
            Directions45.AddLast(Direction.UpRight);
            Directions45.AddLast(Direction.Right);
            Directions45.AddLast(Direction.DownRight);
            Directions45.AddLast(Direction.Down);
            Directions45.AddLast(Direction.DownLeft);
            Directions45.AddLast(Direction.Left);
            Directions45.AddLast(Direction.UpLeft);
        }

        public static Direction GetRelativeDirection(Position start, Position end)
        {
            if (start.X > end.X)
            {
                if (start.Y > end.Y)
                {
                    return Direction.UpLeft;
                }
                else if (start.Y < end.Y)
                {
                    return Direction.DownLeft;
                }
                else
                {
                    return Direction.Left;
                }
            }
            else if (start.X < end.X)
            {
                if (start.Y > end.Y)
                {
                    return Direction.UpRight;
                }
                else if (start.Y < end.Y)
                {
                    return Direction.DownRight;
                }
                else
                {
                    return Direction.Right;
                }
            }
            else
            {
                if (start.Y > end.Y)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Down;
                }
            }
        }

        public static Direction TurnRight90(Direction currentDirection) => Turn(currentDirection, Directions90, isRight: true, "TurnRight90");
        public static Direction TurnLeft90(Direction currentDirection) => Turn(currentDirection, Directions90, isRight: false, "TurnLeft90");
        public static Direction TurnRight45(Direction currentDirection) => Turn(currentDirection, Directions45, isRight: true, "TurnRight45");
        public static Direction TurnLeft45(Direction currentDirection) => Turn(currentDirection, Directions45, isRight: false, "TurnLeft45");

        private static Direction Turn(Direction currentDirection, LinkedList<Direction> srcDirections, bool isRight, string targetTaskName)
        {
            LinkedListNode<Direction>? dirNode = srcDirections.Find(currentDirection);

            if (dirNode != null)
            {
                return isRight ? GetNextNodeCycled(dirNode) : GetPrevNodeCycled(dirNode);
            }
            else
            {
                Console.WriteLine($"{targetTaskName}: Wrong input direction");
                return Direction.Up;
            }
        }

        private static Direction GetPrevNodeCycled(LinkedListNode<Direction> listNode)
        {
            LinkedListNode<Direction>? prev = listNode.Previous;
            if (prev == null)
            {
                LinkedListNode<Direction>? last = Directions90.Last;
                if (last != null)
                {
                    return last.Value;
                }
                
                Console.WriteLine($"GetPrevNodeCycled: Empty list?");
                return Direction.Up;
            }

            return prev.Value;
        }

        private static Direction GetNextNodeCycled(LinkedListNode<Direction> listNode)
        {
            LinkedListNode<Direction>? next = listNode.Next;
            if (next == null)
            {
                LinkedListNode<Direction>? first = Directions90.First;
                if (first != null)
                {
                    return first.Value;
                }
                
                Console.WriteLine($"GetNextNodeCycled: Empty list?");
                return Direction.Up;
            }

            return next.Value;
        }
    }
}