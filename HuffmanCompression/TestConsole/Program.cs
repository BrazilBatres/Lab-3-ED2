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
            string hola = "Pablo Daniel Cuevas Paz";
            byte[] Array = Encoding.ASCII.GetBytes(hola);
            using (var Memory = new MemoryStream(Array))
            {
                int bufferSize = 10;
                BinaryReader reader = new BinaryReader(Memory);
                byte[] res = reader.ReadBytes(bufferSize);
            }
        }
    }
}
