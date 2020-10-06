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
            var comparator = (BinaryNode)obj;

            if (character.CompareTo(comparator.character) > 0)
            {
                return 1;
            }
            else if (character.CompareTo(comparator.character) < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
