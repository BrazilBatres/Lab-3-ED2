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
using System.Text.Json;

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
            string path = _env.ContentRootPath;
            byte[] ByteArray = null;
            CustomFile result = new CustomFile();
            Huffman compression = new Huffman();
            double originalSize;
            using (var Memory = new MemoryStream())
            {
                await file.CopyToAsync(Memory);
                string content = Encoding.ASCII.GetString(Memory.ToArray());
                ByteArray = compression.Compress(content);
                originalSize = Memory.Length;
            }
            double compressedSize = ByteArray.Length;
            compression.UpdateCompressions(path, name, path, originalSize, compressedSize);
            result.FileBytes = ByteArray;
            result.contentType = "image / huff";
            result.FileName = name;

            
            return File(result.FileBytes, result.contentType, result.FileName + ".huff");
        }

        [Route("decompress")]

        public async Task<ActionResult> Decompress([FromForm] IFormFile file)
        {
            string OriginalName = file.FileName;
            Huffman decompresser = new Huffman();
            string decompressedText;
            using (var Memory = new MemoryStream())
            {
                await file.CopyToAsync(Memory);
                string CompressedFile = "";
                byte[] ByteArray = Memory.ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (var item in ByteArray)
                {
                    sb.Append(Convert.ToChar(item));
                }
                CompressedFile = sb.ToString();
                decompressedText = decompresser.Decompress(CompressedFile);
            }
            byte[] Content = null;
            Content = Encoding.ASCII.GetBytes(decompressedText);
            CustomFile result = new CustomFile();
            result.FileBytes = Content;
            result.contentType = "text/plain";
            result.FileName = OriginalName;
            return File(result.FileBytes, result.contentType, result.FileName);
        }

        [HttpGet]
        public IActionResult ReturnJSON()
        {
                List<string> Compressions = new List<string>();
                string path = _env.ContentRootPath;
           
                using (StreamReader reader = new StreamReader(path+ "/CompressedFiles.txt"))
                {
                    string siguiente = "";
                    do
                    {
                        siguiente = reader.ReadLine();
                        if (siguiente != null)
                        {
                            Compressions.Add(siguiente);
                        }

                    } while (siguiente != null);

                    JsonSerializer.Serialize(Compressions);
                    return Ok(Compressions);
                }
           
               
        }
    }
}
