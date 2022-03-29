using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Codurance.FunctionAPI;

public static class GenerateGUID
{
    private const string DivisibleByFourTemplate = "The number {0} is divisible by 4. Woo!";

    [FunctionName("GenerateGUID")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req,
        [Queue("api")] [StorageAccount("AzureWebJobsStorage")]
        ICollector<string> msg,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<NumberRequest>(requestBody);

        var guid = Guid.NewGuid();

        if (data.number % 4 == 0)
            log.LogInformation(DivisibleByFourTemplate, data.number);

        var message = new NumberMessage(data.number, guid);

        msg.Add(JsonConvert.SerializeObject(message));

        return new OkObjectResult(guid);
    }
}
