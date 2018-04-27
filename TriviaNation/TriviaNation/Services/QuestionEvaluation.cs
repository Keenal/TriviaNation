using System.Collections;
using System.Collections.Generic;
using TriviaNation.Models;
using TriviaNation.Models.Abstract;
using TriviaNation.Services.Abstract;

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
File Name: QuestionEvaluation.cs 
*/

namespace TriviaNation.Services
{ 
    public class QuestionEvaluation : IQuestionEvaluation
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Change { get; set; }
        public int PointValue { get; set; }

        IQuestion question;
        ITrivia trivia;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="questionPack"></param>
        public QuestionEvaluation(IQuestionPack questionPack)
        {
            //questionTable = new QuestionTable();
            question = new Questions();
            //trivia = new Trivia(questionTable, question);3
            trivia = new Trivia(questionPack);
        }

        /// <summary>
        /// sets the questions information
        /// </summary>
        public void setQuestionInfo()
        {
            question = trivia.GetRandomQuestion();
            Question = question.Question;
            Answer = question.Answer;
            PointValue = question.PointValue;

        }

        /// <summary>
        /// evaluates a questions answer for correct or wrong
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public string evaluateAnswer(string answer)
        {
            string response = "Incorrect";
            Change = false;

            if(answer == Answer)
            {
                response = "Correct";
                Change = true;
            }

            return response;
        }
    }
}
