using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaNation.Models.Abstract;

namespace TriviaNation.Models
{
    public class QuestionPack : IQuestionPack
    {
        public string QuestionPackName { get; set; }

        public int PointValue { get; set; }

        public IDataBaseTable Database { get; set; }

        public List<IQuestion> QuestionPackQuestions { get; set; }

        /// <summary>
        /// Constructs a TriviaAdministration object with database and question types as instance fields through use of interfaces 
        /// </summary>
        /// <param name="questionPackName">The question object</param>
        /// <param name="database">The database object related to questions</param>
        public QuestionPack(String questionPackName, int pointValue)
        {
            //constructs the QuestionPack
            this.QuestionPackName = questionPackName;
            this.PointValue = pointValue;
            Database = new QuestionTable(questionPackName);

            //populates the List<Questions> for this QuestionPack
            this.QuestionPackQuestions = new List<IQuestion>();
            PopulateListFromTable();
        }

        public QuestionPack() {

        }

        /// <summary>
        /// Adds a question to the database
        /// </summary>
        /// <param name="qeustion">The question</param>
        /// <param name="answer">The answer</param>
        public void AddQuestion(string questionText, string answer, string questionType)
        {
            IQuestion question = new Questions(questionText, answer, questionType, PointValue, QuestionPackName);
            Database.InsertRowIntoTable(Database.TableName, question);
            QuestionPackQuestions.Add(question);
        }

        /// <summary>
        /// Deletes a question from the database by question list number
        /// </summary>
        /// <param name="questionNumber">The user input question number that matches the row position of a question</param>
        public void DeleteQuestion(int questionNumber)
        {
            if (questionNumber <= QuestionPackQuestions.Count)
            {
                IQuestion question = QuestionPackQuestions[questionNumber];
                Database.DeleteRowFromTable(question.Question);
                QuestionPackQuestions.Remove(question);
            }
            else
            {
                Console.WriteLine("Question Number Does Not Exist.");
            }            
        }

        /// <summary>
        /// Deletes a question from the database by question text
        /// </summary>
        /// <param name="questionNumber">The user input question number that matches the row position of a question</param>
        public void DeleteQuestion(string questionText)
        {
            IQuestion question; 
            for (int i = 0; i < QuestionPackQuestions.Count; i++)
            {
                question = QuestionPackQuestions[i];
                if (question.Question.Equals(questionText))
                {
                    Database.DeleteRowFromTable(question.Question);
                    QuestionPackQuestions.Remove(question);
                }
            }
        }

        // First get the question object to edit (in GUI will apply new strings to this object)
        public IQuestion GetEditableQuestion(int questionNumber)
        {
            if (questionNumber <= QuestionPackQuestions.Count)
            {
                IQuestion question = QuestionPackQuestions[questionNumber];
                Database.DeleteRowFromTable(question.Question);
                QuestionPackQuestions.Remove(question);
                return question;
            }
            else
            {
                Console.WriteLine("Question Number Does Not Exist.");
                return new Questions("", "", "", 0, "");
            }
        }

        // Send newly edited question object back here to be inserted.  
        // Requires minimal code on the GUI side of things.  GUI side 
        // should only need to set edited questions to object and then 
        // send object here. Note* This really doesnt need testing?  Already tested in Add method?
        public void InsertEditedQuestion(IQuestion editedQuestion)
        {
            AddQuestion(editedQuestion.Question, editedQuestion.Answer, editedQuestion.QuestionType);
        }

        /// <summary>
        /// Returns all question data in the QuestionPack in the form of a list of objects
        /// </summary>
        /// /// <returns>The list of question objects</returns>
        public IEnumerable<IQuestion> ListQuestions()
        {
            return QuestionPackQuestions;
        }

        /// <summary>
        /// Returns all question data in the database in the form of a list of objects
        /// </summary>
        /// /// <returns>The list of question objects</returns>
        public void PopulateListFromTable()
        {
            if (Database.TableExists(Database.TableName))
            {
                for (int i = 1; i <= Database.RetrieveNumberOfRowsInTable(); i++)
                {
                    IQuestion questionToAdd = SetRowToObject(i);
                    QuestionPackQuestions.Add(questionToAdd);
                }
            }
        }
            
        // Refactored code
        private IQuestion SetRowToObject(int questionNumber)
        {
            string tableRow = Database.RetrieveTableRow(Database.TableName, questionNumber);
            string[] split = tableRow.Split(separator: '\n');

            string questionText = split[0];
            string answer = split[1];
            string questionType = split[2];
            string pointValueString = split[3];
            string questionPackName = split[4];
            int pointValueInt = int.Parse(pointValueString); 

            IQuestion question = new Questions(questionText, answer, questionType, pointValueInt, questionPackName);
            return question;
        }

        /// <summary>
        /// Returns a list of question properties/values
        /// </summary>
        /// <returns>The list of properties/values</returns>
        public IEnumerable<string> GetValues()
        {
            List<string> questionValues = new List<string>
            {
                this.QuestionPackName, this.PointValue.ToString()
            };

            return questionValues;
        }
    }
}
