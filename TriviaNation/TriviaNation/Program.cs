using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaNation
{
    class Program
    {
        static void Main(string[] args)
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            IDataBaseTable QT = new QuestionTable();
            QT.CreateTable();
            Console.WriteLine(QT.TableExists());
            IQuestion question1 = new Questions();
            
            // abstract class that implements both interfaces
            DataEntry admin = new TriviaAdministration(question1, QT);
            admin.AddQuestion("Test", "Yup");
            admin.ListQuestions();


            ///////// NOT part of IDataEntry ////////////
            IQuestion question2 = new Questions();
            ITrivia trivia = new Trivia(QT, question2);
            Console.WriteLine(trivia.GetRandomQuestion());
            string answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));
            ////////////////////////////////////////////

            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
