using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    This is the main class and the classes, interfaces are tested from here. 
*/

namespace TriviaNation
{
    class Program
    {
        static void Main(string[] args)
        {
            /* This all LOOKS like a lot more code for program.cs, 
             * but actually it is much less.  Its very efficient, 
             * especially for future implementation. I'll take down
             * the comments and white space when everyone can see the 
             * changes. Trivia testing was also added.
             */
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            // Corrected for interface use (also required for my classes ~Randy)
            IDataBaseTable QT = new QuestionTable();
            QT.CreateTable(QT.TableName, QT.TableCreationString);
            Console.WriteLine("The table exists: {0}", QT.TableExists(QT.TableName));
            IQuestion question = new Questions();
            /* This (ITriviaAdministration) IS an IDataEntry interface.  
             * Interface inherits the IDataEntry inteferface. For future 
             * implementation. Will make interfaces granular, versatile, 
             * and easy to change. 
             */
            ITriviaAdministration admin = new TriviaAdministration(question, QT);

            // Same as InsertRowIntoTable method call had here before.
            admin.AddQuestion("Test", "Yup", "Question Type: MC (Test)");
            admin.AddQuestion("Working?", "Affirmitive", "Question Type: T/F (Test)");
            admin.AddQuestion("No more objects necessary?", "Fer Shizzle", "Question Type: Matching (Test)");
           
            Console.WriteLine("The number of rows in this table are: {0}", QT.RetrieveNumberOfRowsInTable());

            // Takes the place of all the RetriveTableRow method calls for output
            string test = admin.ListQuestions();
            Console.WriteLine(test);

            Console.WriteLine("The number of cols in this table are: {0}", QT.RetriveNumberOfColsInTable());

            // Replaces the "QT.DeleteRowFromTable("This is question1");"
            admin.DeleteQuestion(1);

            Console.WriteLine("The number of rows in this table are now: {0}", QT.RetrieveNumberOfRowsInTable());

            // Takes the place of all the RetrieveTableRow method calls for output
            test = admin.ListQuestions();
            Console.WriteLine(test);

            // Lastly, added testing for Trivia
            ITrivia trivia = new Trivia(QT, question);
            Console.WriteLine(trivia.GetRandomQuestion());
            string answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));
            Console.WriteLine(trivia.GetRandomQuestion());
            answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));
            // End Trivia Testing

            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
