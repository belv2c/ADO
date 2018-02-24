using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ADO.DataAccess
{
    class InvoiceModifier
    {
        readonly string _connectionString = ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString;

        // delete invoice
        public bool Delete(int invoiceId)
        {
            //delete the line items for the invoiceId
            if (!DeleteLineItems(invoiceId))
            {
                return false;
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();

                // setting up a parameter that we need to define next
                cmd.CommandText = @"Delete
                                    From Invoice
                                    Where InvoiceId = @InvoiceId";

                // parameter-ize the query
                var invoiceIdParam = new SqlParameter("@InvoiceId", System.Data.SqlDbType.Int);
                invoiceIdParam.Value = invoiceId;
                cmd.Parameters.Add(invoiceIdParam);

                //open the connection
                connection.Open();

                // takes in an int number of rows effected
                var result = cmd.ExecuteNonQuery();

                // sets the result
                // return whether or not the result is one or not
                return result == 1;
            }
        }


        // delete line items
        bool DeleteLineItems(int invoiceId)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"Delete
                                    From InvoiceLine
                                    Where InvoiceId = @InvoiceId";

                var invoiceIdParam = new SqlParameter("@InvoiceId", System.Data.SqlDbType.Int);
                invoiceIdParam.Value = invoiceId;
                cmd.Parameters.Add(invoiceIdParam);

                connection.Open();

                var result = cmd.ExecuteNonQuery();

                return result > 1;

            }
        }
    }
}
