using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    class QuestionTable : IDataBaseTable
    {
        //name of this specific DataBase Table
        private const String tableName = "QuestionTable";
        //String used to create this specific Table
        private const String tableCreationString = "(question varchar(4000) not null PRIMARY KEY, answer varchar(4000) not null);";

        /// <summary>
        /// Default Constructor for the QuestionTable class
        /// </summary>
        public QuestionTable()
        {

        }

        /// <summary>
        /// Accessor for tableName
        /// </summary>
        public String TableName
        {
            get => tableName;
        }

        /// <summary>
        /// Accessor for tableCreationString
        /// </summary>
        public String TableCreationString
        {
            get => tableCreationString;
        }

        /// <summary>
        /// Checks to see if this Table table exists currently in the DataBase
        /// </summary>
        /// <param name="tableExists">True if the Table does exist in the DataBase, False if it does not</param>
        public Boolean TableExists()
        {
            return DataBaseOperations.TableExists(tableName);
        }

        /// <summary>
        /// Creates this Table
        /// </summary>
        public void CreateTable()
        {
            DataBaseOperations.CreateTable(tableName, tableCreationString);
        }

        /// <summary>
        /// Inserts a question and answer into the Table
        /// </summary>
        /// <param name="question">First column of the Table, the question to add</param>
        /// <param name="answer">Second column of the Table, the answer to add</param>
        public void InsertRowIntoTable(String question, String answer)
        {
            String insertString = "INSERT INTO " + TableName + "(question, answer) VALUES ('" + question + "', '" + answer + "');";
            DataBaseOperations.InsertIntoTable(insertString);
        }

        /// <summary>
        /// Retrieves the number of rows a table has
        /// </summary>
        /// <returns name="numberOfRowsInTheTable">The number of the rows in the Table</param>
        public int RetrieveNumberOfRowsInTable()
        {
            return DataBaseOperations.RetrieveNumberOfRowsInTable(TableName);
        }

        /// <summary>
        /// Retrieves a row from the Table
        /// </summary>
        /// <param name="rowNumber">The number of the row to retrieve from the Table</param>
        /// <returns name="retrievedRow">The row that was retrieved</param>
        public String RetrieveTableRow(int rowNumber)
        {
            String retrievedRow = DataBaseOperations.RetrieveRowFromTable("" +
                "SELECT * FROM" +
               "(" +
                "Select " +
                "Row_Number() Over (Order By question) As RowNum" +
                ", * " +
               "From QuestionTable" +
               ") t2 " +
               "where RowNum = " + rowNumber + ";");

            return retrievedRow;
        }

        /// <summary>
        /// Deletes a row from the Table
        /// </summary>
        /// <param name="rowNumber">The number of the row to DELETE from the Table</param>
        public void DeleteRowFromTable()
        {
            //finish writing this method during next sprint
            String rowToDelete = "";
            DataBaseOperations.DeleteRowFromTable(rowToDelete);
        }
    }
}