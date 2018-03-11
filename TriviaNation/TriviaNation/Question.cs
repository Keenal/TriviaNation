using System;
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
*/

namespace TriviaNation
{
    public class Question : IDataEntry
    {
        private String question = "";
        private String answer = "";

        public Question(String question, String answer)
        {
            this.question = question;
            this.answer = answer;
        }
 
        public IEnumerable<string> GetValues(){
            List<string> questionAndAnswer = new List<string>();
            questionAndAnswer.Add(question);
            questionAndAnswer.Add(answer);

            return questionAndAnswer;
        }
    }
}
