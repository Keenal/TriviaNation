using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public class TerritoryAdministration : ITerritoryAdministration
    {
        private ITerritory _territory;
        private IDataBaseTable _database;

        public TerritoryAdministration()
        {
            _territory = null;

            _database = null;
        }

        public TerritoryAdministration(ITerritory territory, IDataBaseTable database)
        {
            _territory = territory ??
                throw new ArgumentNullException(nameof(territory));

            _database = database ??
                throw new ArgumentNullException(nameof(database));
        }

        public void AddTerritory(string territoryIndex, string username, string color)  
        {
            _territory.territoryIndex = territoryIndex;
            _territory.userName = username;
            _territory.color = color;

            _database.InsertRowIntoTable(_database.TableName, this);
        }

        public void DeleteTerritory(string territoryIndex)
        {
            _database.DeleteRowFromTable(territoryIndex);
        }
        public string ListTerritories()
        {
            string listOfTerritories = "";
            for (int i = 1; i <= _database.RetrieveNumberOfRowsInTable(); i++)
            {
                // string marker = "cx" + i;
                listOfTerritories = listOfTerritories + i + ". " + _database.RetrieveTableRow(_database.TableName, i);
            }

            return listOfTerritories;
        }

        public IEnumerable<string> GetValues()
        {
            List<string> territoryData = new List<string>
            {
                _territory.territoryIndex,
                _territory.userName,
                _territory.color
            };
            return territoryData;
        }

    }
}
