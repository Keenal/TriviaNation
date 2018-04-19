using System;
using System.Collections.Generic;
using TriviaNation.Models;
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
        private List<IQuestionPack> questionPackList;
        /// <summary>
        /// IQuestion object for modeling question data
        /// </summary>
        private IDataBaseTable questionPackTable;

        /// <summary>
        /// Constructs a TriviaAdministration object with database instance field through use of interfaces 
        /// </summary>
        public TriviaAdministration()
        {
            this.questionPackTable = new QuestionPackTable();
            this.questionPackList = new List<IQuestionPack>();
        }

        /// <summary>
        /// Adds a question to the database
        /// </summary>
        /// <param name="query">The question</param>
        /// <param name="answer">The answer</param>
        public IQuestionPack AddQuestionPack(string questionPackName, int questionPointValue)
        {
            //creates a new instance of a QuestionPack
            IQuestionPack questionPack = new QuestionPack(questionPackName, questionPointValue, null);

            //creates a new QuestionTable named for this QuestionPack
            String tableName = (questionPackName);
            IDataBaseTable newQuestionTable = new QuestionTable(tableName);
            newQuestionTable.CreateTable(newQuestionTable.TableName, newQuestionTable.TableCreationString);

            //adds the database table to the QuestionPack object
            questionPack.Database = newQuestionTable;

            //insert this question pack into the master QuestionPackTable that includes all the active
            //question pack names... later can add additional information like qeustion pack price and creater
            //ect... using the QuestionPack class' getValues() method
            questionPackTable.InsertRowIntoTable(questionPackTable.TableName, questionPack);

            //adds the question pack to the questionPackList
            questionPackList.Add(questionPack);

            //returns the QuestionPack that was added
            return questionPack;
        }

        /// <summary>
        /// Deletes a question from the database
        /// </summary>
        /// <param name="questionNumber">The user input question number that matches the row position of a question</param>
        public void DeleteQuestionPack(string questionPackName)
        {
            for (int i = 0; i < questionPackList.Count; i++)
            {
                if (questionPackList[i].Equals(questionPackName))
                {
                    string questionPackTableName = (questionPackList[i].QuestionPackName);
                    DataBaseOperations.DeleteTable(questionPackTableName);
                    questionPackList.Remove(questionPackList[i]);
                }
            }
        }

        /// <summary>
        /// Returns all question data in the database in the form of a list of objects
        /// </summary>
        /// /// <returns>The list of question objects</returns>
        public IEnumerable<IQuestionPack> ListQuestionPacks()
        {
            return questionPackList;
        }
    }
}
