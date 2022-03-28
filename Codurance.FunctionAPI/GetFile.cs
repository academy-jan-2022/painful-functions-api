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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetFile/{id:Guid}")] HttpRequest req,
            Guid id,
            ILogger log){
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            BlobDownloadResult download = await new BlobServiceClient(connectionString)
            .GetBlobContainerClient("files")
            .GetBlobClient($"{id}.txt")
            .DownloadContentAsync();


            int number = Int32.Parse(download.Content.ToString());

            var type = number % 2 == 0 ? "even" : "odd";

            log.LogInformation($"File with id: {id} contains an {type} number");

            return new OkObjectResult(download.Content.ToString());
        }
    }
}
