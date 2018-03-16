﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
File Name: Question.cs 
    This class creates a template for the questions created in the Question branch 
*/

namespace TriviaNation
{
    public class Questions : IDataEntry
    {
        private String question = "";
        private String answer = "";
        private String questionType = "";

        public Questions(String question, String answer, String questionType)
        {
            this.question = question;
            this.answer = answer;
            this.questionType = questionType;
        }

        public IEnumerable<string> GetValues()
        {
            List<string> questionAndAnswer = new List<string>();
            questionAndAnswer.Add(question);
            questionAndAnswer.Add(answer);
            questionAndAnswer.Add(questionType);

            return questionAndAnswer;
        }
    }
}