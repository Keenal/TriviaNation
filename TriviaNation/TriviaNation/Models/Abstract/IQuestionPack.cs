using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation.Models.Abstract
{
    public interface IQuestionPack : IDataEntry
    {
        string QuestionPackName
        {
            get; set;
        }

        List<IQuestion> QuestionPackQuestions
        {
            get;
        }
        IDataBaseTable Database { get; set; }

        void PopulateListFromTable();
    }
}
