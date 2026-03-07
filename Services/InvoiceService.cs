using System;
using System.Collections.Generic;
using System.Linq;
using ManagmentSystem.Models;
using ManagmentSystem.Data;
using ManagmentSystem.Exceptions;
using ManagmentSystem.Extensions;

namespace ManagmentSystem.Services
{
    public class InvoiceService
    {
        private readonly JsonHelper<Invoice> _jsonHelper;
        private List<Invoice> invoices;

        public InvoiceService()
        {
            _jsonHelper = new JsonHelper<Invoice>("invoices.json");
            invoices = _jsonHelper.Load() ;
        }

        public void AddInvoice(Invoice newInvoice)
        {
            newInvoice.Id = invoices.Any()  ? invoices.Max(i => i.Id) + 1  : 1001;

            newInvoice.InvoiceDate = DateTime.Now;

            invoices.Add(newInvoice);
            _jsonHelper.Save(invoices);
        }

        public List<Invoice> GetAllInvoices()
        {
            if (!invoices.Any())
                throw new InvoiceNotFoundException("No invoices available.");

            return invoices;
        }

        public List<Invoice> GetInvoicesByCustomer(string customerName)
        {
            var result = invoices
                .Where(i => i.CustomerName.Equals(
                    customerName,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!result.Any())
                throw new CustomerNotFoundException(customerName);

            return result;
        }
        public List<Invoice> GetMonthlyInvoices(int year, int month)
        {   var result = invoices.Where(i => i.IsInMonth(year, month)).ToList();

            if (!result.Any())
                throw new InvoiceNotFoundException(
     $"No invoices found for {month}/{year}."
 );


            return result;

        }
    }
}
