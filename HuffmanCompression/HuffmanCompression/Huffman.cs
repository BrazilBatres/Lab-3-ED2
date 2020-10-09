using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace HuffmanCompression
{
    public class Huffman: ICompression
    {
        Dictionary<byte, Character> Characters = new Dictionary<byte, Character>();
        HeapQueue<BinaryNode> PriorityQueue = new HeapQueue<BinaryNode>();
        BinaryHuffTree huffTree = new BinaryHuffTree();
        double totalCharQuantity;
        int DifferentCharQuantity;
        int FrecuencyBytes;
        public string Name;
        public byte[] Compress(byte[] ToCompresstxt, string FileName)
        {
            Name = FileName;
            totalCharQuantity = ToCompresstxt.Length;
            AssignFrecuency(ToCompresstxt);
            AssignPrefixCodes();
            return BitToCharText(CharToPrefixCodeTxt(ToCompresstxt));
        }
        void AssignFrecuency(byte[] Text)
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
        string CharToPrefixCodeTxt(byte[] Text)
        {
            string prefixCodeText = "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Text.Length; i++)
            {
                Characters.TryGetValue(Text[i], out Character character);
                sb.Append(character.prefixcode);
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
            }
            int length = Bytes.Count + Characters.Count * (1 + FrecuencyBytes)+3+Name.Length;
            byte[] DecimalBytes = new byte[length];
            AddMetaName(DecimalBytes);
            DecimalBytes[Name.Length+1] = (byte)Characters.Count;
            DecimalBytes[Name.Length + 2] = (byte)FrecuencyBytes;
            int k = Name.Length+3;
            foreach (var item in Characters)
            {
                DecimalBytes[k] = (byte)item.Key;
                k ++;
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
            Dictionary<byte, string> PrefixCodes = new Dictionary<byte, string>();
            huffTree.PreOrder(PrefixCodes);
            foreach (var item in Characters)
            {
               PrefixCodes.TryGetValue(item.Key, out string prefixCode);
                item.Value.prefixcode = prefixCode;
                
            }
        }
        public byte[] Decompress(byte[] CompressedTxt)
        {
            FillData(CompressedTxt);
            AssignPrefixCodes();
            int Position = Name.Length + 3 + DifferentCharQuantity * (1 + FrecuencyBytes);
            return PrefixCodeToCharText(CharToBitText(CompressedTxt, Position));
            
        }
        byte[] PrefixCodeToCharText(string Text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Text);
            Queue<byte> result = new Queue<byte>();
            while (totalCharQuantity >= 1)
            {
                int length = 1;
                bool found = false;
                byte accordingChar = 0;
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
                result.Enqueue(accordingChar);
                
            }
            byte[] FinalText = new byte[result.Count];
            for (int i = 0; i < FinalText.Length; i++)
            {
                FinalText[i] = result.Dequeue();
            }
            return FinalText;
        }
        string CharToBitText(byte[] _text, int position)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _text.Length-position; i++)
            {
                sb.Append(ToBinary(_text[position + i]).PadLeft(8, '0'));
            }
             
            return sb.ToString();
        }
        void FillData(byte[] Text)
        {
            int w = 0;
            Name = "";
            while (Text[w] != 10)
            {
                Name += Convert.ToChar(Text[w]).ToString();
                w++;
            }
            DifferentCharQuantity = Text[w+1];
            FrecuencyBytes = Text[w+2];
            Characters.Clear();
            totalCharQuantity = 0;
            int i = w+3;
            while (i < Name.Length + 3 + DifferentCharQuantity * (1 + FrecuencyBytes))
            {
                Character newChar = new Character();
                string binary_number= "";
                for (int h = 1; h <= FrecuencyBytes; h++)
                {
                    binary_number += ToBinary(Text[i + h]).PadLeft(8, '0');
                }

                newChar.frecuency = ToDecimal(binary_number);
                totalCharQuantity += newChar.frecuency;
                binary_number = ToBinary(Text[i]).PadLeft(8, '0');
                Characters.Add(Convert.ToByte(ToDecimal(binary_number)), newChar);
                i += 1 + FrecuencyBytes;
            }  
        }
        
        public void UpdateCompressions(string path, string name, string route, double originalSize, double CompressedSize)
        {
            double compressionFactor, compressionRatio, reductionPercentage;

            compressionRatio = CompressedSize / originalSize;
            compressionFactor = originalSize / CompressedSize;
            reductionPercentage = compressionRatio * 100;

            Compression compression = new Compression();
            compression.OriginalName = name;
            compression.CompressedFilePath = route;
            compression.CompressionRatio = compressionRatio;
            compression.CompressionFactor = compressionFactor;
            compression.ReductionPercentage = reductionPercentage;

            path += "/CompressedFiles.json";

            List<Compression> PreviousFile = new List<Compression>();
            
            
            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    string file;
                    MemoryStream memory = new MemoryStream();
                    fs.CopyTo(memory);
                    file = Encoding.ASCII.GetString(memory.ToArray());
                    PreviousFile = DeserializeCompression(file);
                }
            }
            PreviousFile.Add(compression);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(WriteJson(PreviousFile));
            }

        }

        public string WriteJson(List<Compression> list)
        {
            return JsonSerializer.Serialize(list, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public static List<Compression> DeserializeCompression(string content)
        {
            return JsonSerializer.Deserialize<List<Compression>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public string SerializeCompressions(List<Compression> list)
        {
            return JsonSerializer.Serialize(list, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public void AddMetaName(byte[] ByteArray)
        {
            for (int i = 0; i < Name.Length; i++)
            {
                ByteArray[i] = (byte)Name[i];
            }
            ByteArray[Name.Length] = 10;
        }
    }
}
