using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient; // referance - System.Data.dll

namespace TriviaNation
{
    class DataBaseOperations
    {
        SqlConnection connection;

        public void ConnectToDB()
        {
            var connectionString = ConfigurationManager.ConnectionString;
            try
            {
                var cb = new SqlConnectionStringBuilder
                {
                    DataSource = "trivianation.database.windows.net",
                    UserID = "trivianationadmin",
                    Password = "SoftwareEngineering2",
                    InitialCatalog = "TriviaNation"
                };
                using (this.connection = new SqlConnection(cb.ConnectionString))
                {
                    this.connection.Open();
                    CreateTable();
                }
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