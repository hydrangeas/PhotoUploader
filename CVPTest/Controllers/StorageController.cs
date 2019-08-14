using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CVPTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CVPTest.Controllers
{
    public class StorageController : Controller
    {
        private readonly IConfiguration config;
        private readonly JobDatabase jobDatabase;

        public StorageController(IConfiguration _config, JobDatabase _jobDatabase)
        {
            config = _config;
            jobDatabase = _jobDatabase;
        }

        #region snippet1
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var connectionString = config.GetConnectionString("UploadBlob");
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
                        try
                        {
                            // TODO: streamを経由せずにBlobへコピーする方法はないか確認
                            // (メインメモリが少ないのでボトルネックになるかも）
                            await file.CopyToAsync(stream);
                            // ファイル=>stream コピー時にPositionが末尾へ移動しているので
                            // 0に戻してやる（そうしないとアップロード後のサイズが0になる）
                            stream.Position = 0;

                            // stream=>blob アップロード
                            await blockBlob.UploadFromStreamAsync(stream);

                            // Job情報をデータベースに保存する
                            await AddAsync(new Job
                            {
                                LogicalName = file.FileName,
                                PhysicalName = blockBlobName.ToString(),
                                PhysicalPath = blockBlob.Uri.AbsoluteUri
                            });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                            // TODO: エラーについて対策する
                            // メモリ確保エラー
                            // ネットワークエラー
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// アップロードしたファイルの情報をテーブルに追加する
        /// </summary>
        /// <param name="job">Jobテーブルに追加するJob情報</param>
        public async Task AddAsync(Job job)
        {
            if (job == null)
                return;

            jobDatabase.Jobs.Add(job);
            try
            {
                await jobDatabase.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
                //TODO: エラー復帰方法を考える
            }
        }
        #endregion
    }
}
