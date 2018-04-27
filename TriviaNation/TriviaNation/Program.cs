using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TriviaNation.Models;
using TriviaNation.Models.Abstract;
using TriviaNation.Repository.Abstract;

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
File Name: Program.cs 
*/

namespace TriviaNation
{
    class Program
    {
        static void Main(string[] args)
        {
            //connect to the DB
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            ITriviaAdministration triviaAdmin = new TriviaAdministration();

            //COMMENTED OUT CODE IS USED IF ALL THE TABLES ARE NEEDED TO BE 
            //RECREATED AND REPOPULATED IN CASE OF CHANGES WHILE DEBUGGING
            /*
            //creates a new QuestionPackTable to populate with QuestionPacks
            QuestionPackTable questionPackTable = new QuestionPackTable();
            questionPackTable.CreateTable(questionPackTable.TableName, questionPackTable.TableCreationString);
            //set up selection, creation or deletion of QuestionPacks
            ITriviaAdministration triviaAdmin = new TriviaAdministration(); // comment out triviaAdmin above if you need to use this
            //Create 3 QuestionPacks
            IQuestionPack qp1 = triviaAdmin.AddQuestionPack("questionPack1", 5);
            IQuestionPack qp2 = triviaAdmin.AddQuestionPack("questionPack2", 10);
            IQuestionPack qp3 = triviaAdmin.AddQuestionPack("questionPack3", 20);
            //populate questionPacks
            qp1.AddQuestion("Is this questionPack1, q1?", "yes~no~maybe~blue~yes", "MC");
            qp1.AddQuestion("Is this questionPack1, q2?", "no~yes~maybe~blue~yes", "MC");
            qp1.AddQuestion("Is this questionPack1, q3?", "maybe~no~yes~blue~yes", "MC");
            qp1.AddQuestion("Is this questionPack1, q4?", "blue~no~maybe~yes~yes", "MC");
            qp1.AddQuestion("Is this questionPack1, q5?", "5~4~3~6~5", "MC");
            qp1.AddQuestion("Is this questionPack1, q6?", "q4~q6~maybe~blue~q6", "MC");
            qp1.AddQuestion("Is this questionPack1, q7?", "yes~no~maybe~blue~yes", "MC");
            qp1.AddQuestion("Is this questionPack1, q8?", "YES~NO~MAYBE~BLUE~YES", "MC");
            qp1.AddQuestion("Is this questionPack1, q9?", "red~purple~fox~yes~yes", "MC");
            qp1.AddQuestion("Is this questionPack1, q10?", "Yes~No~Maybe~Blue~Yes", "MC");

            qp2.AddQuestion("Is this questionPack2, q1?", "yes~no~maybe~blue~yes", "MC");
            qp2.AddQuestion("Is this questionPack2, q2?", "no~yes~maybe~blue~yes", "MC");
            qp2.AddQuestion("Is this questionPack2, q3?", "maybe~no~yes~blue~yes", "MC");
            qp2.AddQuestion("Is this questionPack2, q4?", "blue~no~maybe~yes~yes", "MC");
            qp2.AddQuestion("Is this questionPack2, q5?", "5~4~3~6~5", "MC");
            qp2.AddQuestion("Is this questionPack2, q6?", "q4~q6~maybe~blue~q6", "MC");
            qp2.AddQuestion("Is this questionPack2, q7?", "yes~no~maybe~blue~yes", "MC");
            qp2.AddQuestion("Is this questionPack2, q8?", "YES~NO~MAYBE~BLUE~YES", "MC");
            qp2.AddQuestion("Is this questionPack2, q9?", "red~purple~fox~yes~yes", "MC");

            qp3.AddQuestion("Is this questionPack3, q1?", "yes~no~maybe~blue~yes", "MC");
            qp3.AddQuestion("Is this questionPack3, q2?", "no~yes~maybe~blue~yes", "MC");
            qp3.AddQuestion("Is this questionPack3, q3?", "maybe~no~yes~blue~yes", "MC");
            qp3.AddQuestion("Is this questionPack3, q4?", "blue~no~maybe~yes~yes", "MC");
            qp3.AddQuestion("Is this questionPack3, q5?", "5~4~3~6~5", "MC");
            qp3.AddQuestion("Is this questionPack3, q6?", "q4~q6~maybe~blue~q6", "MC");
            qp3.AddQuestion("Is this questionPack3, q7?", "yes~no~maybe~blue~yes", "MC");
            qp3.AddQuestion("Is this questionPack3, q8?", "YES~NO~MAYBE~BLUE~YES", "MC");
            qp3.AddQuestion("Is this questionPack3, q9?", "red~purple~fox~yes~yes", "MC");
            */

            //This section of code was for testing the listing of QuestionPacks, QuestionPack
            //Questions, and deleting QuestionPacks
            /*
            //list all QuestionPacks
            IEnumerable<IQuestionPack> qpList = triviaAdmin.ListQuestionPacks();
            foreach (IQuestionPack qp in qpList)
            {
                Console.WriteLine(qp.QuestionPackName);
                Console.WriteLine(qp.PointValue);
                Console.WriteLine();
            }

            //list all the Questions in qp1
            IQuestionPack qp1 = triviaAdmin.RetrieveQuestionPackByName("questionPack1");
            if (qp1 != null)
            {
                for (int i = 0; i < qp1.QuestionPackQuestions.Count; i++)
                {
                    Console.WriteLine(qp1.QuestionPackQuestions[i].Question);
                    Console.WriteLine(qp1.QuestionPackQuestions[i].Answer);
                    Console.WriteLine(qp1.QuestionPackQuestions[i].QuestionType);
                    Console.WriteLine(qp1.QuestionPackQuestions[i].PointValue);
                    Console.WriteLine(qp1.QuestionPackQuestions[i].QuestionPack);
                    Console.WriteLine();
                }
            }

            //list all the Questions in qp2
            IQuestionPack qp2 = triviaAdmin.RetrieveQuestionPackByName("questionPack2");
            if (qp2 != null)
            {
                for (int i = 0; i < qp2.QuestionPackQuestions.Count; i++)
                {
                    Console.WriteLine(qp2.QuestionPackQuestions[i].Question);
                    Console.WriteLine(qp2.QuestionPackQuestions[i].Answer);
                    Console.WriteLine(qp2.QuestionPackQuestions[i].QuestionType);
                    Console.WriteLine(qp2.QuestionPackQuestions[i].PointValue);
                    Console.WriteLine(qp2.QuestionPackQuestions[i].QuestionPack);
                    Console.WriteLine();
                }
            }

            //list all the Questions in qp3
            IQuestionPack qp3 = triviaAdmin.RetrieveQuestionPackByName("questionPack3");
            if (qp3 != null)
            {
                for (int i = 0; i < qp3.QuestionPackQuestions.Count; i++)
                {
                    Console.WriteLine(qp3.QuestionPackQuestions[i].Question);
                    Console.WriteLine(qp3.QuestionPackQuestions[i].Answer);
                    Console.WriteLine(qp3.QuestionPackQuestions[i].QuestionType);
                    Console.WriteLine(qp3.QuestionPackQuestions[i].PointValue);
                    Console.WriteLine(qp3.QuestionPackQuestions[i].QuestionPack);
                    Console.WriteLine();
                }
            }

            triviaAdmin.DeleteQuestionPack("questionPack3");
            //list all QuestionPacks
            IEnumerable<IQuestionPack> qpList1 = triviaAdmin.ListQuestionPacks();
            foreach (IQuestionPack qp in qpList1)
            {
                Console.WriteLine(qp.QuestionPackName);
                Console.WriteLine(qp.PointValue);
                Console.WriteLine();
            }

            //list all the Questions in qp3
            IQuestionPack qp4 = triviaAdmin.RetrieveQuestionPackByName("questionPack3");
            if (qp4 != null)
            {
                for (int i = 0; i < qp4.QuestionPackQuestions.Count; i++)
                {
                    Console.WriteLine(qp4.QuestionPackQuestions[i].Question);
                    Console.WriteLine(qp4.QuestionPackQuestions[i].Answer);
                    Console.WriteLine(qp4.QuestionPackQuestions[i].QuestionType);
                    Console.WriteLine(qp4.QuestionPackQuestions[i].PointValue);
                    Console.WriteLine(qp4.QuestionPackQuestions[i].QuestionPack);
                    Console.WriteLine();
                }
            }
            */

            //This block of code was for testing the retrieval of random questions
            //IQuestionPack qp2 = triviaAdmin.RetrieveQuestionPackByName("questionPack2");
            //ITrivia trivia = new Trivia(qp2);

            //Console.WriteLine(trivia.GetRandomQuestion().Question);
            //Console.WriteLine(trivia.GetRandomQuestion().Question);
            //Console.WriteLine(trivia.GetRandomQuestion().Question);
            //Console.WriteLine(trivia.GetRandomQuestion().Question);
            ITerritoryTable table= new TerritoryTable();
            ITriviaTerritory terr = new TriviaTerritory();
            ITerritoryAdministration admin = new TerritoryAdministration(terr, table);

            admin.DisableTurn(admin.ListTerritories(), "Mossy");
            //admin.CheckForTurn("Mossy");
            string a = "";
            //Console.ReadKey();
        }
    }
}
