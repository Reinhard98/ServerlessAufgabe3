using Microsoft.Azure.Cosmos.Table;
using System;

namespace Transactions {
    public class TransactionTable : TableEntity {

        public DateTime ExecutionDate { get; set; }

        public string Amount { get; set; }

        public string Description { get; set; }

        public string CreditorIBAN { get; set; }

        public string DebtorIBAN { get; set; }

    }
}
