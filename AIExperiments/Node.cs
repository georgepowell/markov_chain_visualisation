using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIExperiments
{
    class Node
    {
        public int Value { get; set; }
        public bool IsTerminal { get; set; }

        public Node Node1 { get; set; }
        public Node Node2 { get; set; }

        public Node(int terminalValue)
        {
            Value = terminalValue;
            IsTerminal = true;
        }

        public Node(Node node1, Node node2)
        {
            IsTerminal = false;

            Node1 = node1;
            Node2 = node2;
        }

        public bool NodeEquals(Node node)
        {
            if (IsTerminal != node.IsTerminal)
                return false;

            if (IsTerminal)
                return true;

            return Node1.NodeEquals(node.Node1) && Node2.NodeEquals(node.Node2);
        }
    }
}
