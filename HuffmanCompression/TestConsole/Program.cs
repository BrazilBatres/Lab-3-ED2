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
            string file = "Soy un archivo pa codificar";
            using (var Memory = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(@"C:\Users\joseg\Desktop\temp.txt");
                writer.WriteLine("Nombre del Archivo");
                writer.Write(file);
                writer.Close();
            }

            using (var Memory = new MemoryStream())
            {
                StreamReader reader = new StreamReader(@"C:\Users\joseg\Desktop\temp.txt");
                string nombre = reader.ReadLine();
                string arch = reader.ReadToEnd();
            }
        }
    }
}
