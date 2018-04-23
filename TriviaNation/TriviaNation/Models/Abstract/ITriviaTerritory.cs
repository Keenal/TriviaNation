using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public interface ITriviaTerritory
    {
        string territoryIndex { get; set; }

        string userName { get; set; }

        string color { get; set; }

        string playersTurn { get; set; }
    }
}
