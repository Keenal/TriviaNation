

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
File Name: Questions.cs 
*/
namespace TriviaNation
{
    /// <summary>
    /// Class to model questions, answers and their point values.
    /// </summary>
    public class Questions : IQuestion
    {
        /// <summary>
        /// A question in a trivia
        /// </summary>
        private string question;
        /// <summary>
        /// The answer to a question
        /// </summary>
        private string answer;
        /// <summary>
        /// The Package of Questions this question belongs to
        /// </summary>
        private string questionPack;

        // For future sprint.
        private int pointValue;

        /// <summary>
        /// The type of question
        /// </summary>
        private string questionType;

        /// <summary>
        /// Constructs a Question object with default values as instance fields
        /// </summary>
        public Questions(string question, string answer, string questionType, int pointValue, string questionPack)
        {
            this.question = question;
            this.answer = answer;
            this.questionType = questionType;
            this.pointValue = pointValue;
            this.questionPack = questionPack;
        }

        public Questions()
        {

        }

        /// <summary>
        /// Accessor and mutator Property for the question in a trivia
        /// </summary>
        public string Question
        {
            get
            {
                return question;
            }

            set
            {
                question = value;
            }
        }
        /// <summary>
        /// Accessor and mutator Property for the answer to a question
        /// </summary>
        public string Answer
        {
            get
            {
                return answer;
            }

            set
            {
                answer = value;
            }
        }

        /// <summary>
        /// Accessor and mutator Property for the answer value of a question
        /// </summary>
        public int PointValue
        {
            get
            {
                return pointValue;
            }

            set
            {
                pointValue = value;
            }
        }

        public string QuestionType
        {
            get
            {
                return questionType;
            }

            set
            {
                questionType = value;
            }
        }

        public string QuestionPack
        {
            get
            {
                return questionPack;
            }

            set
            {
                questionPack = value;
            }
        }

        /// <summary>
        /// Returns a list of question properties/values
        /// </summary>
        /// <returns>The list of properties/values</returns>
        public IEnumerable<string> GetValues()
        {
            List<string> questionValues = new List<string>
            {
                this.Question, this.Answer, this.QuestionType, this.PointValue.ToString(), this.QuestionPack
            };

            return questionValues;
        }
    }
}