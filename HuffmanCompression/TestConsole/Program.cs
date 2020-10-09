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

            //string easy;
            //using (StreamReader reader = new StreamReader(@"C:\Users\brazi\Desktop\prueba.txt"))
            //{
            //    easy = reader.ReadToEnd();
            //}
            //Huffman huffman = new Huffman();
            //byte[] comp = huffman.Compress(easy.ToCharArray());
            //char easy2 = easy[0];
            //byte _easy = (byte)easy2;

            string easy;
            using (StreamReader reader = new StreamReader(@"C:\Users\brazi\Desktop\cuento.txt"))
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
            //char[] Decomp = huffman.Decompress(compressed);
            string r = huffman.Decompress(compressed);
            Console.WriteLine(r == easy);
            using (StreamWriter writer = new StreamWriter(@"C:\Users\brazi\Downloads\Huffi2.txt"))
            {
                writer.Write(r);
            }
            //int entero = 5;
            //byte bait = Convert.ToByte(entero);
            //Console.WriteLine(bait);
            //Console.ReadKey();

            //Console.ReadKey();
            Huffman compressor = new Huffman();

            //Lee el archivo como string
            //using (StreamReader fs = new StreamReader(@"C:\Users\joseg\Desktop\Prueba.txt"))
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append(fs.ReadToEnd());
            //    //ORIGINAL EN STRING
            //    string texto = sb.ToString();
            //    //ORIGNIAL EN BYTE ARRAY
            //    byte[] bytes = Encoding.UTF8.GetBytes(texto);
            //    //ORIGINAL EN CHARS
            //    char[] chars = Encoding.UTF8.GetChars(bytes);
            //    //Resultado guarda los bytes comprimidos
            //    //byte[] resultado = compressor.Compress(Encoding.UTF8.GetChars(bytes));
            //    ////Final guarda los char descomprimidos
            //    //char[] final = compressor.Decompress(resultado);
            //    //string descomprimido = "";
            //    //foreach (var item in final)
            //    //{
            //    //    descomprimido += item.ToString();
            //    //}

            //}

            //Leer como archivo
            using (FileStream fs = File.OpenRead(@"C:\Users\brazi\Desktop\prueba.txt"))
            {
                MemoryStream Memory = new MemoryStream();
                //se guarda en Memory que es un arreglo de bytes
                fs.CopyTo(Memory);
                byte[] baits = Memory.ToArray();
                baits = compressor.Compress(baits);

            }
            
            
            byte[] FinalChars = null;
            using (var Memory = new MemoryStream())
            {
                await file.CopyToAsync(Memory);
                byte[] ByteArray = Memory.ToArray();
                //decompressedText = decompresser.Decompress(ByteArray);
                //FinalChars = new byte[decompressedText.Length];

                //for (int i = 0; i < decompressedText.Length; i++)
                //{
                //    FinalChars[i] = (byte)decompressedText[i];
                //}

            }
            byte[] Content = null;
            Content = FinalChars;
            CustomFile result = new CustomFile();
            result.FileBytes = Content;
            result.contentType = "text/plain";
            result.FileName = OriginalName;
            return File(result.FileBytes, result.contentType, result.FileName + ".txt");





        }
    }
}
