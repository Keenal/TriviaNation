using System.Collections;
using System.Collections.Generic;
using TriviaNation.Models;
using TriviaNation.Models.Abstract;
using TriviaNation.Services.Abstract;

namespace TriviaNation.Services
{ 
    public class QuestionEvaluation : IQuestionEvaluation
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Change { get; set; }

        IDataBaseTable questionTable;
        IQuestion question;
        ITrivia trivia;

        public QuestionEvaluation(IQuestionPack questionPack)
        {
            //questionTable = new QuestionTable();
            question = new Questions();
            //trivia = new Trivia(questionTable, question);3
            trivia = new Trivia(questionPack);
        }

        public void setQuestionInfo()
        {
            question = trivia.GetRandomQuestion();
            Question = question.Question;
            Answer = question.Answer;
        }

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
