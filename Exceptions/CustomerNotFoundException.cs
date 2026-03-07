using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string customerName)
            : base($"No invoices were found for the customer: {customerName}") { }
    }
}
