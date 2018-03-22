using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public class TerritoryTable : IDataBaseTable
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

            string insertString = "INSERT INTO " + tableName + "(territoryIndex, username, color) VALUES ('" + territoryIndex + "', '" + username + "', '" + color + "');";
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
             "Row_Number() Over (Order By username) As RowNum" +
             ", * " +
            "From " + tableName +
            ") t2 " +
            "where RowNum = " + rowNumber + ";");

            return retrievedRow;
        }

        public void DeleteRowFromTable(string territoryIndex)
        {
            string rowToDelete = ("DELETE FROM " + tableName + " WHERE territoryIndex='" + territoryIndex + "';");

            DataBaseOperations.DeleteRowFromTable(rowToDelete);
        }
    }
}
