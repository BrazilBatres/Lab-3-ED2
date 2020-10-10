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
            string texto1;
            string texto2;

            using (StreamReader reader = new StreamReader(@"C:\Users\joseg\Desktop\OJALA\cuento2.txt"))
            {
                texto1 = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader(@"C:\Users\joseg\Desktop\OJALA\cuento.txt"))
            {
                texto2 = reader.ReadToEnd();
            }

            Console.WriteLine(texto2 == texto1);
            Console.ReadKey();
        }
    }
}
