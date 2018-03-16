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

        // For future sprint.
        private int pointValue;

        private string questionType;

        /// <summary>
        /// Constructs a Question object with default values as instance fields
        /// </summary>
        public Questions()
        {
            this.question = "";
            this.answer = "";
            this.pointValue = 0;
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
    }
}