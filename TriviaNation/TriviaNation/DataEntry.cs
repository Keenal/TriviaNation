using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    public abstract class DataEntry : IDataEntry, ITriviaAdministration
    {
        public abstract void AddQuestion(string question, string answer);
        public abstract void DeleteQuestion();
        public abstract void ListQuestions();
        public abstract IEnumerable<string> GetValues();
    }
}
