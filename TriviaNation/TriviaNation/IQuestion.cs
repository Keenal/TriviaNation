using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    /// <summary>
    /// Interface for the Question class
    /// </summary>
    public interface IQuestion
    {
        string Question
        {
            get; set;
        }

        string Answer
        {
            get; set;
        }

        int PointValue
        {
            get; set;
        }
    }
}
