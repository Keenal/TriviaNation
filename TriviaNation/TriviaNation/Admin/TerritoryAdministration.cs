using System;
using System.Collections.Generic;
using TriviaNation.Repository.Abstract;

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
File Name: TerritoryAdministration.cs 
*/

namespace TriviaNation
{
    public class TerritoryAdministration : ITerritoryAdministration
    {
        private ITriviaTerritory _territory;
        private ITerritoryTable _database;

        /// <summary>
        /// default constructor
        /// </summary>
        public TerritoryAdministration()
        {
            _territory = null;

            _database = null;
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="territory"></param>
        /// <param name="database"></param>
        public TerritoryAdministration(ITriviaTerritory territory, ITerritoryTable database)
        {
            _territory = territory ??
                throw new ArgumentNullException(nameof(territory));

            _database = database ??
                throw new ArgumentNullException(nameof(database));
        }

        /// <summary>
        /// adds a territory 
        /// </summary>
        /// <param name="territoryIndex"></param>
        /// <param name="username"></param>
        /// <param name="color"></param>
        public void AddTerritory(string territoryIndex, string username, string color)  
        {
            _territory.territoryIndex = territoryIndex;
            _territory.userName = username;
            _territory.color = color;

            _database.InsertRowIntoTable(_database.TableName, this);
        }

        /// <summary>
        /// updates the user and the color of a territory
        /// </summary>
        /// <param name="territoryIndex"></param>
        /// <param name="username"></param>
        /// <param name="color"></param>
        public void UpdateUserAndColor(string territoryIndex, string username, string color)
        {
            _database.UpdateUserAndColor(territoryIndex, username, color);
        }
         
        /// <summary>
        /// deletes a territory from the database
        /// </summary>
        /// <param name="territoryIndex"></param>
        public void DeleteTerritory(string territoryIndex)
        {
            _database.DeleteRowFromTable(territoryIndex);
        }

        /// <summary>
        /// retreives a list of all the territories
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// gets the values of a territory to pass to the database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// checks for the users turn
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckForTurn(string username)
        {
            if (_database.RetrieveNumberOfDistinctRowsInTable() < 2)
                return true;

            return _database.CheckForTurn(username);
        }

        /// <summary>
        /// disables a users turn 
        /// </summary>
        /// <param name="territoryList"></param>
        /// <param name="username"></param>
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
