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
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            IDataBaseTable QT = new QuestionTable();
            QT.CreateTable(QT.TableName, QT.TableCreationString);
            Console.WriteLine("The table exists: {0}", QT.TableExists(QT.TableName));
            IQuestion question = new Questions();
           
            ITriviaAdministration admin = new TriviaAdministration(question, QT);
            admin.AddQuestion("Test", "Yup", "Question Type: MC (Test)");
            admin.AddQuestion("Working?", "Affirmitive", "Question Type: T/F (Test)");
            admin.AddQuestion("No more objects necessary?", "Fer Shizzle", "Question Type: Matching (Test)");          
            Console.WriteLine("The number of rows in this table are: {0}", QT.RetrieveNumberOfRowsInTable());   
            string test = admin.ListQuestions();
            Console.WriteLine(test);
            Console.WriteLine("The number of cols in this table are: {0}", QT.RetriveNumberOfColsInTable());
            admin.DeleteQuestion("1");
            Console.WriteLine("The number of rows in this table are now: {0}", QT.RetrieveNumberOfRowsInTable());
            test = admin.ListQuestions();
            Console.WriteLine(test);

            /////////////////
            IDataBaseTable UT = new UserTable();
            UT.CreateTable(UT.TableName, UT.TableCreationString);
            Console.WriteLine("The table exists: {0}", UT.TableExists(QT.TableName));
            IUser user = new User();
            IUserAdministration userAdmin = new UserAdministration(user, UT);
            userAdmin.AddUser("Bob", "robert@uwf.edu", "password", "password", "65");
            userAdmin.AddUser("Sugar", "rcq1@uwf.edu", "abcd1234", "abcd1234", "107");
            test = userAdmin.ListUsers();
            Console.WriteLine(test);
            Console.WriteLine("The number of rows in USER table are now: {0}", UT.RetrieveNumberOfRowsInTable());

            IUserAuthentication validate = new UserAuthentication(UT, user);
            Console.WriteLine("Testing proper user name and password that exists: ");
            Boolean isAuthenticated = validate.AuthenticateUser("robert@uwf.edu", "password");
            Console.WriteLine("Authentication is: " + isAuthenticated);

            Console.WriteLine("Testing invalid user name and password that does not exist: ");
            isAuthenticated = validate.AuthenticateUser("Bobby", "password");
            Console.WriteLine("Authentication is: " + isAuthenticated);

            Console.WriteLine("Testing invalid confirmation password:");
            Boolean flag = userAdmin.AddUser("Phil", "tiger@uwf.edu", "house", "home", "107");
            Console.WriteLine("Password Confirmed? " + flag);
            Console.WriteLine("The number of rows in USER table are now: {0}", UT.RetrieveNumberOfRowsInTable());
            userAdmin.DeleteUser("1");
            Console.WriteLine("The number of rows in USER table are now: {0}", UT.RetrieveNumberOfRowsInTable());

            ////////////////

            Console.WriteLine("Testing Trivia Now");
            ITrivia trivia = new Trivia(QT, question);
            Console.WriteLine(trivia.GetRandomQuestion());
            string answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));
            Console.WriteLine(trivia.GetRandomQuestion());
            answer = Console.ReadLine();
            Console.WriteLine("Your answer is: " + trivia.EvaluateAnswer(answer));

            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
