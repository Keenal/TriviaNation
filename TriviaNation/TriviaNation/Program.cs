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
            IQuestion question = new Questions();
            
            // abstract class that implements both interfaces
            DataEntry admin = new TriviaAdministration(question, QT);

            admin.AddQuestion("Test", "Yup");
            admin.AddQuestion("Working?", "Affirmitive");
            admin.AddQuestion("No more objects necessary?", "Fer Shizzle");
            admin.DeleteQuestion();
            admin.ListQuestions();

            ///////// NOT part of IDataEntry ////////////
            ITrivia trivia = new Trivia(QT, question);

            Console.WriteLine(trivia.GetRandomQuestion());
            string answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));
            Console.WriteLine(trivia.GetRandomQuestion());
            answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));
            ////////////////////////////////////////////

            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
