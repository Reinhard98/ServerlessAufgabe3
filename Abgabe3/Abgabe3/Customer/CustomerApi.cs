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

namespace Customer {
    public class ReportApi {

        [FunctionName("CreateCustomer")]
        public static async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "customer")] HttpRequest req,
        [Table("Customer", Connection = "AzureWebJobsStorage")] IAsyncCollector<CustomerTable> CustomerTableCollector, ILogger log)
        {
            log.LogInformation("Adding new Customer");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateCustomerDto>(requestBody);

            if (input.IBAN == null) {
                return new BadRequestObjectResult("Please provide IBAN");
            }

            var customer = new Customer
            {
                Name = input.Name,
                Address = input.Address,
                IBAN = input.IBAN
            };
            
            await CustomerTableCollector.AddAsync(customer.ToTable());

            return new OkObjectResult(customer);
        }


        [FunctionName("GetAllCustomer")]
        public static async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "customer")] HttpRequest req,
        [Table("Customer", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
        ILogger log)
        {
            log.LogInformation("Getting all Customers");

            TableQuery<CustomerTable> query = new TableQuery<CustomerTable>();
            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var data = segment.Select(CustomerExtensions.ToCustomer);

            return new OkObjectResult(data);
        }

        [FunctionName("GetCustomer")]
        public static async Task<IActionResult> GetCustomer(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "customer/{IBAN}")] HttpRequest req, [FromRoute] String IBAN,
        [Table("Customer", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
        ILogger log) {
            log.LogInformation("Getting Customer with IBAN: " + IBAN);

            TableQuery<CustomerTable> query = new TableQuery<CustomerTable>();
            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var data = segment.Select(CustomerExtensions.ToCustomer).Where(ct => ct.IBAN == IBAN).FirstOrDefault();

            return new OkObjectResult(data);
        }


    }
}
