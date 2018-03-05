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
        private static SqlConnection s_connection;

        /// <summary>
        /// Accessor and Mutator for s_connection
        /// </summary>
        public static SqlConnection Connection
        {
            get => s_connection; set => s_connection = value;
        }

        /// <summary>
        /// Connects to database based on hard coded database information
        /// </summary>
        public static void ConnectToDB()
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
                s_connection = new SqlConnection(cb.ConnectionString);
                s_connection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Determine if a Table exists in a database
        /// </summary>
        /// <param name="tableName">The name of the Table to check if exists</param>
        public static void SeeIfTableExists(String tableName)
        {
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tableName + "'";
            SqlCommand cmd = new SqlCommand(sqlString, s_connection);
            SqlDataReader myReader = null;
            int count = 0;

            try
            {
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                    count++;
                myReader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            if (count == 0)
            {
                Console.WriteLine("Table " + tableName + " doesnt exist");
            }
            else
            {
                Console.WriteLine("Table " + tableName + " exists");
            }
        }

        /// <summary>
        /// DELETEs a table
        /// </summary>
        /// <param name="tableName">The name of the Table to DELETE</param>
        public static void DeleteTable(String tableName)
        {
            String TSQLSourceCode;
            TSQLSourceCode = ("DROP TABLE IF EXISTS " + tableName + ";");

            SqlCommand deleteTableCommand = new SqlCommand(TSQLSourceCode, s_connection);
            deleteTableCommand.ExecuteNonQuery();
            Console.WriteLine("Deletion of " + tableName + " complete!");
        }

        /// <summary>
        /// CREATEs a TABLE in the database
        /// </summary>
        /// <param name="tableName">The name of the Table to create</param>
        public static void CreateTable(String tableName, String tableCreationString)
        {
            //DELETEs the table if it exists
            DeleteTable(tableName);

            //Builds the table creation String
            String TSQLSourceCode;
            TSQLSourceCode = "" +
            "CREATE TABLE " + tableName + tableCreationString;

            //CREATEs the table
            SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Creation of " + tableName + " complete!");
        }

        /// <summary>
        /// INSERTs a row into a Table 
        /// </summary>
        /// <param name="insertString">The String argument to Insert a row into a Table</param>
        public static void InsertIntoTable(String insertString)
        {
            String TSQLSourceCode = insertString;
            SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Insertion complete!");
        }

        /// <summary>
        /// RETRIEVEs a row from a Table
        /// </summary>
        /// <param name="rowToRetrieve">The row number to RETRIEVE from the Table</param>
        /// <returns name="retrievedRow">The row retrieved from the Table</returns>
        public static String RetrieveRowFromTable(String rowToRetrieve)
        {
            String retrievedRow = "";
            int i = 0;
            String TSQLSourceCode = rowToRetrieve;

            using (SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retrievedRow = reader.GetString(0) + "\n" + reader.GetString(1);
                    }
                }
            }
            return retrievedRow;
        }

        /// <summary>
        /// DELETEs a row from a TABLE 
        /// </summary>
        /// <param name="rowToDelete">The number of the row to DELETE from a Table</param>
        public static void DeleteRowFromTable(int rowToDelete)
        {
            //put code here
        }
    }
}
