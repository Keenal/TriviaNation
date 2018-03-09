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
    public interface ITriviaAdministration
    {
        void AddQuestion(string question, string answer);
        void DeleteQuestion();
        void ListQuestions();
    }
}
