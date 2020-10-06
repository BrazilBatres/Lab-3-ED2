using System;
using System.Collections.Generic;
using System.IO;
using HuffmanCompression;
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dictionary<char, int> prueba = new Dictionary<char, int>();
            //Letter hola = new Letter();
            //hola.Frecuency = 1;
            //prueba.Add('h', 1);
            //prueba.TryGetValue('h', out int ans);
            //ans= ans +1;
            //prueba.TryGetValue('h', out int ans1);
            //int entero = 3;
            //Console.WriteLine(ans1);
            //Console.WriteLine(Convert.ToInt32('1'.ToString()));
            Console.WriteLine("Escriba texto a comprimir");
            string text = Console.ReadLine();
            Huffman huffman = new Huffman();
            using (StreamWriter writer = new StreamWriter(@"C:\Users\brazi\Desktop\huff.txt"))
            {
                byte[] towrite = huffman.Compress(text);
                for (int i = 0; i < towrite.Length; i++)
                {
                    writer.Write(Convert.ToChar(towrite[i]).ToString());
                }
            }
            string text2 = "";
            using (StreamReader reader = new StreamReader(@"C:\Users\brazi\Desktop\huff.txt"))
            {
                text2 = reader.ReadToEnd();
            }
            string text3 = huffman.Decompress(text2);
            Console.WriteLine(text3);
            Console.WriteLine(text == text3);
            Console.ReadKey();

        }
    }
}
