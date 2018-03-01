using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // referance - System.Data

namespace TriviaNation
{
    class DataBaseOperations
    {
        private SqlConnection connection;

        public void ConnectToDB()
        {
            try
            {
                var cb = new SqlConnectionStringBuilder
                {
                    DataSource = "trivianation.database.windows.net",
                    UserID = "trivianationadmin",
                    Password = "SoftwareEngineering2",
                    InitialCatalog = "TriviaNation"
                };
                this.connection = new SqlConnection(cb.ConnectionString);
                this.connection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void CreateTable()
        {
            String TSQLSourceCode;

            TSQLSourceCode = "" +
            "DROP TABLE IF EXISTS tableName; " +

            "CREATE TABLE tableName(" +
                "Question    nchar(100)     not null    PRIMARY KEY," +
                " Answer      nchar(100)      not null);";

            SqlCommand command = new SqlCommand(TSQLSourceCode, this.connection);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected + " Rows Affected.");
        }
    }
}
