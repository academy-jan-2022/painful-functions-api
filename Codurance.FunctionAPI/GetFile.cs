using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace Codurance.FunctionAPI
{
    
    public static class GetFile
    {
        [FunctionName("GetFile")]
        public static async Task<IActionResult> Run2(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetFile/{id:Guid}")] HttpRequest req,
            Guid id,
            ILogger log){
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("files");

            string fileName = $"{id}" + ".txt";
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            BlobDownloadResult download = await blobClient.DownloadContentAsync();

            return new OkObjectResult(download.Content.ToString());
        }
    }
}