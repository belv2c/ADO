using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace ADO.DataAccess
{
    class InvoiceQuery
    {
        public List<Invoice> GetInvoiceByTrackFirstLetter(string firstCharacter)
        {
            using (var connection = new SqlConnection("Server=(local);Database=Chinook;Trusted_Connection=True;"))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = @"select  x.invoiceid,BillingAddress
                                from invoice i
	                                join InvoiceLine x on x.InvoiceId = i.InvoiceId
                                where exists (select TrackId from Track where Name like @FirstLetter + '%' and TrackId = x.TrackId)";

                // sql parameter collection - setting up parameter
                var firstLetterParam = new SqlParameter("@FirstLetter", System.Data.SqlDbType.VarChar);
                // here's the value to use sql server when you see the parameters in a query
                firstLetterParam.Value = firstCharacter;
                // add to list of parameters
                cmd.Parameters.Add(firstLetterParam);


                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var invoiceId = reader.GetInt32(0);
                    var billingAddress = reader["BillingAddress"].ToString();

                    Console.WriteLine($"Invoice {invoiceId} is going to {billingAddress}");
                }
            }
        }
    }

    // create in-code representation of database table
    // can interact more naturally with the data in our code
    internal class Invoice
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public string BillingPostalCode { get; set; }
        public double Total { get; set; }
    }
}