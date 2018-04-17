using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation.Models.Abstract

/// <summary>
/// Interface for the Teacher class
/// </summary>

{
    public interface ITeacher
    {
        double GameStartTime
        {
            get; set;
        }

        double GameEndTime
        {
            get; set; 
        }

        double QuestionStartTime
        {
            get; set;
        }

        double QuestionEndTime
        {
            get; set;
        }

    }
}
