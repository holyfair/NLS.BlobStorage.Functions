using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NLS.BlobStorage.Functions.Consts;

namespace NLS.BlobStorage.Functions.Ordetrs
{
    public static class OrderAcknowledgementFunction
    {
        [FunctionName("Acknowledge")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [BlobTrigger("inbound/orders", Connection = "AzureWebJobsStorage")]
        Stream myBlob,
        string name,
        ILogger log)
        {
            log.LogInformation($"Blob trigger function processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            // Add your custom logic here to process the blob and generate the XML file

            // Generate the XML content
            string xmlContent = GenerateXmlContent(name);

            // Set the response content type to XML
            var contentResult = new ContentResult
            {
                Content = xmlContent,
                ContentType = "application/xml",
                StatusCode = 200
            };

            return contentResult;
        }

        private static string GenerateXmlContent(string fileName)
        {
            string templateFilePath = "Acknowledgement.xml";

            string templateContent = File.ReadAllText(templateFilePath);
            string filledXml = string.Format(templateContent, ResultCodes.Acknowledgment, FileTypes.PurchaseOrder, fileName);

            return filledXml;
        }
    }
}
