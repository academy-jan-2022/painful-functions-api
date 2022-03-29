using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
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

    private static readonly Lazy<TelemetryClient> TelemetryClientLazy =
        new(() =>
        {
            var config = TelemetryConfiguration.CreateDefault();
            config.InstrumentationKey =
                Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
            return new TelemetryClient(config);
        });

    private static TelemetryClient TelemetryClient => TelemetryClientLazy.Value;

    [FunctionName("GenerateGUID")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req,
        [Queue("api")] [StorageAccount("AzureWebJobsStorage")]
        ICollector<string> msg,
        ILogger log)
    {
        var correlationID = Guid.NewGuid();
        TelemetryClient.TrackTrace(
            "C# HTTP trigger function processed a request",
            new Dictionary<string, string>
            {
                { "correlationID", correlationID.ToString() }
            }
        );

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<NumberRequest>(requestBody);

        var guid = Guid.NewGuid();


        if (data.number % 4 == 0)
        {
            TelemetryClient.TrackTrace(
                string.Format(DivisibleByFourTemplate, data.number),
                new Dictionary<string, string>
                {
                    { "divisibleBy", "4" },
                    { "correlationID", correlationID.ToString() }
                }
            );
        }

        var message = new NumberMessage(data.number, guid, correlationID);

        msg.Add(JsonConvert.SerializeObject(message));

        return new OkObjectResult(guid);
    }
}
