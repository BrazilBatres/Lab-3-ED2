using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HuffmanCompression
{
    public interface ICompression
    {
        public byte[] Compress(string path, string FileName, int bSize);
        public byte[] Decompress(byte[] CompressedTxt);
    }
}
