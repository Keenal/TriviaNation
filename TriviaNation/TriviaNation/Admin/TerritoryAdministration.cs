using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public class TerritoryAdministration : ITerritoryAdministration
    {
        private ITriviaTerritory _territory;
        private IDataBaseTable _database;

        public TerritoryAdministration()
        {
            _territory = null;

            _database = null;
        }

        public TerritoryAdministration(ITriviaTerritory territory, IDataBaseTable database)
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

        public List<TriviaTerritory> ListTerritories()
        {
            List<TriviaTerritory> territories = new List<TriviaTerritory>();
            string listOfTerritories = "";
            string[] test;
 
            for (int i = 1; i <= _database.RetrieveNumberOfRowsInTable(); i++)
            {
                // string marker = "cx" + i;
                TriviaTerritory territory = new TriviaTerritory();
                listOfTerritories = _database.RetrieveTableRow(_database.TableName, i);

                test = listOfTerritories.Split('\n');

                territory.territoryIndex = test[0];
                territory.userName = test[1];
                territory.color = test[2];

                territories.Add(territory);
            }

            return territories;
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
