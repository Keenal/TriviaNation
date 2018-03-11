using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    /// <summary>
    /// Interface for the TriviaAdministration class
    /// </summary>
    public interface ITriviaAdministration : IDataEntry
    {
        void AddQuestion(string question, string answer);
        void DeleteQuestion(int questionNumber);
        string ListQuestions();
    }
}
