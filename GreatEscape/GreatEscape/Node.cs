using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatEscape
{
    public class Node
    {

        public Node ParentNode { get; set; }

        public int DistanceFromStart { get; set; }

        public Point Position { get; set; }
        public NodeState State { get; set; }

        public Node(int x, int y)
        {
            Position = new Point(x,y);
            State= NodeState.Untested;
        }

        public static int GetTraversalCost()
        {
            return 1;
        }
    }
}
