using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions
{
    public class Transaction {


        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime ExecutionDate { get; set; } = DateTime.Now;

        public string Amount { get; set; }

        public string Description { get; set; }

        public string CreditorIBAN { get; set; }

        public string DebtorIBAN { get; set; }
    }
}
