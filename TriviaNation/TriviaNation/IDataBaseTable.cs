using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    public interface IDataBaseTable
    {
        String TableName
        {
            get;
        }
        String TableCreationString
        {
            get;
        }

        Boolean TableExists();
        void CreateTable();
        int RetrieveNumberOfRowsInTable();
        void InsertRowIntoTable(String columnOneValue, String columnTwoValue);
        String RetrieveTableRow(int rowNumber);
        void DeleteRowFromTable(); // not yet implemented in DataBaseOperations until next sprint.
    }
}
