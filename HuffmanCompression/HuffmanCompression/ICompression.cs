using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    public interface ICompression
    {
        public byte[] Compress(char[] ToCompresstxt);
        public char[] Decompress(string CompressedTxt);
    }
}
