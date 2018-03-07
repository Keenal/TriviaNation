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
            get => s_connection;
            set => s_connection = value;
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
        /// <returns name="exists">True if table exists, False if table does not exist</returns>
        public static Boolean TableExists(String tableName)
        {
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tableName + "'";
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            SqlDataReader myReader = null;
            int count = 0;
    
            try
            {
                myReader = command.ExecuteReader();
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
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// CREATEs a TABLE in a database
        /// </summary>
        /// <param name="tableName">The name of the Table to create</param>
        /// <param name=""="tableCreationString">The string used to finish the SQL command used in creating this particular Table</param>
        public static void CreateTable(String tableName, String tableCreationString)
        {
            //DELETEs the table if it exists
            DeleteTable(tableName);

            //Builds the table creation String
            String TSQLSourceCode = "CREATE TABLE " + tableName + tableCreationString;

            //CREATEs the table
            SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Creation of " + tableName + " complete!");
        }

        /// <summary>
        /// DELETEs a Table
        /// </summary>
        /// <param name="tableName">The name of the Table to DELETE</param>
        public static void DeleteTable(String tableName)
        {
            String TSQLSourceCode = ("DROP TABLE IF EXISTS " + tableName + ";");

            SqlCommand deleteTableCommand = new SqlCommand(TSQLSourceCode, s_connection);
            deleteTableCommand.ExecuteNonQuery();
            Console.WriteLine("Deletion of " + tableName + " complete!");
        }

        /// <summary>
        /// Retrieves the number of rows a specific Table has
        /// </summary>
        /// <param name="tableName">The name of the Table to place number of row inquiry</param>
        /// <returns name="numberOfRowsInTable">The number of rows in this particular Table</param>
        public static int RetrieveNumberOfRowsInTable(String tableName)
        {
            int numberOfRowsInTable = 0;
            String TSQLSourceCode = "SELECT COUNT(*) FROM " + tableName + ";";

            using (SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numberOfRowsInTable = reader.GetInt32(0);
                    }
                }
            }
            return numberOfRowsInTable;
        }

        /// <summary>
        /// INSERTs a row into a Table 
        /// </summary>
        /// <param name="insertString">The String SQL command to Insert a row into a Table</param>
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
        /// <param name="rowToRetrieve">The SQL command to RETRIEVE a row from the Table</param>
        /// <returns name="retrievedRow">The row retrieved from the Table</returns>
        public static String RetrieveRowFromTable(String rowToRetrieve)
        {
            String retrievedRow = "";
            String TSQLSourceCode = rowToRetrieve;

            using (SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retrievedRow += reader.GetString(1) + "\n" + reader.GetString(2) + "\n";
                    }
                }
            }
            return retrievedRow;
        }

        /// <summary>
        /// DELETEs a row from a TABLE 
        /// </summary>
        /// <param name="rowToDelete">The number of the row to DELETE from the Table</param>
        public static void DeleteRowFromTable(String rowToDelete)
        {
            //implement in later sprint when required
        }
    }
}
