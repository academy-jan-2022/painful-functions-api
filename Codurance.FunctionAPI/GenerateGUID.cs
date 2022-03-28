using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace Codurance.FunctionAPI
{
    public static class GenerateGUID
    {
        [FunctionName("GenerateGUID")]
        
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Queue("api"),StorageAccount("AzureWebJobsStorage")] ICollector<string> msg,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            NumberRequest data = JsonConvert.DeserializeObject<NumberRequest>(requestBody); 
            
            Guid guid = Guid.NewGuid();

            if (data.number % 4 == 0)
            {
                log.LogInformation($"The number {data.number} is divisible by 4. Woo!");
            }
            
            NumberMessage message = new NumberMessage(data.number, guid);
           
            msg.Add(JsonConvert.SerializeObject(message));

            return new OkObjectResult(guid);
        }

    }
}