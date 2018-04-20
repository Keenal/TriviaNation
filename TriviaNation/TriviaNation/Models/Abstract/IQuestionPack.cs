using System;
using System.Collections.Generic;

namespace TriviaNation.Models.Abstract
{
    public interface IQuestionPack : IDataEntry
    {
        string QuestionPackName
        {
            get; set;
        }

        int PointValue
        {
            get; set;
        }

        List<IQuestion> QuestionPackQuestions
        {
            get;
        }
        IDataBaseTable Database { get; set; }

        void AddQuestion(string questionText, string answer, string questionType);

        void DeleteQuestion(string questionText);

        void DeleteQuestion(int questionNumber);

        IEnumerable<IQuestion> ListQuestions();

        void PopulateListFromTable();
    }
}
