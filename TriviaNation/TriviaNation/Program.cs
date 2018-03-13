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
            QuestionTable QT = new QuestionTable();
            QT.CreateTable(QT.TableName, QT.TableCreationString);
            Console.WriteLine("The table exists: {0}", QT.TableExists(QT.TableName));
            IDataEntry question1 = new Question("This is question1", "This is answer1");
            IDataEntry question2 = new Question("This is question2", "This is answer2");
            IDataEntry question3 = new Question("This is question3", "This is answer3");
            QT.InsertRowIntoTable(question1);
            QT.InsertRowIntoTable(question2);
            QT.InsertRowIntoTable(question3);
            Console.WriteLine("The number of rows in this table are: {0}", QT.RetrieveNumberOfRowsInTable());
            Console.WriteLine(QT.RetrieveTableRow(1));
            Console.WriteLine(QT.RetrieveTableRow(2));
            Console.WriteLine(QT.RetrieveTableRow(3));
            Console.WriteLine("The number of cols in this table are: {0}", QT.RetriveNumberOfColsInTable());
            QT.DeleteRowFromTable("This is question1");
            Console.WriteLine("The number of rows in this table are now: {0}", QT.RetrieveNumberOfRowsInTable());
            Console.WriteLine(QT.RetrieveTableRow(1));
            Console.WriteLine(QT.RetrieveTableRow(2));
            Console.WriteLine(QT.RetrieveTableRow(3));

            Console.WriteLine("Press any key to end the program");
            Console.ReadKey();
        }
    }
}
