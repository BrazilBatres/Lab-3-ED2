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
            CustomFile result = new CustomFile();
            byte[] ByteArray = null;
            Huffman compression = new Huffman();
            
            
            string path = _env.ContentRootPath;
            string originalName = file.FileName;
            double originalSize;
            
            using (var Memory = new MemoryStream())
            {
                await file.CopyToAsync(Memory);
                ByteArray = Memory.ToArray();
                ByteArray = compression.Compress(ByteArray);
                originalSize = Memory.Length;
            }

            double compressedSize = ByteArray.Length;
            compression.UpdateCompressions(path, originalName, path, originalSize, compressedSize);
            result.FileBytes = ByteArray;
            result.contentType = "text / plain";
            result.FileName = name;
            return File(result.FileBytes, result.contentType, result.FileName + ".huff");
        }

        [Route("decompress")]

        public async Task<ActionResult> Decompress([FromForm] IFormFile file)
        {
            string OriginalName = file.FileName;
            Huffman decompresser = new Huffman();
            byte[] decompressedText;
            using (var Memory = new MemoryStream())
            { 
                await file.CopyToAsync(Memory);
                byte[] ByteArray = Memory.ToArray();
                decompressedText = decompresser.Decompress(ByteArray);
                CustomFile result = new CustomFile();
                result.FileBytes = decompressedText;
                result.contentType = "text/plain";
                result.FileName = OriginalName;
                return File(result.FileBytes, result.contentType, result.FileName);
            }
        }

        [HttpGet]
        public IActionResult ReturnJSON()
        {
                string path = _env.ContentRootPath;
                using (StreamReader reader = new StreamReader(path+ "/CompressedFiles.json"))
                {
                string json = reader.ReadToEnd();
                List<Compression> Compressions = CompressionDeserialize(json);
                JsonSerializer.Serialize(Compressions);
                return Ok(Compressions);
                }  
        }

        public static List<Compression> CompressionDeserialize(string content)
        {
            return JsonSerializer.Deserialize<List<Compression>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
