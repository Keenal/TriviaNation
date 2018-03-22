using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    public class Territory : ITerritory
    {
        public string territoryIndex { get; set; }

        public string userName { get; set; }

        public string color { get; set; }
    }
}
