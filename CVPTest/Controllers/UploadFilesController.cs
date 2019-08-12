using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CVPTest.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly IConfiguration _config;

        public UploadFilesController(IConfiguration config)
        {
            _config = config;
        }

        #region snippet1
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var connectionString = _config.GetConnectionString("UploadBlob");
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            // コンテナ取得
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("jobs");
            await container.CreateIfNotExistsAsync();
            var blockBlobName = Guid.NewGuid();
            var blockBlob = container.GetBlockBlobReference(blockBlobName.ToString());

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        stream.Position = 0;
                        await blockBlob.UploadFromStreamAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }
        #endregion
    }
}
