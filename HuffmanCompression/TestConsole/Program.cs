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
            //Console.WriteLine("Escriba texto a comprimir");
            //string text = Console.ReadLine();
            Huffman huffman = new Huffman();
            //using (StreamWriter writer = new StreamWriter(@"C:\Users\brazi\Desktop\huff.txt"))
            //{
            //    byte[] towrite = huffman.Compress(text);
            //    for (int i = 0; i < towrite.Length; i++)
            //    {
            //        writer.Write(Convert.ToChar(towrite[i]).ToString());
            //    }
            //}
            //string text2 = "";
            //using (StreamReader reader = new StreamReader(@"C:\Users\brazi\Desktop\huff.txt"))
            //{
            //    text2 = reader.ReadToEnd();
            //}
            //string text3 = huffman.Decompress(text2);
            //Console.WriteLine(text3);
            //Console.WriteLine(text == text3);
            //Console.ReadKey();

            //List<Letter> Tabla = new List<Letter>();
            //string str = "HHHAAAABBBEE";
            //bool encontrado = false;
            //for (int i = 0; i < str.Length; i++)
            //{
            //    encontrado = false;
            //    if (Tabla.Count != 0)
            //    {
            //        foreach (var item in Tabla)
            //        {
            //            if (item.Key == str[i].ToString())
            //            {
            //                item.Frecuency += 1;
            //                encontrado = true;
            //            }
            //        }
            //        if (!encontrado)
            //        {
            //            Letter letter = new Letter();
            //            letter.Key = str[i].ToString();
            //            letter.Frecuency = 1;
            //            Tabla.Add(letter);
            //        }
            //    }
            //    else
            //    {
            //        Letter letter = new Letter();
            //        letter.Key = str[i].ToString();
            //        letter.Frecuency = 1;
            //        Tabla.Add(letter);
            //    }
            //}
            //int entero = 3;
            //char caracter = '3';
            //double doble = 3.5;
            //Console.WriteLine(Convert.ToByte(doble));
            //Console.WriteLine(Convert.ToByte((char)caracter));
            //Console.ReadKey();
            string hola;
            // Descarga el cuento.txt de slack y comprímelo y mándalo acá
            using (StreamReader reader = new StreamReader(@"C:\Users\joseg\Desktop\Hola.huff"))
            {
                hola = reader.ReadToEnd();
                string hey = huffman.Decompress(hola);
            }
            byte[] array = Encoding.ASCII.GetBytes(hola);
            string stringg = Encoding.ASCII.GetString(array);

            Console.WriteLine("Tamaño en disco en bytes: ");
            Console.WriteLine(array.Length);
            Console.WriteLine("Tamaño de texto: ");
            Console.WriteLine(hola.Length);           
            




        }


       
    }
}
