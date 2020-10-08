using System;
using System.Collections.Generic;
using System.IO;

using HuffmanCompression;

using System.Text;
using System.Threading.Tasks;


namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string easy;
            using (StreamReader reader = new StreamReader(@"C:\Users\brazi\Downloads\easy test.txt"))
            {
                easy = reader.ReadToEnd();
            }
            Huffman huffman = new Huffman();
            byte[] comp = huffman.Compress(easy);
           
            using (StreamWriter writer = new StreamWriter(@"C:\Users\brazi\Downloads\Huffi.txt"))
            {
                for (int i = 0; i < comp.Length; i++)
                {
                    writer.Write((Convert.ToChar(comp[i])).ToString());
                }
            }
            string compressed;
            using (StreamReader reader = new StreamReader(@"C:\Users\brazi\Downloads\Huffi.txt"))
            {
               compressed = reader.ReadToEnd();
            }
            string Decomp = huffman.Decompress(compressed);

            Console.WriteLine(Decomp == easy);

            Console.ReadKey();
        }
    }
}
