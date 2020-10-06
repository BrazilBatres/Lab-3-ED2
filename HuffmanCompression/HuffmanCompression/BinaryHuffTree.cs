using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    class BinaryHuffTree
    {
        BinaryNode Root;
        public void Insertion(BinaryNode newValue, BinaryNode newValue2)
        {
            Root = new BinaryNode();
            Root.Priority = newValue.Priority + newValue2.Priority;
            Root.LeftSon = newValue;
            Root.RightSon = newValue2;
        }
        public BinaryNode GetRoot()
        {
            BinaryNode toReturn = Root;
            //Root = null;
            return toReturn;
        }

        public void PreOrder(Dictionary<char, string> KeyValuePairs)
        {
            if (Root != null)
            {
                RecursivePreOrder(KeyValuePairs, Root.LeftSon, "", false);
                RecursivePreOrder(KeyValuePairs, Root.RightSon, "", true);
            }
        }
        void RecursivePreOrder(Dictionary<char, string> KeyValuePairs, BinaryNode Actual, string prefixCode, bool right)
        {
            if (!right) prefixCode += '0';
            else prefixCode += '1';

            if (Actual.RightSon != null)
            {
                RecursivePreOrder(KeyValuePairs, Actual.LeftSon, prefixCode, false);
                RecursivePreOrder(KeyValuePairs, Actual.RightSon, prefixCode, true);
            }
            else
            {
                KeyValuePairs.Add(Actual.character, prefixCode);
            }

        }
        
    }
}
