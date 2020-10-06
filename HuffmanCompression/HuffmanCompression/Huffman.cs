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
        int DifferentCharQuantity;
        int FrecuencyBytes;
        
        public byte[] Compress(string ToCompresstxt)
        {
            totalCharQuantity = ToCompresstxt.Length;
            AssignFrecuency(ToCompresstxt);
            AssignPrefixCodes();
            return BitToCharText(CharToPrefixCodeTxt(ToCompresstxt));

        }
        void AssignFrecuency(string Text)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                if (Characters.ContainsKey(Text[i]))
                {
                    Characters.TryGetValue(Text[i], out Character character);
                    character.frecuency++;
                }
                else
                {
                    Character newChar = new Character();
                    newChar.frecuency = 1;
                    Characters.Add(Text[i], newChar);
                }
            }
            foreach (var item in Characters)
            {
                while (item.Value.frecuency > Math.Pow(2, FrecuencyBytes * 8) - 1)
                {
                    FrecuencyBytes++;
                }
            }
            DifferentCharQuantity= Characters.Count;
        }
        string CharToPrefixCodeTxt(string Text)
        {
            string prefixCodeText = "";
            for (int i = 0; i < Text.Length; i++)
            {
                Characters.TryGetValue(Text[i], out Character character);
                prefixCodeText += character.prefixcode;
            }
            int zeros = 8-(prefixCodeText.Length % 8);
            for (int i = 0; i < zeros; i++)
            {
                prefixCodeText += '0';
            }
            return prefixCodeText;
        }

        byte[] BitToCharText(string bitText)
        {
            
            Queue<string> Bytes = new Queue<string>();
            while (bitText.Length != 0)
            {
                Bytes.Enqueue(bitText.Substring(0, 8));
                bitText = bitText.Remove(0, 8);
            }
            int length = Bytes.Count + Characters.Count * (1 + FrecuencyBytes)+2;
            byte[] DecimalBytes = new byte[length];
            DecimalBytes[0] = (byte)Characters.Count;
            DecimalBytes[1] = (byte)FrecuencyBytes;
            int k = 2;
            foreach (var item in Characters)
            {
                DecimalBytes[k] = (byte)item.Key;
                k++;
                ToByte(DecimalBytes, k, item.Value.frecuency);
                k += FrecuencyBytes;
            }
            int count = Bytes.Count;
            for (int j = 0; j < count; j++)
            {
                DecimalBytes[k+j] = ToDecimal(Bytes.Dequeue());
            }
            return DecimalBytes;
        }
        byte ToDecimal(string _binaryNumber)
        {
            byte ToReturn = 0;
            for (int i = 0; i < _binaryNumber.Length; i++)
            {
                double aux = Convert.ToInt32(_binaryNumber[_binaryNumber.Length - 1 - i].ToString()) * Math.Pow(2, i);
                ToReturn += Convert.ToByte(Convert.ToInt32(_binaryNumber[_binaryNumber.Length - 1 - i].ToString()) * Math.Pow(2, i));
            }
            return ToReturn;
        }
        string ToBinary(int DecimalNumber)
        {
            string BinaryNumber = "";
            while (DecimalNumber > 1)
            {
                BinaryNumber = (DecimalNumber % 2) + BinaryNumber;
                DecimalNumber /= 2;
            }
            BinaryNumber = DecimalNumber + BinaryNumber;
            
            return BinaryNumber;
        }
        void ToByte(byte[] ByteArray, int position, int frecuency)
        {
            string binNumber = ToBinary(frecuency).PadLeft(FrecuencyBytes * 8, '0');

            for (int i = 0; i < FrecuencyBytes; i++)
            {
                ByteArray[position + i] = ToDecimal(binNumber.Substring(0, 8));
                binNumber = binNumber.Remove(0, 8);
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
               PrefixCodes.TryGetValue(item.Key, out string prefixCode);
                item.Value.prefixcode = prefixCode;
                
            }
        }

        public string Decompress(string CompressedTxt)
        {
            FillData(CompressedTxt);
            CompressedTxt = CompressedTxt.Remove(0,2 + DifferentCharQuantity * (1 + FrecuencyBytes));
            AssignPrefixCodes();
            return PrefixCodeToCharText(CharToBitText(CompressedTxt));
            
        }
        string PrefixCodeToCharText(string Text)
        {
            int length = 1;
            bool found = false;
            char accordingChar = ' ';
            while (!found)
            {
                string prefix_code = Text.Substring(0, length);
                foreach (var item in Characters)
                {
                    if (item.Value.prefixcode == prefix_code)
                    {
                        found = true;
                        accordingChar = item.Key;
                        break;
                    }
                }
                if(!found) length++;
            }
            Text = Text.Remove(0, length);
            if (totalCharQuantity >1)
            {
                totalCharQuantity--;
                return accordingChar.ToString()+PrefixCodeToCharText(Text);
            }
            else
            {
                return accordingChar.ToString();
            }
        }
        string CharToBitText(string _text)
        {
            string bitText = "";
            for (int i = 0; i < _text.Length; i++)
            {
                bitText += ToBinary(_text[i]).PadLeft(8, '0');
            }
            return bitText;
        }
        void FillData(string Text)
        {
           DifferentCharQuantity = Text[0];
            FrecuencyBytes = Text[1];
            Characters.Clear();
            totalCharQuantity = 0;
            int i = 2;
            while (i < 2+ DifferentCharQuantity * (1 + FrecuencyBytes))
            {
                Character newChar = new Character();
                string binary_number= "";
                for (int h = 1; h <= FrecuencyBytes; h++)
                {
                    binary_number += ToBinary(Text[i + h]);
                }

                newChar.frecuency = ToDecimal(binary_number);
                totalCharQuantity += newChar.frecuency;
                Characters.Add(Text[i], newChar);
                i += 1 + FrecuencyBytes;
            }


        }

        
    }
}
