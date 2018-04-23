using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public interface ITerritoryAdministration : IDataEntry
    {
        void AddTerritory(string territoryIndex, string username, string color);

        void DeleteTerritory(string territoryIndex);

        void UpdateUserAndColor(string territoryIndex, string username, string color);

        List<TriviaTerritory> ListTerritories();

        bool CheckForTurn(string username);

        void DisableTurn(List<TriviaTerritory> territoryList, string username);
    }
}
