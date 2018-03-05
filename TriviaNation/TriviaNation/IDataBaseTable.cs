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

        void CreateTable();
        void InsertRowIntoTable(String columnOneValue, String columnTwoValue);
        String RetrieveTableRow(int rowNumber);
        void DeleteRowFromTable(int rowNumber);
    }
}
