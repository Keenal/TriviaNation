using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation.Repository.Abstract
{
    public interface ITerritoryTable
    {
        String TableName
        {
            get;
        }
        String TableCreationString
        {
            get;
        }
        Boolean TableExists(String tableName);

        void CreateTable(String tableName, String tableCreationString);

        int RetrieveNumberOfRowsInTable();

        int RetrieveNumberOfDistinctRowsInTable();

        int RetriveNumberOfColsInTable();

        void InsertRowIntoTable(String tableName, IDataEntry dataEntry);

        String RetrieveTableRow(String tableName, int rowNumber);

        String RetrieveTableRowsByCriteria(String tableName, String columnName, String matchingCriteria);

        void DeleteRowFromTable(String firstColumnText);

        void UpdateUserAndColor(string territoryIndex, string username, string color);

        void UpdatePlayerTurn(string territoryIndex, string playerTurn);

        bool CheckForTurn(string username);
    }
}
