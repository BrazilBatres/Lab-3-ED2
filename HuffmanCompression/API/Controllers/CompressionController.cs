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
            string originalName = file.FileName;
            string resulte = "";
            byte[] ByteArray = null;
            CustomFile result = new CustomFile();
            Huffman compression = new Huffman();
            double originalSize;
            StringBuilder sb = new StringBuilder();
            using (var Memory = new MemoryStream())
            {
                await file.CopyToAsync(Memory);
                string content = Encoding.ASCII.GetString(Memory.ToArray());
                resulte = compression.Compress(content.ToCharArray());
                sb.Append(originalName.ToString());
                sb.Append(resulte);
                originalSize = Memory.Length;
            }
            ByteArray = Encoding.ASCII.GetBytes(sb.ToString());
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
            char[] decompressedText;
            byte[] FinalChars = null;
            using (var Memory = new MemoryStream())
            {
                string OriginalName;
                Huffman decompresser = new Huffman();
                char[] decompressedText;
                byte[] FinalChars = null;
                using (var Memory = new MemoryStream())
                {
                    await file.CopyToAsync(Memory);
                    StreamReader reader = new StreamReader(Memory);
                    string NextLine = "";
                     OriginalName = reader.ReadLine();
                    List<string> PreviousFile = new List<string>();
                    do
                    {
                        NextLine = reader.ReadLine();
                        if (NextLine != null)
                        {
                            PreviousFile.Add(NextLine);
                        }
                    } while (NextLine != null);
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in PreviousFile)
                    {
                        sb.Append(item);
                    }
                    string FinalText = sb.ToString();
                    byte[] ByteArray = Encoding.ASCII.GetBytes(FinalText);
                    decompressedText = decompresser.Decompress(ByteArray);
                    FinalChars = new byte[decompressedText.Length];

                    for (int i = 0; i < decompressedText.Length; i++)
                    {
                        FinalChars[i] = (byte)decompressedText[i];
                    }

                }
                byte[] Content = null;
                Content = FinalChars;
                CustomFile result = new CustomFile();
                result.FileBytes = Content;
                result.contentType = "text/plain";
                result.FileName = OriginalName;
                return File(result.FileBytes, result.contentType, result.FileName + ".txt");
            }
            catch (Exception)
            {
                return null; 
            }
            byte[] Content = null;
            Content = FinalChars;
            CustomFile result = new CustomFile();
            result.FileBytes = Content;
            result.contentType = "text/plain";
            result.FileName = OriginalName;
            return File(result.FileBytes, result.contentType, result.FileName + ".txt");
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
