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
            /////////////////////////////////////////***********
            IQuestion question = new Questions();
            ITriviaAdministration admin = new TriviaAdministration(question, QT);
            ITrivia trivia = new Trivia(QT, question);
            admin.AddQuestion("Testing?", "Yes");
            admin.AddQuestion("Second try?", "Affirmitive");
            admin.AddQuestion("Working?", "Yup");
            admin.ListQuestions();
            Console.WriteLine(trivia.GetRandomQuestion());
            string answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));
            //////////////////////////////////////////////********

            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
