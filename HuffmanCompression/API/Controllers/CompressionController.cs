using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using API.Models;
using HuffmanCompression;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class CompressionController : ControllerBase
    {
        private IWebHostEnvironment _env;
        public CompressionController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("compress/{name}")]

        public async Task<ActionResult> Compress(string name, [FromForm] IFormFile file)
        {
            byte[] ByteArray = null;
            CustomFile result = new CustomFile();
            Huffman compression = new Huffman();
            using (var Memory = new MemoryStream())
            {
                await file.CopyToAsync(Memory);
                string content = Encoding.ASCII.GetString(Memory.ToArray());
                ByteArray = compression.Compress(content);
            }

            result.FileBytes = ByteArray;
            result.contentType = "Compressed File / huff";
            result.FileName = name;

            return File(result.FileBytes, result.contentType, result.FileName + ".huff");
        }

        [Route("decompress")]

        public CustomFile Decompress()
        {

        }

        [HttpGet]
        
        public IActionResult ReturnJSON()
        {
            string path = _env.ContentRootPath;
            using (StreamReader = )
            {

            }
        }
    }
}
