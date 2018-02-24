using System;
using ADO.DataAccess;

namespace ADOExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // takes user input
            var firstLetter = Console.ReadLine();

            // querying database
            var invoiceQuery = new InvoiceQuery();
            var invoices = invoiceQuery.GetInvoiceByTrackFirstLetter(firstLetter);

            // prints output to console
            foreach (var invoice in invoices)
            {
                Console.WriteLine($"Invoice Id {invoice.InvoiceId} was shipped to {invoice.BillingAddress}.");
            }

            var invoiceModifier = new InvoiceModifier();
            invoiceModifier.Delete(9);
            
            Console.ReadLine();
        }
    }
}