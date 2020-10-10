using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCompression
{
    public interface ICompression
    {
        public byte[] Compress(byte[]ToCompresstxt, string FileName);
        public byte[] Decompress(byte[] CompressedTxt);
    }
}
