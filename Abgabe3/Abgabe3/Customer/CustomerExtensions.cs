using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer {
    public static class CustomerExtensions {
        
        public static CustomerTable ToTable(this Customer customer) {
            return new CustomerTable
            {
                PartitionKey = "Customer",
                RowKey = customer.Id,
                Address = customer.Address,
                Name = customer.Name,
                IBAN = customer.IBAN
            };
        }

        public static Customer ToCustomer(this CustomerTable customerTable)
        {
            return new Customer {
                Id = customerTable.RowKey,
                Address = customerTable.Address,
                Name = customerTable.Name,
                IBAN = customerTable.IBAN
            };
        }
    }
}
