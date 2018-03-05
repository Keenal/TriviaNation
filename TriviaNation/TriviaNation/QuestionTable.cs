using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    class QuestionTable : IDataBaseTable
    {
        //name of the Table
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
            get
            {
                return tableName;
            }
        }

        /// <summary>
        /// Accessor for tableCreationString
        /// </summary>
        public String TableCreationString
        {
            get
            {
                return tableCreationString;
            }
        }

        /// <summary>
        /// Creates a Table
        /// </summary>
        /// <param name="tableName">the name of the Table to create</param>
        /// <param name="tableCreationString">The String used in Table creation to define Table Parameters</param>
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
        /// Retrieves a row from the Table
        /// </summary>
        /// <param name="rowNumber">The number of the row to retrieve from the Table</param>
        /// <returns name="retrievedRow">The row that was retrieved</param>
        public String RetrieveTableRow(int rowNumber)
        {
            String retrievedRow = "";
 //NEED to correct this code to retrieve only the selected row
            retrievedRow = DataBaseOperations.RetrieveRowFromTable("SELECT * FROM " + TableName + ";");
//DELETE this WriteLine after method fixed
            Console.WriteLine(retrievedRow);
            return retrievedRow;
        }

        /// <summary>
        /// Deletes a row from the Table
        /// </summary>
        /// <param name="rowNumber">The number of the row to DELETE from the Table</param>
        public void DeleteRowFromTable(int rowNumber)
        {
            DataBaseOperations.DeleteRowFromTable(rowNumber);
        }
    }
}
