using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation.Repository.Abstract
{
    public interface IUserTable
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

        int RetriveNumberOfColsInTable();

        void InsertRowIntoTable(String tableName, IDataEntry dataEntry);

        String RetrieveTableRow(String tableName, int rowNumber);

        String RetrieveTableRowsByCriteria(String tableName, String columnName, String matchingCriteria);

        void DeleteRowFromTable(String firstColumnText);

        string UpdateScore(string username, string newScore);
    }
}
