using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriviaNation.Repository.Abstract;

namespace TriviaNation
{
    public class TerritoryTable : ITerritoryTable
    {
        private const string tableName = "Territories";

        private const string tableCreationString = "(territoryIndex varchar(50) not null PRIMARY KEY, username varchar(4000), color varchar(50));";

        public TerritoryTable()
        {

        }

        public string TableName
        {
            get => tableName;
        }

        public string TableCreationString
        {
            get => tableCreationString;
        }

        public Boolean TableExists(string tableName)
        {
            return DataBaseOperations.TableExists(tableName);
        }

        public void CreateTable(string tableName, string tableCreationString)
        {
            DataBaseOperations.CreateTable(tableName, tableCreationString);
        }

        public int RetrieveNumberOfRowsInTable()
        {
            return DataBaseOperations.RetrieveNumberOfRowsInTable(TableName);
        }

        public int RetrieveNumberOfDistinctRowsInTable()
        {
            return DataBaseOperations.RetrieveNumberOfDistinctRowsInTable(TableName);
        }

        public int RetriveNumberOfColsInTable()
        {
            return DataBaseOperations.RetrieveNumberOfColsInTable(TableName);
        }

        public void InsertRowIntoTable(string tableName, IDataEntry dataEntry)
        {
            List<string> list = new List<string>();
            list = (List<string>)dataEntry.GetValues();

            string territoryIndex = list[0];
            string username = list[1];
            string color = list[2];

            string insertString = "INSERT INTO " + tableName + "(territoryIndex, username, color, playersTurn) VALUES ('" + territoryIndex + "', '" + username + "', '" + color + "', 0);";
            DataBaseOperations.InsertIntoTable(insertString);
        }

        //public void AddUserToTerritory(string territoryIndex, string username, string color)
        //{
        //    string insertString = ("INSERT INTO " + tableName + "(username, color) VALUES ('" + username + "', '" + color + "') WHERE territoryIndex='" + territoryIndex + "';");
        //}

        public string RetrieveTableRow(string tableName, int rowNumber)
        {
            string retrievedRow = DataBaseOperations.RetrieveRowFromTable("" +
             "SELECT * FROM" +
            "(" +
             "Select " +
             "Row_Number() Over (Order By territoryIndex) As RowNum" +
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

        public void DeleteRowFromTable(string territoryIndex)
        {
            string rowToDelete = ("DELETE FROM " + tableName + " WHERE territoryIndex='" + territoryIndex + "';");

            DataBaseOperations.DeleteRowFromTable(rowToDelete);
        }

        public void UpdateUserAndColor(string territoryIndex, string username, string color)
        {
            string update = ("UPDATE " + tableName + " SET username = \'" + username + "\', color = \'" + color + "\' WHERE territoryIndex = " + territoryIndex);

            DataBaseOperations.UpdateTable(update);
        }

        public void UpdatePlayerTurn(string territoryIndex, string playerTurn)
        {
            string update = ("UPDATE " + tableName + " SET playersTurn = \'" + playerTurn + "\' WHERE territoryIndex = " + territoryIndex);

            DataBaseOperations.UpdateTable(update);
        }

        public bool CheckForTurn(string username)
        {
            string check = ("SELECT COUNT(playersTurn) FROM " + tableName + " WHERE username = \'" + username + "\' AND playersTurn = \'1\'");

            return DataBaseOperations.CheckForTurn(check);
        }
    }
}
