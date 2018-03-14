using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // referance - System.Data

/**
TriviaNation is a networked trivia game designed for use in 
classrooms. Class members are each in control of a nation on 
a map. The goal of the game is to increase the size of the nation 
by winning trivia challenges and defeating other class members 
in contested territories. The focus is on gamifying learning and 
making it an enjoyable experience.
@author Timothy McWatters
@author Keenal Shah
@author Randy Quimby
@author Wesley Easton
@author Wenwen Xu
@version 1.0
CEN3032    "TriviaNation" SEII- Group 1's class project
File Name: DataBaseOperations.cs 
    This class connects to the db, determines if a table exist in a database, creates and deletes a table in the database, 
    inserts and deletes a row in the table, and retrieves the row and the number of row.
*/

namespace TriviaNation
{
    public class DataBaseOperations
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
        /// Default constructor for the DataBaseOperations class
        /// </summary>
        public DataBaseOperations()
        {

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
        /// Retrieves the number of cols a specific Table has
        /// </summary>
        /// <param name="tableName">The name of the Table to place number of row inquiry</param>
        /// <returns name="numberOfColsInTable">The number of cols in this particular Table</param>
        public static int RetrieveNumberOfColsInTable(String tableName)
        {
            int numberOfColsInTable = 0;
            String TSQLSourceCode = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "';";

            using (SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numberOfColsInTable = reader.GetInt32(0);
                    }
                }
            }
            return numberOfColsInTable;
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
        /// <param name="deletionString">The String for deleting the row to DELETE from the Table</param>
        public static void DeleteRowFromTable(String deletionString)
        {
            SqlCommand command = null;
            String TSQLSourceCode = deletionString;
            command = new SqlCommand(TSQLSourceCode, s_connection);
            command.ExecuteNonQuery();
            Console.WriteLine("Deletion complete!");
        }
    }
}