using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    /// <summary>
    /// Class to handle the logic of a trivia game
    /// </summary>
    class Trivia : ITrivia
    {
        /// <summary>
        /// IDataBaseTable object for storing and retrieving question data
        /// </summary>
        private IDataBaseTable database;
        /// <summary>
        /// IQuestion object for modeling question data
        /// </summary>
        private IQuestion questions;
        /// <summary>
        /// Random object for generating random integers 
        /// </summary>
        private Random random;
        
        /// <summary>
        /// Constructs a Trivia object with database and question types as instance fields through use of interfaces 
        /// </summary>
        /// <param name="database">The database object related to questions</param>
        /// <param name="questions">The question object</param>
        public Trivia(IDataBaseTable database, IQuestion questions)
        {
            this.database = database;
            this.questions = questions;
            random = new Random();
        }

        /// <summary>
        /// Generates a random number within a range determinant on the number of questions in the database table
        /// </summary>
        /// <returns name="randomNum">The random number</returns>
        public int RandomGenerator()
        {
            int randomNum = random.Next(1, database.RetrieveNumberOfRowsInTable() + 1);
            return randomNum;
        }

        /// <summary>
        /// Returns a random question from the database
        /// </summary>
        /// <returns name="randomNum">The question</returns>
        public string GetRandomQuestion()
        {
            int n = RandomGenerator();
            string retrieveRow = database.RetrieveTableRow(n);
            string[] split = retrieveRow.Split(separator: '\n');
            questions.Question = split[0];
            questions.Answer = split[1];
            return questions.Question;
        }

        /// <summary>
        /// Evaluates answer input
        /// </summary>
        /// <param name="answer">The answer</param>
        /// <returns></returns>
        public Boolean EvaluateAnswer(string answer)
        {
            if (answer.Trim().Equals(questions.Answer, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
                return false;

            // all questions should have a flag for what kind of question it is (IE "m" for multiple choice)
            // Will need to overide and/or add more evaluateAnswer methods for different answer formats
            // place a call to here in the handler
        }

    }
}
