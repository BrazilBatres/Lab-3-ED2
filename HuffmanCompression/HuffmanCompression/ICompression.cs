using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    public interface ICompression
    {
        public byte[] Compress(string ToCompresstxt);
        public string Decompress(string CompressedTxt);
    }
}
