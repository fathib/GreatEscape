using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GreatEscape
{



    public class Board
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _myId;
        private Point[] _playersPositions;
        private Node[,] _map;
        private List<Wall> _walls;
        private Direction _mainDirection;
        private Node _endNode;


        public Node MyPosition
        {
            get
            {
                Point position = _playersPositions[_myId];
                Node node = _map[position.X, position.Y];
                return node;
            }
        }

        public string GetNextOrder()
        {
            string order = String.Empty;

            Point position = _playersPositions[_myId];
            Node node = _map[position.X, position.Y];
            var myPath = FindPath(node);

            var nextPosition = myPath[0];

            if (nextPosition.X > position.X)
                order = "RIGHT";
            else if (nextPosition.X < position.X)
                order = "LEFT";
            else if (nextPosition.Y > position.Y)
                order = "DOWN";
            else if (nextPosition.Y < position.Y)
                order = "UP";

            return order;

        }
      
        public Board(int width, int height, int myId, int playerCount)
        {
            _width = width;
            _height = height;
            _myId = myId;
            _walls = new List<Wall>();
            _playersPositions = new Point[playerCount];
            _map = new Node[width, height];

            if (myId == 0)
                _mainDirection = Direction.Right;
            else if (myId == 1)
                _mainDirection = Direction.Left;
            else if (myId == 2)
                _mainDirection = Direction.Top;
            else
                _mainDirection = Direction.Down;

            LoadMap();
        }

        private void LoadMap()
        {
            for (int x = 0; x < _width; x++)
                for (int y = 0; y < _height; y++)
                    _map[x, y] = new Node(x, y);
        }

        public void LoadWall(int x, int y, string orientation)
        {
            Point p = new Point(x, y);
            Orientation o = (orientation == "H") ? Orientation.Horizontal : Orientation.Veritcal;
            _walls.Add(new Wall(p, o));
        }

        public void LoadPlayersPositions(int x, int y, int Id)
        {
            _playersPositions[Id] = new Point(x, y);
        }

        public List<Point> FindPath(Node myNode)
        {
            // The start node is the first entry in the 'open' list
            List<Point> path = new List<Point>();

          

            bool success = Search(myNode);
            if (success)
            {
                // If a path was found, follow the parents from the end node to build a list of locations
                Node node = this._endNode;
                while (node.ParentNode != null)
                {
                    path.Add(node.Position);
                    node = node.ParentNode;
                }

                // Reverse the list so it's in the correct order when returned
                path.Reverse();
            }

            return path;
        }

        private bool Search(Node currentNode)
        {
            // Set the current node to Closed since it cannot be traversed more than once
           
            List<Node> nextNodes= new List<Node>();
            nextNodes.Add(currentNode);
          
            while (nextNodes.Count > 0)
            {
                var nextNode = nextNodes[0];
                nextNodes.RemoveAt(0);
                nextNode.State = NodeState.Closed;
                if (IsEndLocation(nextNode))
                {
                    _endNode = nextNode;
                    return true;
                }
              
                nextNodes.AddRange(GetAdjacentWalkableNodes(nextNode));
                nextNodes.Sort((node1, node2) => node1.DistanceFromStart.CompareTo(node2.DistanceFromStart));

            }
 

            // The method returns false if this path leads to be a dead end
            return false;
        }

        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {

            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Node> nextLocations = GetAdjacentLocations(fromNode);

            foreach (var node in nextLocations)
            {

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    
                    int costFromStarWithThisParentNode = fromNode.DistanceFromStart + 1;
                    //we find a shortest path
                    if (costFromStarWithThisParentNode < node.DistanceFromStart)
                    {
                        node.ParentNode = fromNode;
                        node.DistanceFromStart = costFromStarWithThisParentNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.DistanceFromStart = fromNode.DistanceFromStart + 1;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }


        //Get all reachable nodes (filter borders and walls)
        private List<Node> GetAdjacentLocations(Node currentNode)
        {
            List<Node> nodes = new List<Node>();

            var y = currentNode.Position.Y;
            var x = currentNode.Position.X;

            if (x < _width - 1 && CanMove(currentNode, Direction.Right))
            {
                nodes.Add(_map[x + 1, y]);
            }
            if (y < _height - 1 && CanMove(currentNode, Direction.Down))
            {
                nodes.Add(_map[x, y + 1]);
            }
            if (y > 0 && CanMove(currentNode, Direction.Top))
            {
                nodes.Add(_map[x, y - 1]);
            }
            if (x > 0 && CanMove(currentNode, Direction.Left))
            {
                nodes.Add(_map[x - 1, y]);
            }


            return nodes;
        }


        private bool CanMove(Node currentNode, Direction direction)
        {
            //try to find a wall able to block the move
            //if he doesnt exist, we can move
            
            List<Point> possibleWallPosition = new List<Point>();
            Orientation wallOrientation = (direction == Direction.Right || direction == Direction.Left)
                ? Orientation.Veritcal
                : Orientation.Horizontal;

            if (direction == Direction.Right)
            {
                //
                //  |
                // x||
                //   |

                possibleWallPosition.Add(new Point(currentNode.Position.X + 1, currentNode.Position.Y));
                possibleWallPosition.Add(new Point(currentNode.Position.X + 1, currentNode.Position.Y - 1));
            }
            else if (direction == Direction.Left)
            {
                //
                //  |
                //  ||x
                //   |

                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y));
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y - 1));
            }
            else if (direction == Direction.Top)
            {
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y));
                possibleWallPosition.Add(new Point(currentNode.Position.X - 1, currentNode.Position.Y));
            }
            else if (direction == Direction.Down)
            {
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y + 1));
                possibleWallPosition.Add(new Point(currentNode.Position.X - 1, currentNode.Position.Y + 1));
            }

            var blockerWall = (from w in _walls
                               join y in possibleWallPosition on w.Position equals y
                               where w.Orientation == wallOrientation
                               select w).FirstOrDefault();

            return blockerWall == null;

        }


        private bool IsEndLocation(Node nextNode)
        {

            if (_mainDirection == Direction.Right)
                return nextNode.Position.X == _width - 1;
            else if (_mainDirection == Direction.Left)
                return nextNode.Position.X == 0;
            else if (_mainDirection == Direction.Top)
                return nextNode.Position.Y == 0;
            else
                return nextNode.Position.Y == _height - 1;
        }


    }
}