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
        //Dictionary<char, Character> Characters2 = new Dictionary<char, Character>();
        HeapQueue<BinaryNode> PriorityQueue = new HeapQueue<BinaryNode>();
        BinaryHuffTree huffTree = new BinaryHuffTree();
        double totalCharQuantity;
        int DifferentCharQuantity;
        int FrecuencyBytes;
        
        public byte[] Compress(string ToCompresstxt)
        {
            totalCharQuantity = ToCompresstxt.Length;
            
            AssignFrecuency(ToCompresstxt);
            bool check = CheckQuantity();
            AssignPrefixCodes();
            return BitToCharText(CharToPrefixCodeTxt(ToCompresstxt));

        }
        
        bool CheckQuantity()
        {
            int suma = 0;
            foreach (var item in Characters)
            {
                suma += item.Value.frecuency;
            }
            return suma == 208926;
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
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Text.Length; i++)
            {
                Characters.TryGetValue(Text[i], out Character character);
                sb.Append(character.prefixcode);
                //prefixCodeText += character.prefixcode;
            }
            prefixCodeText = sb.ToString();
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
            StringBuilder sb = new StringBuilder();
            sb.Append(bitText);
            while (sb.Length != 0)
            {
                Bytes.Enqueue(sb.ToString(0,8));
                sb.Remove(0, 8);
                //bitText = bitText.Remove(0, 8);
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
                DecimalBytes[k+j] = Convert.ToByte(ToDecimal(Bytes.Dequeue()));
            }
            return DecimalBytes;
        }
        int ToDecimal(string _binaryNumber)
        {
            int ToReturn = 0;
            for (int i = 0; i < _binaryNumber.Length; i++)
            {
                double aux = Convert.ToInt32(_binaryNumber[_binaryNumber.Length - 1 - i].ToString()) * Math.Pow(2, i);
                ToReturn += Convert.ToInt32(Convert.ToInt32(_binaryNumber[_binaryNumber.Length - 1 - i].ToString()) * Math.Pow(2, i));
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
                ByteArray[position + i] = Convert.ToByte(ToDecimal(binNumber.Substring(0, 8)));
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
        //void AssignPrefixCodes2()
        //{
        //    foreach (var item in Characters2)
        //    {
        //        BinaryNode actual = new BinaryNode();
        //        actual.character = item.Key;
        //        actual.Priority = item.Value.frecuency / totalCharQuantity;
        //        PriorityQueue.Add(actual);
        //    }
        //    bool exit = false;
        //    BinaryNode Auxnode = new BinaryNode();
        //    while (!exit)
        //    {
        //        //hay la posibilidad de que haya solo un caracter?
        //        if (!PriorityQueue.IsEmpty())
        //        {
        //            Auxnode = PriorityQueue.Remove();
        //        }
        //        if (!PriorityQueue.IsEmpty())
        //        {
        //            huffTree.Insertion(Auxnode, PriorityQueue.Remove());

        //        }
        //        if (!PriorityQueue.IsEmpty())
        //        {

        //            PriorityQueue.Add(huffTree.GetRoot());
        //        }
        //        else
        //        {
        //            exit = true;
        //        }
        //    }
        //    Dictionary<char, string> PrefixCodes = new Dictionary<char, string>();
        //    huffTree.PreOrder(PrefixCodes);
        //    foreach (var item in Characters2)
        //    {
        //        PrefixCodes.TryGetValue(item.Key, out string prefixCode);
        //        item.Value.prefixcode = prefixCode;

        //    }
        //}
        public string Decompress(string CompressedTxt)
        {
            FillData(CompressedTxt);
            int largo = CompressedTxt.Length;
            CompressedTxt = CompressedTxt.Remove(0,2 + DifferentCharQuantity * (1 + FrecuencyBytes));
            AssignPrefixCodes();
            //AssignPrefixCodes2();
            return PrefixCodeToCharText(CharToBitText(CompressedTxt));
            
        }
        string PrefixCodeToCharText(string Text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Text);
            StringBuilder builder = new StringBuilder();
            
            while (totalCharQuantity >= 1)
            {
                int length = 1;
                bool found = false;
                char accordingChar = ' ';
                while (!found)
                {
                    string prefix_code = sb.ToString(0, length);
                    foreach (var item in Characters)
                    {
                        if (item.Value.prefixcode == prefix_code)
                        {
                            found = true;
                            accordingChar = item.Key;
                            sb.Remove(0, length);
                            break;
                        }
                    }
                    if (!found) length++;
                }
                totalCharQuantity--;
                builder.Append(accordingChar.ToString());
            }

            return builder.ToString();
        }
        string CharToBitText(string _text)
        {
            string bitText = "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _text.Length; i++)
            {
                sb.Append(ToBinary(_text[i]).PadLeft(8, '0'));
                //bitText += ToBinary(_text[i]).PadLeft(8, '0');
            }
            bitText = sb.ToString();
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
                    binary_number += ToBinary(Text[i + h]).PadLeft(8, '0');
                }

                newChar.frecuency = ToDecimal(binary_number);
                totalCharQuantity += newChar.frecuency;
                Characters.Add(Text[i], newChar);
                i += 1 + FrecuencyBytes;
            }
            //bool hola = CheckQuantity();
            //string e = ErrorChar();
        }
        //string ErrorChar()
        //{
        //    string ret = "todos iguales";
        //    foreach (var item in Characters)
        //    {
        //        Characters2.TryGetValue(item.Key, out Character val);
        //        if (item.Value.frecuency != val.frecuency)
        //        {
        //            ret = item.Key.ToString();
        //        }
        //    }
        //    return ret;
        //}

        public void UpdateCompressions(string path, string name, string route, double originalSize, double CompressedSize)
        {
            double compressionFactor, compressionRatio, reductionPercentage;

            compressionRatio = CompressedSize / originalSize;
            compressionFactor = originalSize / CompressedSize;
            reductionPercentage = compressionRatio * 100;
            path += "/CompressedFiles.txt"; 
            List<string> PreviousFile = new List<string>();
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string NextLine;
                    do
                    {
                        NextLine = reader.ReadLine();
                        PreviousFile.Add(NextLine);
                    } while (NextLine != null);

                }
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
