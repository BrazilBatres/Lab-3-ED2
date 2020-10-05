using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    class BinaryNode:IPriority, IComparable
    {
        public char character;
        public BinaryNode RightSon;
        public BinaryNode LeftSon;

        public double Priority { get; set; }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
