using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    /// <summary>
    /// Interface for the trivia class
    /// </summary>
    public interface ITrivia
    {
        string GetRandomQuestion();
        Boolean EvaluateAnswer(string answer);
    }
}
