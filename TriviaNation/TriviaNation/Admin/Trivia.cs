using System;
using TriviaNation.Models.Abstract;

/**
TriviaNation is a networked trivia game designed for use in
classrooms. Class members are each in control of a nation on
a map. The goal of the game is to increase the size of the nation 
by winning trivia challenges and defeating other class members 
in contested territories. The focus is on gamifying learning and
making it an enjoyable experience.

@author Timothy McWatters
@author Keenal Shah
@author Randy Quimby
@author Wesley Easton
@author Wenwen Xu
@version 1.0
CEN3032    "TriviaNation" SEII- Group 1's class project
File Name: Trivia.cs 
*/

namespace TriviaNation
{
    /// <summary>
    /// Class to handle the logic of a trivia game
    /// </summary>
    public class Trivia : ITrivia
    {
        /// <summary>
        /// IDataBaseTable object for storing and retrieving question data
        /// </summary>
        private IDataBaseTable database;

        /// <summary>
        /// IQuestion object for modeling question data
        /// </summary>
        private IQuestionPack questionPack;

        /// <summary>
        /// Random object for generating random integers 
        /// </summary>
        private Random random;

        /// <summary>
        /// question object for referancing this question
        /// </summary>
        private IQuestion question;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="database"></param>
        /// <param name="question"></param>
        public Trivia(IDataBaseTable database, IQuestion question)
        {
            this.database = null;
            this.questionPack = null;
            random = new Random();
            this.database = database;
            this.question = question;
        }

        /// <summary>
        /// Constructs a Trivia object with database, random generation and question objects as instance fields through use of IDataBaseTable and IQuestion interfaces 
        /// </summary>
        /// <param name="database">The database object related to questions</param>
        /// <param name="questionPack">The questionPack we are getting qeustions from</param>
        public Trivia(IQuestionPack questionPack)
        {
            this.questionPack = questionPack;
            database = new QuestionTable(questionPack.QuestionPackName);
            random = new Random();
        }

        /// <summary>
        /// Generates a random number within a range determinant on the number of questions in this QuestionPacks table
        /// </summary>
        /// <returns name="randomNum">The random number</returns>
        public int RandomGenerator()
        {
            int randomNum = random.Next(1, database.RetrieveNumberOfRowsInTable() + 1);
            return randomNum;
        }

        /// <summary>
        /// Returns a random question from the List of questions
        /// </summary>
        /// <returns name="question">The question from the List of questions</returns>
        public IQuestion GetRandomQuestion()
        {
            int n = RandomGenerator();
            question = questionPack.QuestionPackQuestions[n];

            return question;
        }

        /// <summary>
        /// Evaluates answer input to correct or wrong
        /// </summary>
        /// <param name="answer">The answer</param>
        /// <returns>boolean representation of if a problem is correct or wrong</returns>
        public Boolean EvaluateAnswer(string answer)
        {
            if (answer.Trim().Equals(question.Answer, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
                return false;
        }
    }
}
