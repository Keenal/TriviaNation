using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    /// <summary>
    /// Class to model questions, answers and their point values.
    /// </summary>
    class Questions : IQuestion
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
        /// Accessor and mutator for the question in a trivia
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
    }
}
