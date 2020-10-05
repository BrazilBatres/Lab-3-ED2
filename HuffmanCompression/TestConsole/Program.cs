using System;
using System.Collections.Generic;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Letter> Tabla = new List<Letter>();
            string str = "HHHAAAABBBEE";
            bool encontrado = false;
            for (int i = 0; i < str.Length; i++)
            {
                encontrado = false;
                if (Tabla.Count != 0)
                {
                    foreach (var item in Tabla)
                    {
                        if (item.Key == str[i].ToString())
                        {
                            item.Frecuency += 1;
                            encontrado = true;
                        }
                    }
                    if (!encontrado)
                    {
                        Letter letter = new Letter();
                        letter.Key = str[i].ToString();
                        letter.Frecuency = 1;
                        Tabla.Add(letter);
                    }
                }
                else
                {
                    Letter letter = new Letter();
                    letter.Key = str[i].ToString();
                    letter.Frecuency = 1;
                    Tabla.Add(letter);
                }
            }
        }
    }
}
