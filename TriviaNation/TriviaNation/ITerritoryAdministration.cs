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

        string ListTerritories();
    }
}
