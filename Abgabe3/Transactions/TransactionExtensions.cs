using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions {
    public static class TransactionExtensions {
        
        public static TransactionTable ToTable(this Transaction transaction) {
            return new TransactionTable
            {
                PartitionKey = "Customer",
                RowKey = transaction.Id,
                Description = transaction.Description,
                CreditorIBAN = transaction.CreditorIBAN,
                DebtorIBAN = transaction.DebtorIBAN,
                ExecutionDate = transaction.ExecutionDate,
                Amount = transaction.Amount
            };
        }

        public static Transaction ToCustomer(this TransactionTable transactionTable)
        {
            return new Transaction {
                Id = transactionTable.RowKey,
                Amount = transactionTable.Amount,
                Description = transactionTable.Description,
                CreditorIBAN = transactionTable.CreditorIBAN,
                DebtorIBAN= transactionTable.DebtorIBAN
            };
        }
    }
}
