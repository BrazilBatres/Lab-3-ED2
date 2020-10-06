using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
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
            using (StreamReader reader = new StreamReader(@"C:\Users\joseg\Desktop\chino.txt"))
            {
                hola = reader.ReadToEnd();
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
