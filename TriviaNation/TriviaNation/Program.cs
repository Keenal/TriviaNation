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
            QuestionTable QT = new QuestionTable();
            QT.CreateTable();
            DataBaseOperations.SeeIfTableExists(QT.TableName);
            QT.InsertRowIntoTable("This is a question1", "This is the answer1");
            QT.InsertRowIntoTable("This is a question2", "This is the answer2");
            QT.InsertRowIntoTable("This is a question3", "This is the answer3");
            QT.RetrieveTableRow(1);


            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
