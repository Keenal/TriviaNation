﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
File Name: ITeacher.cs 
*/

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
