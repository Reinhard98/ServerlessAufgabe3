using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer
{
    public class Customer {


        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Address { get; set; }

        public string IBAN { get; set; }
    }
}
