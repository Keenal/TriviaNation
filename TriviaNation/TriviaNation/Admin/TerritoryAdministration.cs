using System;
using System.Collections.Generic;
using TriviaNation.Repository.Abstract;

namespace TriviaNation
{
    public class TerritoryAdministration : ITerritoryAdministration
    {
        private ITriviaTerritory _territory;
        private ITerritoryTable _database;

        public TerritoryAdministration()
        {
            _territory = null;

            _database = null;
        }

        public TerritoryAdministration(ITriviaTerritory territory, ITerritoryTable database)
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

        public void UpdateUserAndColor(string territoryIndex, string username, string color)
        {
            _database.UpdateUserAndColor(territoryIndex, username, color);
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
                TriviaTerritory territory = new TriviaTerritory();
                listOfTerritories = _database.RetrieveTableRow(_database.TableName, i);

                test = listOfTerritories.Split('\n');

                territory.territoryIndex = test[0];
                territory.userName = test[1];
                territory.color = test[2];
                territory.playersTurn = test[3];

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

        public bool CheckForTurn(string username)
        {
            if (_database.RetrieveNumberOfDistinctRowsInTable() < 2)
                return true;

            return _database.CheckForTurn(username);
        }

        public void DisableTurn(List<TriviaTerritory> territoryList, string username)
        {
            if(_database.RetrieveNumberOfDistinctRowsInTable() < 2)
            {
                _database.UpdatePlayerTurn(territoryList[0].territoryIndex, "1");
                return;
            }

            int count = 0;
            bool doNotAdd = false;
            foreach(TriviaTerritory territory in territoryList)
            {
                if(territory.userName.Equals(username) && territory.playersTurn.Equals("1"))
                {
                    _database.UpdatePlayerTurn(territory.territoryIndex, "0");
                    doNotAdd = true;
                }
                if(!doNotAdd)
                    count++;
            }

            for(int i = count; i < territoryList.Count; i++)
            {
                if (territoryList[i].userName != username)
                {
                    _database.UpdatePlayerTurn(territoryList[i].territoryIndex, "1");
                    break;
                }

                if(i == territoryList.Count - 1)
                {
                    i = -1;
                }
            }
        }
    }
}
