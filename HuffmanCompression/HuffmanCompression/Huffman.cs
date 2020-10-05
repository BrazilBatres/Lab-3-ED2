using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
   public class Huffman: ICompression
    {
        Dictionary<char, Character> Characters = new Dictionary<char, Character>();
        HeapQueue<BinaryNode> PriorityQueue = new HeapQueue<BinaryNode>();
        BinaryHuffTree huffTree = new BinaryHuffTree();
        double totalCharQuantity;
        public byte[] Compress(string ToCompresstxt)
        {
            //Método para tomar frecuencias de caracteres
            AssignPrefixCodes();
            

        }
         
        string CharacterSubstitution(byte Text)
        {
            string bitText = "";
            for (int i = 0; i < Text.Length; i++)
            {
                bitText+= Characters.TryGetValue()
            }
        }
        void AssignPrefixCodes()
        {
            foreach (var item in Characters)
            {
                BinaryNode actual = new BinaryNode();
                actual.character = item.Key;
                actual.Priority = item.Value.frecuency / totalCharQuantity;
                PriorityQueue.Add(actual);
            }
            bool exit = false;
            BinaryNode Auxnode = new BinaryNode();
            while (!exit)
            {
                //hay la posibilidad de que haya solo un caracter?
                if (!PriorityQueue.IsEmpty())
                {
                    Auxnode = PriorityQueue.Remove();
                }
                if (!PriorityQueue.IsEmpty())
                {
                    huffTree.Insertion(Auxnode, PriorityQueue.Remove());

                }
                if (!PriorityQueue.IsEmpty())
                {

                    PriorityQueue.Add(huffTree.GetRoot());
                }
                else
                {
                    exit = true;
                }
            }
            Dictionary<char, string> PrefixCodes = new Dictionary<char, string>();
            huffTree.PreOrder(PrefixCodes);
            foreach (var item in Characters)
            {
                PrefixCodes.TryGetValue(item.Key, out string currentPrefixCode);
                item.Value.prefixcode = currentPrefixCode;
            }
        }

        public string Decompress(string CompressedTxt)
        {
            throw new NotImplementedException();
        }

        
    }
}
