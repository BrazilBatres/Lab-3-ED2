using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CustomFile
    {
        public byte[] FileBytes { get; set; }
        public string contentType { get; set; }
        public string FileName { get; set; }
        public string OriginalName { get; set; }
    }
}
