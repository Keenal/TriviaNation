using System;
using System.Collections.Generic;

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
File Name: TriviaAdministration.cs 
*/

namespace TriviaNation
{
    /// <summary>
    /// A class to handle administrative tasks for questions and answers
    /// </summary>
    public class TriviaAdministration : ITriviaAdministration
    {
        /// <summary>
        /// IQuestion object for modeling question data
        /// </summary>
        private IQuestion question;
        /// <summary>
        /// IDataBaseTable object for storing and retrieving question data
        /// </summary>
        private IDataBaseTable database;

        /// <summary>
        /// Constructs a TriviaAdministration object with default values as instance fields
        /// </summary>
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
        public void AddQuestion(string query, string answer, string questionType, string questionPack)
        {
            question.Question = query;
            question.Answer = answer;
            question.QuestionType = questionType;
            question.QuestionPack = questionPack;
            database.InsertRowIntoTable(database.TableName, this);
        }

        /// <summary>
        /// Deletes a question from the database
        /// </summary>
        /// <param name="questionNumber">The user input question number that matches the row position of a question</param>
        public void DeleteQuestion(int questionNumber)
        {
            /* The commented code below will be in the handler.  Will be a graphical
             * list rather than from console.
             * 
             * ListQuestions();
             * Console.WriteLine("Enter question number to delete");
             * int questionNumber = Convert.ToInt32(Console.ReadLine());
             * DeleteQuestion(questionNumber);
             */
            SetRowToObject(questionNumber);
            database.DeleteRowFromTable(question.Question);
        }

        // Refactored code
        private void SetRowToObject(int questionNumber)
        {
            string tableRow = database.RetrieveTableRow(database.TableName, questionNumber);
            string[] split = tableRow.Split(separator: '\n');
            question.Question = split[0];
            question.Answer = split[1];
            question.QuestionType = split[2];
            question.QuestionPack = split[3];
        }

        // First get the question object to edit (in GUI will apply new strings to this object)
        public IQuestion GetEditableQuestion(int questionNumber)
        {
            DeleteQuestion(questionNumber);
            return question;
        }

        // Send newly edited question object back here to be inserted.  
        // Requires minimal code on the GUI side of things.  GUI side 
        // should only need to set edited questions to object and then 
        // send object here. Note* This really doesnt need testing?  Already tested in Add method?
        public void InsertEditedQuestion(IQuestion editedQuestion)
        {
            AddQuestion(editedQuestion.Question, editedQuestion.Answer, editedQuestion.QuestionType, editedQuestion.QuestionPack);
        }

        /// <summary>
        /// Returns all question data in the database in the form of a list of objects
        /// </summary>
        /// /// <returns>The list of question objects</returns>
        public IEnumerable<IQuestion> ListQuestions()
        {
            List<IQuestion> allQuestionModels = new List<IQuestion>();
            for (int i = 1; i <= database.RetrieveNumberOfRowsInTable(); i++)
            {
                SetRowToObject(i);
                IQuestion questionModel = new Questions
                {
                    Question = question.Question,
                    Answer = question.Answer,
                    QuestionType = question.QuestionType,
                    QuestionPack = question.QuestionPack
                };

                allQuestionModels.Add(questionModel);
            }

            return allQuestionModels;
        }

        /// <summary>
        /// Returns a list of question properties/values
        /// </summary>
        /// <returns>The list of properties/values</returns>
        public IEnumerable<string> GetValues()
        {
            List<string> questionValues = new List<string>
            {
                question.Question, question.Answer, question.QuestionType, question.QuestionPack
            };

            return questionValues;
        }

    }
}
