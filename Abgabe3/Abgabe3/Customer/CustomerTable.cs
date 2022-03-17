using Microsoft.Azure.Cosmos.Table;

namespace Customer
{
    public class CustomerTable : TableEntity {

        public string Name { get; set; }

        public string Address { get; set; }

        public string IBAN { get; set; }

    }
}
