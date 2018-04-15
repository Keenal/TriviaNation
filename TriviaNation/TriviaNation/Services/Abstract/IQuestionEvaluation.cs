using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriviaNation.Services.Abstract
{
    public interface IQuestionEvaluation
    {
        string Question { get; set; }

        string Answer { get; set; }

        bool Change { get; set; }

        void setQuestionInfo();

        string evaluateAnswer(string answer);
    }
}
