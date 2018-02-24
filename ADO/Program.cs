using System;
using System.Data.SqlClient;

namespace ADOExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstLetter = Console.ReadLine();

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
                firstLetterParam.Value = firstLetter;
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

            Console.ReadLine();

        }
    }
}