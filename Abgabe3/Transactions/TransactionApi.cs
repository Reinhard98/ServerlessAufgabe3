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

namespace Transactions {
    public class TransactionApi {

        [FunctionName("Create")]
        public static async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "transaction")] HttpRequest req,
        [Table("Transaction", Connection = "AzureWebJobsStorage")] IAsyncCollector<TransactionTable> todoTableCollector, ILogger log)
        {
            log.LogInformation("Adding new Transaction");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateTransactionDto>(requestBody);

            
            var transaction = new Transaction
            {
                Amount = input.Amount,
                Description = input.Description,
                DebtorIBAN = input.DebtorIBAN,
                CreditorIBAN = input.CreditorIBAN
            };
            
            await todoTableCollector.AddAsync(transaction.ToTable());

            return new OkObjectResult(transaction);
        }


        [FunctionName("GetAll")]
        public static async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "transaction")] HttpRequest req,
        [Table("Transaction", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
        ILogger log)
        {
            log.LogInformation("Getting all Transactions");

            TableQuery<TransactionTable> query = new TableQuery<TransactionTable>();
            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var data = segment.Select(TransactionExtensions.ToCustomer);

            return new OkObjectResult(data);
        }


        

    }
}
