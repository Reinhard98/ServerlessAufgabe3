using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using Customer;

namespace Report {
    public class ReportApi {

        private static readonly HttpClient client = new HttpClient();

        [FunctionName("CreateReport")]
        public static async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "report/{IBAN}")] HttpRequest req, [FromRoute] String IBAN, ILogger log)
        {
            log.LogInformation("Getting Report");

            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetStreamAsync("http://localhost:7071/api/customer/" + IBAN);

            var requestBody = await new StreamReader(response).ReadToEndAsync();
            var customer = JsonConvert.DeserializeObject<Customer.Customer>(requestBody);


            var responseTransaction = await client.GetStreamAsync("http://localhost:7071/api/transaction/" + IBAN);

            var requestBodyTransaction = await new StreamReader(responseTransaction).ReadToEndAsync();
            var transaction = JsonConvert.DeserializeObject<List<Transactions.Transaction>>(requestBodyTransaction);

            var result = new { CustomerID = customer.Id, CustomerName = customer.Name, CustomerAddress = customer.Address, transaction };

            return new JsonResult(result);
        }


        


    }
}
