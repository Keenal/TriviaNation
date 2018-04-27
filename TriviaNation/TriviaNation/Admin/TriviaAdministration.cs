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
        /// IDataBaseTable object holding questionPack information in the database
        /// </summary>
        private IDataBaseTable questionPackTable;

        /// <summary>
        /// Constructs a TriviaAdministration object with database instance field through use of interfaces 
        /// </summary>
        public TriviaAdministration()
        {
            this.questionPackTable = new QuestionPackTable();
            this.QuestionPackList = new List<IQuestionPack>();
            PopulateListFromTable();
        }

        /// <summary>
        /// QuestionPackList Property
        /// </summary>
        public List<IQuestionPack> QuestionPackList { get => questionPackList; set => questionPackList = value; }

        /// <summary>
        /// Adds a question pack to the database as its own table, and adds it to the list of question packs in the 
        /// QuestionPackTable
        /// </summary>
        /// <param name="questionPackName">The question packs name</param>
        /// <param name="questionPointValue">The questionss point value</param>
        public IQuestionPack AddQuestionPack(string questionPackName, int questionPointValue)
        {
            //creates a new instance of a QuestionPack
            IQuestionPack questionPack = new QuestionPack(questionPackName, questionPointValue);

            //creates a new QuestionTable named for this QuestionPack
            IDataBaseTable questionTableDatabase = new QuestionTable(questionPackName);
            questionTableDatabase.CreateTable(questionTableDatabase.TableName, questionTableDatabase.TableCreationString);

            //insert this question pack into the master QuestionPackTable that includes all the active
            //question packs
            questionPackTable.InsertRowIntoTable(questionPackTable.TableName, questionPack);

            //adds the question pack to the questionPackList
            QuestionPackList.Add(questionPack);

            //returns the QuestionPack that was added
            return questionPack;
        }

        /// <summary>
        /// Deletes a questionPack from the database
        /// </summary>
        /// <param name="questionPackName">The name of the question pack to delete</param>
        public void DeleteQuestionPack(string questionPackName)
        {
            for (int i = 0; i < QuestionPackList.Count; i++)
            {
                if (QuestionPackList[i].QuestionPackName.Equals(questionPackName))
                {
                    DataBaseOperations.DeleteTable(questionPackName);
                    questionPackTable.DeleteRowFromTable(questionPackName);
                    QuestionPackList.Remove(QuestionPackList[i]);
                }
            }
        }

        /// <summary>
        /// Returns question pack from the database by questionPackName
        /// </summary>
        /// /// <returns>The questionPack</returns>
        public IQuestionPack RetrieveQuestionPackByName(string questionPackName)
        {
           for (int i = 0; i < QuestionPackList.Count; i++)
           {
              if (QuestionPackList[i].QuestionPackName.Equals(questionPackName))
              {
                 return QuestionPackList[i];
              }
           }
           Console.WriteLine("WARNING: Question Pack " + questionPackName + " does not exist");
           return null;
        }

        /// <summary>
        /// Returns all question data in the database in the form of a list of objects
        /// </summary>
        /// /// <returns>The list of question objects</returns>
        public IEnumerable<IQuestionPack> ListQuestionPacks()
        {
            return QuestionPackList;
        }

        /// <summary>
        /// populates a List<QuestionPack> from the database
        /// </summary>
        public void PopulateListFromTable()
        {
            for (int i = 1; i <= questionPackTable.RetrieveNumberOfRowsInTable(); i++)
            {
                IQuestionPack questionPackToAdd = SetRowToObject(i);
                QuestionPackList.Add(questionPackToAdd);
            }
        }

        /// <summary>
        /// retrives qeustion pack information from the database and creates a QuestionPack object with that data
        /// </summary>
        /// <param name="questionPackNumber"></param>
        /// <returns></returns>
        private IQuestionPack SetRowToObject(int questionPackNumber)
        {
            string tableRow = questionPackTable.RetrieveTableRow(questionPackTable.TableName, questionPackNumber);
            string[] split = tableRow.Split(separator: '\n');

            string questionPackName = split[0];
            string pointValueString = split[1];
            int pointValue = int.Parse(pointValueString);
            IQuestionPack questionPack = new QuestionPack(questionPackName, pointValue);
            return questionPack;
        }
    }
}
