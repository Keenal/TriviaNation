using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    /// <summary>
    /// A class to handle administrative tasks for questions and answers
    /// </summary>
    class TriviaAdministration : DataEntry
    {
        /// <summary>
        /// IQuestion object for modeling question data
        /// </summary>
        private IQuestion question;
        /// <summary>
        /// IDataBaseTable object for storing and retrieving question data
        /// </summary>
        private IDataBaseTable database;

        public TriviaAdministration()
        {
            this.question = null;
            this.database = null;
        }

        /// <summary>
        /// Constructs a TriviaAdministration object with database and question types as instance fields through use of interfaces 
        /// </summary>
        /// <param name="question">The question object</param>
        /// <param name="database">The database object related to questions</param>
        public TriviaAdministration(IQuestion question, IDataBaseTable database)
        {
            this.question = question;
            this.database = database;
        }

        /// <summary>
        /// Adds a question to the database
        /// </summary>
        /// <param name="query">The question</param>
        /// <param name="answer">The answer</param>
        public override void AddQuestion(string query, string answer)
        {
            question.Question = query;
            question.Answer = answer;
            database.InsertRowIntoTable(this);
        }

        public override void DeleteQuestion()
        {
            ListQuestions();
            Console.WriteLine("Enter question number to delete");
            int questionNumber = Convert.ToInt32(Console.ReadLine());
            String tableRow = database.RetrieveTableRow(questionNumber);
            String[] split = tableRow.Split(separator: '\n');
            question.Question = split[0];
            database.DeleteRowFromTable(question.Question);
        }

        /// <summary>
        /// Lists all questions and answers in the database
        /// </summary>
        public override void ListQuestions()
        {
            for (int i = 1; i <= database.RetrieveNumberOfRowsInTable(); i++)
            {
                Console.WriteLine(i + ". " + database.RetrieveTableRow(i));
            }    
        }

        public override IEnumerable<string> GetValues()
        {
            List<string> questionValues = new List<string>
            {
                question.Question,
                question.Answer
            };
            return questionValues;
        }
       
    }
}
