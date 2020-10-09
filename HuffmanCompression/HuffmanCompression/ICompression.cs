using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    public interface ICompression
    {
        public byte[] Compress(/*char*/byte[] /*string*/ ToCompresstxt, string FileName);
        public /*char[]*//*Queue<byte>*/ byte[] Decompress(/*string*/byte[] CompressedTxt);
    }
}
