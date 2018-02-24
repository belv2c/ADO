using ADO.DataAccess.Models;
using System;
using System.Collections.Generic;
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
                cmd.CommandText = @"select  i.*
                                from invoice i
	                                join InvoiceLine x 
                                    on x.InvoiceId = i.InvoiceId
                                where exists (select TrackId from Track 
                                    where Name like @FirstLetter + '%' and TrackId = x.TrackId)";

                // sql parameter collection - setting up parameter
                var firstLetterParam = new SqlParameter("@FirstLetter", System.Data.SqlDbType.VarChar);
                // here's the value to use sql server when you see the parameters in a query
                firstLetterParam.Value = firstCharacter;
                // add to list of parameters
                cmd.Parameters.Add(firstLetterParam);


                var reader = cmd.ExecuteReader();

                // create the list
                var invoices = new List<Invoice>();

                while (reader.Read())
                {
                    var invoice = new Invoice
                    {
                        // fill the properties with the values from each row in our database
                        // object initializer
                        InvoiceId = int.Parse(reader["InvoiceId"].ToString()),
                        CustomerId = int.Parse(reader["CustomerId"].ToString()),
                        InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString()),
                        BillingAddress = reader["BillingAddress"].ToString(),
                        BillingCity = reader["BillingCity"].ToString(),
                        BillingState = reader["BillingState"].ToString(),
                        BillingCountry = reader["BillingCountry"].ToString(),
                        BillingPostalCode = reader["BillingPostalCode"].ToString(),
                        Total = double.Parse(reader["Total"].ToString())
                    };

                    invoices.Add(invoice);

                }

                return invoices;
            }
        }
    }
}