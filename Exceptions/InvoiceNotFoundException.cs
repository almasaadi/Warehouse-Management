using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Exceptions
{
    public class InvoiceNotFoundException : Exception
    {
        public InvoiceNotFoundException(string message): base(message) { }
    }
}
