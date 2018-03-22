using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation
{
    public interface ITerritory
    {
        string territoryIndex { get; set; }

        string userName { get; set; }

        string color { get; set; }
    }
}
