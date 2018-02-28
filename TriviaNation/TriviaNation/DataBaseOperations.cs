using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // referance - System.Data.dll

namespace TriviaNation
{
    class DataBaseOperations
    {
        //SqlConnection connection = null;

        public void ConnectToDB()
        {
            try
            {
                var cb = new SqlConnectionStringBuilder();
                cb.DataSource = "trivianation.database.windows.net";
                cb.UserID = "trivianationadmin";
                cb.Password = "SoftwareEngineering2";
                cb.InitialCatalog = "TriviaNation";
                SqlConnection connection = new SqlConnection(cb.ConnectionString);
                connection.Open();
                CreateTable(connection);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void CreateTable(SqlConnection connection)
        {
            String TSQLSourceCode;

            TSQLSourceCode = "" +
            "DROP TABLE IF EXISTS tableName; " +

            "CREATE TABLE tableName(" +
                "Question    nchar(100)     not null    PRIMARY KEY," +
                " Answer      nchar(100)      not null);";

            SqlCommand command = new SqlCommand(TSQLSourceCode, connection);
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected + " Rows Affected.");
        }
    }
}
