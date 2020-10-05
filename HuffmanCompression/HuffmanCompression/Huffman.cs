using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    class Huffman: ICompression
    {
        Dictionary<char, Character> Characters = new Dictionary<char, Character>();
        HeapQueue<BinaryNode> PriorityQueue = new HeapQueue<BinaryNode>();
        BinaryHuffTree huffTree = new BinaryHuffTree();
        double totalCharQuantity;
        public string Compress(string ToCompresstxt)
        {
            //Método para tomar frecuencias de caracteres
            AssignPrefixCodes();
            

        }
         
        string CharacterSubstitution(string Text)
        {
            string bitText = "";
            for (int i = 0; i < Text.Length; i++)
            {
                Characters.TryGetValue(Text[i], out Character character);
                bitText += character.prefixcode;
            }
            int zeros = bitText.Length % 8;
            for (int i = 0; i < zeros; i++)
            {
                bitText += '0';
            }
            Queue<string> Bytes = new Queue<string>();
            while (bitText.Length != 0)
            {
                Bytes.Enqueue(bitText.Substring(0, 8));
                bitText = bitText.Remove(0, 8);
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
