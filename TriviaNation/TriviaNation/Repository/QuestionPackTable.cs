using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
File Name: QuestionPackTable.cs 
    This class creates the QuestionTable and has methods that can let you insert row into a table, insert coloumn into a table, delete a row and column and retrive those information. 
*/


namespace TriviaNation
{
    public class QuestionPackTable : IDataBaseTable
    {
        //name of this specific DataBase Table
        private const String tableName = "QuestionPackTable";
        //String used to create this specific Table
        private const String tableCreationString = "(questionPackName varchar(4000) not null PRIMARY KEY);";

        /// <summary>
        /// Default Constructor for the QuestionTable class
        /// </summary>
        public QuestionPackTable()
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
        public Boolean TableExists(String tableName)
        {
            return DataBaseOperations.TableExists(tableName);
        }

        /// <summary>
        /// Creates this Table
        /// </summary>
        public void CreateTable(String tableName, String tableCreationString)
        {
            DataBaseOperations.CreateTable(tableName, tableCreationString);
        }

        /// <summary>
        /// Inserts a row (containing question and answer) into the Table
        /// </summary>
        /// <param name="dataEntry">Instance of IDataEntry Interface containing qustion, answer</param>
        public void InsertRowIntoTable(String tableName, IDataEntry dataEntry)
        {
            List<String> list = new List<string>();
            list = (List<String>)dataEntry.GetValues();

            String questionPackName = list[0];

            String insertString = "INSERT INTO " + tableName + "(questionPackName) VALUES ('" + questionPackName + "');";
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
        public String RetrieveTableRow(String tableName, int rowNumber)
        {
            String retrievedRow = DataBaseOperations.RetrieveRowFromTable("" +
                "SELECT * FROM" +
               "(" +
                "Select " +
                "Row_Number() Over (Order By questionPackName) As RowNum" +
                ", * " +
               "From " + tableName +
               ") t2 " +
               "where RowNum = " + rowNumber + ";");

            return retrievedRow;
        }

        /// <summary>
        /// Retrieves rows from the Table using a set of defined criteria (ie the name of the question pack)
        /// </summary>
        /// <param name="tableName">The name of the table to retrieve the rows from</param>
        /// <param name="columnName">The name of the column we will be matching criteria from</param>
        /// <param name="matchingCriteria">The criteria we want to check the column for to match</param>
        /// <returns name="retrievedRows">The rows that were retrieved</param>
        public String RetrieveTableRowsByCriteria(String tableName, String columnName, String matchingCriteria)
        {
            String retrievedRows = DataBaseOperations.RetrieveRowFromTable("" +
                "SELECT * " +
                "FROM " + tableName + " " +
                "WHERE " + columnName + " = '" + matchingCriteria + "';");

            return retrievedRows;
        }

        /// <summary>
        /// Retrieves the number of columns a table contains
        /// </summary>
        /// <returns name ="numberOfColsInTable">The number of columns in a table</returns>
        public int RetriveNumberOfColsInTable()
        {

            return DataBaseOperations.RetrieveNumberOfColsInTable(TableName);
        }

        /// <summary>
        /// Deletes a row from the Table
        /// </summary>
        /// <param name="question">The question nomenclature of the row to DELETE from the Table</param>
        public void DeleteRowFromTable(String question)
        {
            String rowToDelete = ("DELETE FROM " + tableName + " WHERE questionPackName='" + question + "';");

            DataBaseOperations.DeleteRowFromTable(rowToDelete);
        }
    }
}
