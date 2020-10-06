using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
         
        byte[] CharacterSubstitution(string Text)
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
            byte[] DecimalBytes = new byte[Bytes.Count];
            int count = Bytes.Count;
            for (int i = 0; i < count; i++)
            {
                DecimalBytes[i] = ToDecimal(Bytes.Dequeue());
            }
        }
        byte ToDecimal(string eightbits)
        {
            byte ToReturn = 0;
            for (int i = 0; i < 8; i++)
            {
                
                ToReturn += Convert.ToByte(eightbits[7 - i] * Math.Pow(2, i));
            }
            return ToReturn;
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

        public void UpdateCompressions(string path, string name, string route, double originalSize, double CompressedSize)
        {
            double compressionFactor, compressionRatio, reductionPercentage;

            compressionRatio = CompressedSize / originalSize;
            compressionFactor = originalSize / CompressedSize;
            reductionPercentage = compressionRatio * 100;

            List<string> PreviousFile = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string NextLine;
                do
                {
                    NextLine = reader.ReadLine();
                    PreviousFile.Add(NextLine);
                } while (NextLine != null);

            }
            int LineCount = PreviousFile.Count;
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < LineCount; i++)
                {
                    writer.WriteLine(PreviousFile.First());
                    PreviousFile.Remove(PreviousFile.First());
                }
                writer.WriteLine("{" + "\"originalName\" : \"" + name + "\", \"compressedFilePath\" : \"" + route + "\", \"compressionRatio\" : " + compressionRatio.ToString() + ", \"compressionFactor\" : " + compressionFactor.ToString() + " , \"reductionPercentage\" : " + reductionPercentage.ToString() + "}");
            }
        }
        
    }
}
